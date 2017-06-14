#region using

using MyShare.Kernel.Domain;

#endregion

namespace MyShare.Kernel.Infrastructure
{
    internal static class AggregateRootExtension
    {
        public static dynamic AsDynamic(this AggregateRoot o)
        {
            return new PrivateReflectionDynamicObject {RealObject = o};
        }
    }
}