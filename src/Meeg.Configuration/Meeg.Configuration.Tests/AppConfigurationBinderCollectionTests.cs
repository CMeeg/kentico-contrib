using System.Collections.Generic;
using NUnit.Framework;

namespace Meeg.Configuration.Tests
{
    [TestFixture]
    public class AppConfigurationBinderCollectionTests
    {
        [Test]
        public void GetList()
        {
            var input = new Dictionary<string, string>
            {
                {"StringList:0", "val0"},
                {"StringList:1", "val1"},
                {"StringList:2", "val2"},
                {"StringList:x", "valx"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(input)
                .Build();

            var config = new AppConfiguration(configManager);

            var list = new List<string>();
            config.GetSection("StringList").Bind(list);

            Assert.Multiple(() =>
            {
                Assert.That(4, Is.EqualTo(list.Count));

                Assert.That("val0", Is.EqualTo(list[0]));
                Assert.That("val1", Is.EqualTo(list[1]));
                Assert.That("val2", Is.EqualTo(list[2]));
                Assert.That("valx", Is.EqualTo(list[3]));
            });
        }

        [Test]
        public void GetListNullValues()
        {
            var input = new Dictionary<string, string>
            {
                {"StringList:0", null},
                {"StringList:1", null},
                {"StringList:2", null},
                {"StringList:x", null}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(input)
                .Build();

            var config = new AppConfiguration(configManager);

            var list = new List<string>();
            config.GetSection("StringList").Bind(list);

            CollectionAssert.IsEmpty(list);
        }

        [Test]
        public void GetListInvalidValues()
        {
            var input = new Dictionary<string, string>
            {
                {"InvalidList:0", "true"},
                {"InvalidList:1", "invalid"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(input)
                .Build();

            var config = new AppConfiguration(configManager);

            var list = new List<bool>();
            config.GetSection("InvalidList").Bind(list);

            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(list.Count));
                Assert.That(list[0], Is.True);
            });
        }

        [Test]
        public void BindList()
        {
            var input = new Dictionary<string, string>
            {
                {"StringList:0", "val0"},
                {"StringList:1", "val1"},
                {"StringList:2", "val2"},
                {"StringList:x", "valx"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(input)
                .Build();

            var config = new AppConfiguration(configManager);

            var list = new List<string>();
            config.GetSection("StringList").Bind(list);

            Assert.Multiple(() =>
            {
                Assert.That(4, Is.EqualTo(list.Count));

                Assert.That("val0", Is.EqualTo(list[0]));
                Assert.That("val1", Is.EqualTo(list[1]));
                Assert.That("val2", Is.EqualTo(list[2]));
                Assert.That("valx", Is.EqualTo(list[3]));
            });
        }

        [Test]
        public void GetObjectList()
        {
            var input = new Dictionary<string, string>
            {
                {"ObjectList:0:Integer", "30"},
                {"ObjectList:1:Integer", "31"},
                {"ObjectList:2:Integer", "32"},
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(input)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = new List<NestedOptions>();
            config.GetSection("ObjectList").Bind(options);

            Assert.Multiple(() =>
            {
                Assert.That(3, Is.EqualTo(options.Count));

                Assert.That(30, Is.EqualTo(options[0].Integer));
                Assert.That(31, Is.EqualTo(options[1].Integer));
                Assert.That(32, Is.EqualTo(options[2].Integer));
            });
        }

        [Test]
        public void GetStringDictionary()
        {
            var input = new Dictionary<string, string>
            {
                {"StringDictionary:abc", "val_1"},
                {"StringDictionary:def", "val_2"},
                {"StringDictionary:ghi", "val_3"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(input)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = new Dictionary<string, string>();
            config.GetSection("StringDictionary").Bind(options);

            Assert.Multiple(() =>
            {
                Assert.That(3, Is.EqualTo(options.Count));

                Assert.That("val_1", Is.EqualTo(options["abc"]));
                Assert.That("val_2", Is.EqualTo(options["def"]));
                Assert.That("val_3", Is.EqualTo(options["ghi"]));
            });
        }

        [Test]
        public void GetEnumDictionary()
        {
            var input = new Dictionary<string, string>
            {
                {"EnumDictionary:abc", "val_1"},
                {"EnumDictionary:def", "val_2"},
                {"EnumDictionary:ghi", "val_3"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(input)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = new Dictionary<KeyEnum, string>();
            config.GetSection("EnumDictionary").Bind(options);

            Assert.Multiple(() =>
            {
                Assert.That(3, Is.EqualTo(options.Count));

                Assert.That("val_1", Is.EqualTo(options[KeyEnum.abc]));
                Assert.That("val_2", Is.EqualTo(options[KeyEnum.def]));
                Assert.That("val_3", Is.EqualTo(options[KeyEnum.ghi]));
            });
        }

        [Test]
        public void GetUintEnumDictionary()
        {
            var input = new Dictionary<string, string>
            {
                {"EnumDictionary:abc", "val_1"},
                {"EnumDictionary:def", "val_2"},
                {"EnumDictionary:ghi", "val_3"}
            };

            var configManager = new ConfigurationManagerFixture()
                .WithAppSettings(input)
                .Build();

            var config = new AppConfiguration(configManager);

            var options = new Dictionary<KeyUintEnum, string>();
            config.GetSection("EnumDictionary").Bind(options);

            Assert.Multiple(() =>
            {
                Assert.That(3, Is.EqualTo(options.Count));
                Assert.That("val_1", Is.EqualTo(options[KeyUintEnum.abc]));
                Assert.That("val_2", Is.EqualTo(options[KeyUintEnum.def]));
                Assert.That("val_3", Is.EqualTo(options[KeyUintEnum.ghi]));
            });
        }
    }
}
