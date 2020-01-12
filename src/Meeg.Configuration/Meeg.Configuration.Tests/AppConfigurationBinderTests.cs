using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Meeg.Configuration.Tests
{
    [TestFixture]
    public class AppConfigurationBinderTests
    {
        [Test]
        public void CanBindIConfigurationSection()
        {
            var dic = new Dictionary<string, string>
            {
                {"Section:Integer", "-2"},
                {"Section:Boolean", "TRUe"},
                {"Section:Nested:Integer", "11"},
                {"Section:Virtual", "Sup"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = config.Get<ConfigurationInterfaceOptions>();

            var childOptions = options.Section.Get<DerivedOptions>();

            Assert.Multiple(() =>
            {
                Assert.That(childOptions.Boolean, Is.True);
                Assert.That(-2, Is.EqualTo(childOptions.Integer));
                Assert.That(11, Is.EqualTo(childOptions.Nested.Integer));
                Assert.That("Derived:Sup", Is.EqualTo(childOptions.Virtual));

                Assert.That("Section", Is.EqualTo(options.Section.Key));
                Assert.That("Section", Is.EqualTo(options.Section.Path));
                Assert.That(options.Section.Value, Is.Null);
            });
        }

        [Test]
        public void CanBindWithKeyOverload()
        {
            var dic = new Dictionary<string, string>
            {
                {"Section:Integer", "-2"},
                {"Section:Boolean", "TRUe"},
                {"Section:Nested:Integer", "11"},
                {"Section:Virtual", "Sup"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = new DerivedOptions();
            config.Bind("Section", options);

            Assert.Multiple(() =>
            {
                Assert.That(options.Boolean, Is.True);
                Assert.That(-2, Is.EqualTo(options.Integer));
                Assert.That(11, Is.EqualTo(options.Nested.Integer));
                Assert.That("Derived:Sup", Is.EqualTo(options.Virtual));
            });
        }

        [Test]
        public void CanBindIConfigurationSectionWithDerivedOptionsSection()
        {
            var dic = new Dictionary<string, string>
            {
                {"Section:Integer", "-2"},
                {"Section:Boolean", "TRUe"},
                {"Section:Nested:Integer", "11"},
                {"Section:Virtual", "Sup"},
                {"Section:DerivedSection:Nested:Integer", "11"},
                {"Section:DerivedSection:Virtual", "Sup"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = config.Get<ConfigurationInterfaceOptions>();

            var childOptions = options.Section.Get<DerivedOptionsWithSection>();

            var childDerivedOptions = childOptions.DerivedSection.Get<DerivedOptions>();

            Assert.Multiple(() =>
            {
                Assert.That(childOptions.Boolean, Is.True);
                Assert.That(-2, Is.EqualTo(childOptions.Integer));
                Assert.That(11, Is.EqualTo(childOptions.Nested.Integer));
                Assert.That("Derived:Sup", Is.EqualTo(childOptions.Virtual));
                Assert.That(11, Is.EqualTo(childDerivedOptions.Nested.Integer));
                Assert.That("Derived:Sup", Is.EqualTo(childDerivedOptions.Virtual));

                Assert.That("Section", Is.EqualTo(options.Section.Key));
                Assert.That("Section", Is.EqualTo(options.Section.Path));
                Assert.That("DerivedSection", Is.EqualTo(childOptions.DerivedSection.Key));
                Assert.That("Section:DerivedSection", Is.EqualTo(childOptions.DerivedSection.Path));
                Assert.That(options.Section.Value, Is.Null);
            });
        }

        [Test]
        public void EmptyStringIsNullable()
        {
            var dic = new Dictionary<string, string>
            {
                {"empty", ""},
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            Assert.Multiple(() =>
            {
                Assert.That(config.GetValue<bool?>("empty"), Is.Null);
                Assert.That(config.GetValue<int?>("empty"), Is.Null);
            });
        }

        [Test]
        public void GetScalarNullable()
        {
            var dic = new Dictionary<string, string>
            {
                {"Integer", "-2"},
                {"Boolean", "TRUe"},
                {"Nested:Integer", "11"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            Assert.Multiple(() =>
            {
                Assert.That(config.GetValue<bool?>("Boolean"), Is.True);
                Assert.That(-2, Is.EqualTo(config.GetValue<int?>("Integer")));
                Assert.That(11, Is.EqualTo(config.GetValue<int?>("Nested:Integer")));
            });
        }

        [Test]
        public void CanBindToObjectProperty()
        {
            var dic = new Dictionary<string, string>
            {
                {"Object", "whatever" }
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = new ComplexOptions();
            config.Bind(options);

            Assert.That("whatever", Is.EqualTo(options.Object));
        }

        [Test]
        public void GetNullValue()
        {
            var dic = new Dictionary<string, string>
            {
                {"Integer", null},
                {"Boolean", null},
                {"Nested:Integer", null},
                {"Object", null }
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            Assert.Multiple(() =>
            {
                Assert.That(config.GetValue<bool>("Boolean"), Is.False);
                Assert.That(0, Is.EqualTo(config.GetValue<int>("Integer")));
                Assert.That(0, Is.EqualTo(config.GetValue<int>("Nested:Integer")));
                Assert.That(config.GetValue<ComplexOptions>("Object"), Is.Null);
                Assert.That(config.GetSection("Boolean").Get<bool>(), Is.False);
                Assert.That(0, Is.EqualTo(config.GetSection("Integer").Get<int>()));
                Assert.That(0, Is.EqualTo(config.GetSection("Nested:Integer").Get<int>()));
                Assert.That(config.GetSection("Object").Get<ComplexOptions>(), Is.Null);
            });
        }

        [Test]
        public void GetDefaultsWhenDataDoesNotExist()
        {
            var dic = new Dictionary<string, string>
            {
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var foo = new ComplexOptions();

            Assert.Multiple(() =>
            {
                Assert.That(config.GetValue<bool>("Boolean"), Is.False);
                Assert.That(0, Is.EqualTo(config.GetValue<int>("Integer")));
                Assert.That(0, Is.EqualTo(config.GetValue<int>("Nested:Integer")));
                Assert.That(config.GetValue<ComplexOptions>("Object"), Is.Null);
                Assert.That(config.GetValue("Boolean", true), Is.True);
                Assert.That(3, Is.EqualTo(config.GetValue("Integer", 3)));
                Assert.That(1, Is.EqualTo(config.GetValue("Nested:Integer", 1)));
                Assert.That(config.GetValue("Object", foo), Is.SameAs(foo));
            });
        }

        [Test]
        public void GetUri()
        {
            var dic = new Dictionary<string, string>
            {
                {"AnUri", "http://www.bing.com"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var uri = config.GetValue<Uri>("AnUri");

            Assert.That("http://www.bing.com", Is.EqualTo(uri.OriginalString));
        }

        [TestCase("2147483647", typeof(int))]
        [TestCase("4294967295", typeof(uint))]
        [TestCase("32767", typeof(short))]
        [TestCase("65535", typeof(ushort))]
        [TestCase("-9223372036854775808", typeof(long))]
        [TestCase("18446744073709551615", typeof(ulong))]
        [TestCase("trUE", typeof(bool))]
        [TestCase("255", typeof(byte))]
        [TestCase("127", typeof(sbyte))]
        [TestCase("\uffff", typeof(char))]
        [TestCase("79228162514264337593543950335", typeof(decimal))]
        [TestCase("1.79769e+308", typeof(double))]
        [TestCase("3.40282347E+38", typeof(float))]
        [TestCase("2015-12-24T07:34:42-5:00", typeof(DateTime))]
        [TestCase("12/24/2015 13:44:55 +4", typeof(DateTimeOffset))]
        [TestCase("99.22:22:22.1234567", typeof(TimeSpan))]
        [TestCase("http://www.bing.com", typeof(Uri))]
        [TestCase("Constructor", typeof(AttributeTargets))]
        [TestCase("CA761232-ED42-11CE-BACD-00AA0057B223", typeof(Guid))]
        public void CanReadAllSupportedTypes(string value, Type type)
        {
            var dic = new Dictionary<string, string>
            {
                {"Value", value}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var optionsType = typeof(GenericOptions<>).MakeGenericType(type);
            var options = Activator.CreateInstance(optionsType);
            var expectedValue = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);

            config.Bind(options);
            var optionsValue = options.GetType().GetProperty("Value").GetValue(options);
            var getValueValue = config.GetValue(type, "Value");
            var getValue = config.GetSection("Value").Get(type);

            Assert.Multiple(() =>
            {
                Assert.That(expectedValue, Is.EqualTo(optionsValue));
                Assert.That(expectedValue, Is.EqualTo(getValue));
                Assert.That(expectedValue, Is.EqualTo(getValueValue));
            });
        }

        [TestCase(typeof(int))]
        [TestCase(typeof(uint))]
        [TestCase(typeof(short))]
        [TestCase(typeof(ushort))]
        [TestCase(typeof(long))]
        [TestCase(typeof(ulong))]
        [TestCase(typeof(bool))]
        [TestCase(typeof(byte))]
        [TestCase(typeof(sbyte))]
        [TestCase(typeof(char))]
        [TestCase(typeof(decimal))]
        [TestCase(typeof(double))]
        [TestCase(typeof(float))]
        [TestCase(typeof(DateTime))]
        [TestCase(typeof(DateTimeOffset))]
        [TestCase(typeof(TimeSpan))]
        [TestCase(typeof(AttributeTargets))]
        [TestCase(typeof(Guid))]
        public void ConsistentExceptionOnFailedBinding(Type type)
        {
            const string IncorrectValue = "Invalid data";
            const string ConfigKey = "Value";
            var dic = new Dictionary<string, string>
            {
                {ConfigKey, IncorrectValue}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var optionsType = typeof(GenericOptions<>).MakeGenericType(type);
            var options = Activator.CreateInstance(optionsType);

            var exception = Assert.Throws<InvalidOperationException>(
                () => config.Bind(options));

            var getValueException = Assert.Throws<InvalidOperationException>(
                () => config.GetValue(type, "Value"));

            var getException = Assert.Throws<InvalidOperationException>(
                () => config.GetSection("Value").Get(type));

            Assert.Multiple(() =>
            {
                Assert.That(exception.InnerException, Is.Not.Null);
                Assert.That(getException.InnerException, Is.Not.Null);
            });
        }

        [Test]
        public void ExceptionOnFailedBindingIncludesPath()
        {
            const string IncorrectValue = "Invalid data";
            const string ConfigKey = "Nested:Value";

            var dic = new Dictionary<string, string>
            {
                {ConfigKey, IncorrectValue}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = new OptionsWithNesting();

            Assert.Multiple(() =>
            {
                var exception = Assert.Throws<InvalidOperationException>(
                    () => config.Bind(options));

                Assert.That(exception.Message, Does.Contain(ConfigKey));
            });
        }

        [Test]
        public void BinderIgnoresIndexerProperties()
        {
            var configManager = new ConfigurationManagerFixture()
                .Build();

            var config = new AppConfiguration(configManager);

            config.Bind(new List<string>());
        }

        [Test]
        public void BindCanReadComplexProperties()
        {
            var dic = new Dictionary<string, string>
            {
                {"Integer", "-2"},
                {"Boolean", "TRUe"},
                {"Nested:Integer", "11"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var instance = new ComplexOptions();
            config.Bind(instance);

            Assert.Multiple(() =>
            {
                Assert.That(instance.Boolean, Is.True);
                Assert.That(-2, Is.EqualTo(instance.Integer));
                Assert.That(11, Is.EqualTo(instance.Nested.Integer));
            });
        }

        [Test]
        public void GetCanReadComplexProperties()
        {
            var dic = new Dictionary<string, string>
            {
                {"Integer", "-2"},
                {"Boolean", "TRUe"},
                {"Nested:Integer", "11"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = new ComplexOptions();
            config.Bind(options);

            Assert.Multiple(() =>
            {
                Assert.That(options.Boolean, Is.True);
                Assert.That(-2, Is.EqualTo(options.Integer));
                Assert.That(11, Is.EqualTo(options.Nested.Integer));
            });
        }

        [Test]
        public void BindCanReadInheritedProperties()
        {
            var dic = new Dictionary<string, string>
            {
                {"Integer", "-2"},
                {"Boolean", "TRUe"},
                {"Nested:Integer", "11"},
                {"Virtual", "Sup"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var instance = new DerivedOptions();
            config.Bind(instance);

            Assert.Multiple(() =>
            {
                Assert.That(instance.Boolean, Is.True);
                Assert.That(-2, Is.EqualTo(instance.Integer));
                Assert.That(11, Is.EqualTo(instance.Nested.Integer));
                Assert.That("Derived:Sup", Is.EqualTo(instance.Virtual));
            });
        }

        [Test]
        public void GetCanReadInheritedProperties()
        {
            var dic = new Dictionary<string, string>
            {
                {"Integer", "-2"},
                {"Boolean", "TRUe"},
                {"Nested:Integer", "11"},
                {"Virtual", "Sup"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = new DerivedOptions();
            config.Bind(options);

            Assert.Multiple(() =>
            {
                Assert.That(options.Boolean, Is.True);
                Assert.That(-2, Is.EqualTo(options.Integer));
                Assert.That(11, Is.EqualTo(options.Nested.Integer));
                Assert.That("Derived:Sup", Is.EqualTo(options.Virtual));
            });
        }

        [Test]
        public void GetCanReadStaticProperty()
        {
            var dic = new Dictionary<string, string>
            {
                {"StaticProperty", "stuff"},
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = new ComplexOptions();
            config.Bind(options);

            Assert.That("stuff", Is.EqualTo(ComplexOptions.StaticProperty));
        }

        [Test]
        public void BindCanReadStaticProperty()
        {
            var dic = new Dictionary<string, string>
            {
                {"StaticProperty", "other stuff"},
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var instance = new ComplexOptions();
            config.Bind(instance);

            Assert.That("other stuff", Is.EqualTo(ComplexOptions.StaticProperty));
        }

        [Test]
        public void CanGetComplexOptionsWhichHasAlsoHasValue()
        {
            var dic = new Dictionary<string, string>
            {
                {"obj", "whut" },
                {"obj:Integer", "-2"},
                {"obj:Boolean", "TRUe"},
                {"obj:Nested:Integer", "11"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = config.GetSection("obj").Get<ComplexOptions>();

            Assert.Multiple(() =>
            {
                Assert.That(options, Is.Not.Null);
                Assert.That(options.Boolean, Is.True);
                Assert.That(-2, Is.EqualTo(options.Integer));
                Assert.That(11, Is.EqualTo(options.Nested.Integer));
            });
        }

        [TestCase("ReadOnly")]
        [TestCase("PrivateSetter")]
        [TestCase("ProtectedSetter")]
        [TestCase("InternalSetter")]
        [TestCase("InternalProperty")]
        [TestCase("PrivateProperty")]
        [TestCase("ProtectedProperty")]
        [TestCase("ProtectedPrivateSet")]
        public void GetIgnoresTests(string property)
        {
            var dic = new Dictionary<string, string>
            {
                {property, "stuff"},
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = config.Get<ComplexOptions>();

            Assert.That(options.GetType().GetTypeInfo().GetDeclaredProperty(property).GetValue(options), Is.Null);
        }

        [TestCase("PrivateSetter")]
        [TestCase("ProtectedSetter")]
        [TestCase("InternalSetter")]
        [TestCase("InternalProperty")]
        [TestCase("PrivateProperty")]
        [TestCase("ProtectedProperty")]
        [TestCase("ProtectedPrivateSet")]
        public void GetCanSetNonPublicWhenSet(string property)
        {
            var dic = new Dictionary<string, string>
            {
                {property, "stuff"},
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = config.Get<ComplexOptions>(o => o.BindNonPublicProperties = true);

            Assert.That("stuff", Is.EqualTo(options.GetType().GetTypeInfo().GetDeclaredProperty(property).GetValue(options)));
        }

        [TestCase("InternalReadOnly")]
        [TestCase("PrivateReadOnly")]
        [TestCase("ProtectedReadOnly")]
        public void NonPublicModeGetStillIgnoresReadonly(string property)
        {
            var dic = new Dictionary<string, string>
            {
                {property, "stuff"},
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = config.Get<ComplexOptions>(o => o.BindNonPublicProperties = true);

            Assert.That(options.GetType().GetTypeInfo().GetDeclaredProperty(property).GetValue(options), Is.Null);
        }

        [TestCase("ReadOnly")]
        [TestCase("PrivateSetter")]
        [TestCase("ProtectedSetter")]
        [TestCase("InternalSetter")]
        [TestCase("InternalProperty")]
        [TestCase("PrivateProperty")]
        [TestCase("ProtectedProperty")]
        [TestCase("ProtectedPrivateSet")]
        public void BindIgnoresTests(string property)
        {
            var dic = new Dictionary<string, string>
            {
                {property, "stuff"},
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = new ComplexOptions();
            config.Bind(options);

            Assert.That(options.GetType().GetTypeInfo().GetDeclaredProperty(property).GetValue(options), Is.Null);
        }

        [TestCase("PrivateSetter")]
        [TestCase("ProtectedSetter")]
        [TestCase("InternalSetter")]
        [TestCase("InternalProperty")]
        [TestCase("PrivateProperty")]
        [TestCase("ProtectedProperty")]
        [TestCase("ProtectedPrivateSet")]
        public void BindCanSetNonPublicWhenSet(string property)
        {
            var dic = new Dictionary<string, string>
            {
                {property, "stuff"},
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = new ComplexOptions();
            config.Bind(options, o => o.BindNonPublicProperties = true);

            Assert.That("stuff", Is.EqualTo(options.GetType().GetTypeInfo().GetDeclaredProperty(property).GetValue(options)));
        }

        [TestCase("InternalReadOnly")]
        [TestCase("PrivateReadOnly")]
        [TestCase("ProtectedReadOnly")]
        public void NonPublicModeBindStillIgnoresReadonly(string property)
        {
            var dic = new Dictionary<string, string>
            {
                {property, "stuff"},
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = new ComplexOptions();
            config.Bind(options, o => o.BindNonPublicProperties = true);

            Assert.That(options.GetType().GetTypeInfo().GetDeclaredProperty(property).GetValue(options), Is.Null);
        }

        [Test]
        public void ExceptionWhenTryingToBindToInterface()
        {
            var dic = new Dictionary<string, string>
            {
                {"ISomeInterfaceProperty:Subkey", "x"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            Assert.That(() => config.Bind(new TestOptions()), Throws.InvalidOperationException);
        }

        [Test]
        public void ExceptionWhenTryingToBindClassWithoutParameterlessConstructor()
        {
            var dic = new Dictionary<string, string>
            {
                {"ClassWithoutPublicConstructorProperty:Subkey", "x"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            Assert.That(() => config.Bind(new TestOptions()), Throws.InvalidOperationException);
        }

        [Test]
        public void ExceptionWhenTryingToBindToTypeThrowsWhenActivated()
        {
            var dic = new Dictionary<string, string>
            {
                {"ThrowsWhenActivatedProperty:subkey", "x"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            Assert.Multiple(() =>
            {
                var exception = Assert.Throws<InvalidOperationException>(() => config.Bind(new TestOptions()));
                Assert.That(exception.InnerException, Is.Not.Null);
            });
        }

        [Test]
        public void ExceptionIncludesKeyOfFailedBinding()
        {
            var dic = new Dictionary<string, string>
            {
                {"NestedOptionsProperty:NestedOptions2Property:ISomeInterfaceProperty:subkey", "x"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(dic)
                .Build();

            var config = new AppConfiguration(configManager);

            Assert.That(() => config.Bind(new TestOptions()), Throws.InvalidOperationException);
        }
    }
}
