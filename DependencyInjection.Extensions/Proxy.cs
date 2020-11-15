using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

/**
 * https://docs.microsoft.com/en-us/dotnet/framework/reflection-and-codedom/how-to-define-a-generic-method-with-reflection-emit
 */
namespace DependencyInjection.Extensions
{
    public static class Proxy
    {
        private static readonly ConcurrentDictionary<Type, Type> Types = new ConcurrentDictionary<Type, Type>();

        public static T CreateInstance<T,F>(F value)
            where T : IMark<F>
        {
            var t = typeof(T);
            if (!t.IsInterface)
            {
                throw new ArgumentException();
            }

            var intfs = t.GetInterfaces()
                .Count(a => !(typeof(IDisposable).IsAssignableFrom(a) || typeof(IMark<F>).IsAssignableFrom(a)));
            if (intfs != 0)
            {
                throw new ArgumentException();
            }

            var type = CreateType<T,F>();

            return (T)Activator.CreateInstance(type, new object[] { value });
        }

        public static Type CreateType<T, F>()
            where T : IMark<F>
        {
            return Types.GetOrAdd(typeof(T), (_) =>
            {
                var tb = CreateTypeBuilder<T, F>();

                DefineContsructor<F>(tb);

                return tb.CreateType();
            });
        }

        private static TypeBuilder CreateTypeBuilder<T,F>()
            where T : IMark<F>
        {
            var type = typeof(T);
            var baseType = typeof(Mark<F>);
            var typeSignature = "Proxy_" + type.Name;
            var an = new AssemblyName(typeSignature + "_" + Guid.NewGuid().ToString("N"));
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            var typeBuilder = moduleBuilder.DefineType(
                typeSignature,
                TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit | TypeAttributes.AutoLayout,
                baseType);

            typeBuilder.AddInterfaceImplementation(type);

            return typeBuilder;
        }

        private static void DefineContsructor<F>(TypeBuilder typeBuilder)
        {
            const MethodAttributes attrs = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
            var baseTypeCtor = typeof(Mark<F>).GetConstructors()[0];
            var baseTypeCtorParams = baseTypeCtor.GetParameters().Select(p => p.ParameterType).ToArray();
            var c = typeBuilder.DefineConstructor(
                attrs,
                CallingConventions.Standard,
                new[] { typeof(F) });

            var il = c.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);

            il.Emit(OpCodes.Call, baseTypeCtor);
            il.Emit(OpCodes.Ret);
        }
    }
}