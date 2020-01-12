using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Meeg.Configuration
{
    internal static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            if (type == null)
            {
                return Enumerable.Empty<PropertyInfo>();
            }

            return type.GetTypeInfo().GetAllProperties();
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(this TypeInfo type)
        {
            var allProperties = new List<PropertyInfo>();

            do
            {
                allProperties.AddRange(type.DeclaredProperties);
                type = type.BaseType.GetTypeInfo();
            }
            while (type != typeof(object).GetTypeInfo());

            return allProperties;
        }

        public static Type FindOpenGenericInterface(this Type actual, Type openGenericType)
        {
            if (actual.ImplementsOpenGenericInterface(openGenericType))
            {
                return actual;
            }

            TypeInfo actualTypeInfo = actual.GetTypeInfo();
            IEnumerable<Type> interfaces = actualTypeInfo.ImplementedInterfaces;

            foreach (Type interfaceType in interfaces)
            {
                if (interfaceType.ImplementsOpenGenericInterface(openGenericType))
                {
                    return interfaceType;
                }
            }

            return null;
        }

        public static bool ImplementsOpenGenericInterface(this Type actual, Type openGenericType)
        {
            TypeInfo actualTypeInfo = actual.GetTypeInfo();

            return actualTypeInfo.IsGenericType && actual.GetGenericTypeDefinition() == openGenericType;
        }

        public static object ConvertValue(this Type type, string value)
        {
            TryConvertValue(type, value, out object result, out Exception error);

            if (error != null)
            {
                throw error;
            }

            return result;
        }

        public static bool TryConvertValue(this Type type, string value, out object result, out Exception error)
        {
            error = null;
            result = null;

            if (type == typeof(object))
            {
                result = value;

                return true;
            }

            if (type.ImplementsOpenGenericInterface(typeof(Nullable<>)))
            {
                if (string.IsNullOrEmpty(value))
                {
                    return true;
                }

                return TryConvertValue(Nullable.GetUnderlyingType(type), value, out result, out error);
            }

            TypeConverter converter = TypeDescriptor.GetConverter(type);

            if (converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    result = converter.ConvertFromInvariantString(value);
                }
                catch (Exception ex)
                {
                    error = new InvalidOperationException($"Failed converting string '{value}' to type '{type.FullName}' with type converter '{converter.GetType().FullName}''", ex);
                }

                return true;
            }

            return false;
        }

        public static bool HasDefaultConstructor(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            return type.GetTypeInfo().HasDefaultConstructor();
        }

        public static bool HasDefaultConstructor(this TypeInfo typeInfo)
        {
            return typeInfo.DeclaredConstructors.Any(ctor =>
                ctor.IsPublic && ctor.GetParameters().Length == 0
            );
        }

        public static Array CreateArrayInstance(this Type type, int length)
        {
            if (!type.IsArray)
            {
                throw new ArgumentException($"Type is not an array: {type.FullName}", nameof(type));
            }

            return type.GetTypeInfo().CreateArrayInstance(length);
        }

        public static Array CreateArrayInstance(this TypeInfo typeInfo, int length)
        {
            Type elementType = typeInfo.GetElementType();

            if (elementType == null)
            {
                throw new InvalidOperationException($"Element type is null: {typeInfo.FullName}");
            }

            return Array.CreateInstance(elementType, length);
        }
    }
}
