using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MyShare.Kernel.Extension
{
    internal class TypeSourceSelector : ITypeSourceSelector, ISelector
    {
        private List<ISelector> Selectors { get; } = new List<ISelector>();

        /// <inheritdoc />
        public IImplementationTypeSelector FromAssemblyOf<T>()
        {
            return InternalFromAssembliesOf(new[] { typeof(T).GetTypeInfo() });
        }

        public IImplementationTypeSelector FromAssembliesOf(params Type[] types)
        {
            Preconditions.NotNull(types, nameof(types));

            return InternalFromAssembliesOf(types.Select(x => x.GetTypeInfo()));
        }

        public IImplementationTypeSelector FromAssembliesOf(IEnumerable<Type> types)
        {
            Preconditions.NotNull(types, nameof(types));

            return InternalFromAssembliesOf(types.Select(t => t.GetTypeInfo()));
        }

        public IImplementationTypeSelector FromAssemblies(params Assembly[] assemblies)
        {
            Preconditions.NotNull(assemblies, nameof(assemblies));

            return InternalFromAssemblies(assemblies);
        }

        public IImplementationTypeSelector FromAssemblies(IEnumerable<Assembly> assemblies)
        {
            Preconditions.NotNull(assemblies, nameof(assemblies));

            return InternalFromAssemblies(assemblies);
        }

        public IServiceTypeSelector AddTypes(params Type[] types)
        {
            Preconditions.NotNull(types, nameof(types));

            var selector = new ImplementationTypeSelector(this, types);

            Selectors.Add(selector);

            return selector.AddClasses();
        }

        public IServiceTypeSelector AddTypes(IEnumerable<Type> types)
        {
            Preconditions.NotNull(types, nameof(types));

            var selector = new ImplementationTypeSelector(this, types);

            Selectors.Add(selector);

            return selector.AddClasses();
        }

        void ISelector.Populate(IServiceCollection services, RegistrationStrategy registrationStrategy)
        {
            foreach (var selector in Selectors)
            {
                selector.Populate(services, registrationStrategy);
            }
        }

        private IImplementationTypeSelector InternalFromAssembliesOf(IEnumerable<TypeInfo> typeInfos)
        {
            return InternalFromAssemblies(typeInfos.Select(t => t.Assembly));
        }

        private IImplementationTypeSelector InternalFromAssemblies(IEnumerable<Assembly> assemblies)
        {
            return AddSelector(assemblies.SelectMany(asm => asm.DefinedTypes).Select(x => x.AsType()));
        }

        private IImplementationTypeSelector AddSelector(IEnumerable<Type> types)
        {
            var selector = new ImplementationTypeSelector(this, types);

            Selectors.Add(selector);

            return selector;
        }
    }
}
