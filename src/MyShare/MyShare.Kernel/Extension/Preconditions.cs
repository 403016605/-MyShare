using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace MyShare.Kernel.Extension
{
    [DebuggerStepThrough]
    internal static class Preconditions
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>([NoEnumeration] T value, [InvokerParameterName, NotNull] string parameterName)
            where T : class
        {
            if (!ReferenceEquals(value, null)) return value;
            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentNullException(parameterName);
        }

        [ContractAnnotation("value:null => halt")]
        public static string NotEmpty(string value, [InvokerParameterName, NotNull] string parameterName)
        {
            if (ReferenceEquals(value, null))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            if (value.Length != 0) return value;
            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentException("String value cannot be null.", parameterName);
        }

        public static TEnum IsDefined<TEnum>(TEnum value, [InvokerParameterName, NotNull] string parameterName) where TEnum : struct
        {
            if (Enum.IsDefined(typeof(TEnum), value)) return value;
            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentOutOfRangeException(parameterName);
        }
    }
}
