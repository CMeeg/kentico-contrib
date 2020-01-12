using System;

namespace Meeg.Configuration.Tests
{
    public class ComplexOptions
    {
        public ComplexOptions()
        {
            Nested = new NestedOptions();
            Virtual = "complex";
        }

        public NestedOptions Nested { get; set; }
        public int Integer { get; set; }
        public bool Boolean { get; set; }
        public virtual string Virtual { get; set; }
        public object Object { get; set; }

        public string PrivateSetter { get; private set; }
        public string ProtectedSetter { get; protected set; }
        public string InternalSetter { get; internal set; }
        public static string StaticProperty { get; set; }

        private string PrivateProperty { get; set; }
        internal string InternalProperty { get; set; }
        private string ProtectedProperty { get; set; }

        private string ProtectedPrivateSet { get; set; }

        private string PrivateReadOnly { get; }
        internal string InternalReadOnly { get; }
        private string ProtectedReadOnly { get; }

        public string ReadOnly => null;
    }

    public class NestedOptions
    {
        public int Integer { get; set; }
    }

    public class DerivedOptions : ComplexOptions
    {
        public override string Virtual
        {
            get => base.Virtual;
            set => base.Virtual = "Derived:" + value;
        }
    }

    public class NullableOptions
    {
        public bool? MyNullableBool { get; set; }
        public int? MyNullableInt { get; set; }
        public DateTime? MyNullableDateTime { get; set; }
    }

    public class EnumOptions
    {
        public UriKind UriKind { get; set; }
    }

    public class GenericOptions<T>
    {
        public T Value { get; set; }
    }

    public class OptionsWithNesting
    {
        public NestedOptions Nested { get; set; }

        public class NestedOptions
        {
            public int Value { get; set; }
        }
    }

    public class ConfigurationInterfaceOptions
    {
        public IAppConfigurationSection Section { get; set; }
    }

    public class DerivedOptionsWithSection : DerivedOptions
    {
        public IAppConfigurationSection DerivedSection { get; set; }
    }

    public interface ISomeInterface
    {
    }

    public class ClassWithoutPublicConstructor
    {
        private ClassWithoutPublicConstructor()
        {
        }
    }

    public class ThrowsWhenActivated
    {
        public ThrowsWhenActivated()
        {
            throw new Exception();
        }
    }

    public class NestedOptions1
    {
        public NestedOptions2 NestedOptions2Property { get; set; }
    }

    public class NestedOptions2
    {
        public ISomeInterface ISomeInterfaceProperty { get; set; }
    }

    public class TestOptions
    {
        public ISomeInterface ISomeInterfaceProperty { get; set; }

        public ClassWithoutPublicConstructor ClassWithoutPublicConstructorProperty { get; set; }

        public int IntProperty { get; set; }

        public ThrowsWhenActivated ThrowsWhenActivatedProperty { get; set; }

        public NestedOptions1 NestedOptionsProperty { get; set; }
    }

    public enum KeyEnum
    {
        abc,
        def,
        ghi
    }

    public enum KeyUintEnum : uint
    {
        abc,
        def,
        ghi
    }
}
