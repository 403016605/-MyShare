﻿#region using

using System;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Linq;
using System.Reflection;

#endregion

namespace MyShare.Kernel.Infrastructure
{
    internal class PrivateReflectionDynamicObject : DynamicObject
    {
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;

        private static readonly ConcurrentDictionary<int, CompiledMethodInfo> CachedMembers =
            new ConcurrentDictionary<int, CompiledMethodInfo>();

        public object RealObject { get; set; }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var methodname = binder.Name;
            var type = RealObject.GetType();
            var hash = 23;
            hash = hash * 31 + type.GetHashCode();
            hash = hash * 31 + methodname.GetHashCode();
            var argtypes = new Type[args.Length];
            for (var i = 0; i < args.Length; i++)
            {
                var argtype = args[i].GetType();
                argtypes[i] = argtype;
                hash = hash * 31 + argtype.GetHashCode();
            }
            var method = CachedMembers.GetOrAdd(hash, x =>
            {
                var m = GetMember(type, methodname, argtypes);
                return m == null ? null : new CompiledMethodInfo(m, type);
            });
            result = method?.Invoke(RealObject, args);

            return true;
        }

        private static MethodInfo GetMember(Type type, string name, Type[] argtypes)
        {
            while (true)
            {
                var member = type.GetMethods(BindingFlags)
                    .FirstOrDefault(m => m.Name == name && m.GetParameters()
                                             .Select(p => p.ParameterType).SequenceEqual(argtypes));

                if (member != null)
                {
                    return member;
                }

                var t = type.GetTypeInfo().BaseType;
                if (t == null)
                {
                    return null;
                }

                type = t;
            }
        }
    }
}