using Microsoft.Extensions.DependencyInjection;

namespace MyShare.Kernel.Extension
{
    internal interface ISelector
    {
        void Populate(IServiceCollection services, RegistrationStrategy options);
    }
}