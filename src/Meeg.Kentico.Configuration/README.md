# Meeg.Kentico.Configuration

A [configuration builder](https://docs.microsoft.com/en-us/aspnet/config-builder) that provides access to CMS settings alongside your other application configuration settings.

🙋 For more discussion of why you may want to use this project please see the section on [Why should I use the CMS settings configuration builder](#why-should-i-use-the-cms-settings-configuration-builder).

## Getting started

You will need to add the following NuGet packages:

* `Meeg.Kentico.Configuration.Cms.Admin` must be installed into your CMS Administration project (typically named `CMSApp`)
  * This package contains the [default query](#procname) used to fetch CMS settings to be used in your app configuration
* `Meeg.Kentico.Configuration` must be installed into any project outside of your main CMS project (e.g. your MVC app) where you intend to use the configuration builder

> Please note that:
>
> * These packages require a minimum Kentico version of **12.0.39**
> * You will need to target a minimum of .NET Framework **4.7.2** to use these packages - Kentico targets 4.6.1 by default and you will need to manually [change the target framework version](https://docs.kentico.com/k12/installation/installation-questions-and-answers#InstallationQuestionsandAnswers-CanIchangetheTargetframeworkversionofinstalledprojects?) of your CMS and MVC projects before installing the `Meeg.Kentico.Configuration` NuGet packages

## Usage

To use the configuration builder you must:

* Add the builder to `web.config`
* Fetch CMS settings from configuration

### Add the builder to `web.config`

[Configuration builders](https://github.com/aspnet/MicrosoftConfigurationBuilders) must be defined in `web.config`. The CMS settings configuration builder can be defined like this:

```xml
<configuration>
  <configBuilders>
    <builders>
      <add name="CmsSettings" mode="Greedy" type="Meeg.Kentico.Configuration.Cms.ConfigurationBuilders.CmsSettingsConfigBuilder, Meeg.Kentico.Configuration.Cms" />
    </builders>
  </configBuilders>
</configuration>
```

🙋 The above adds the configuration builder with default/recommended options - please see the [options](#options) section for a description of all available options.

### Fetch CMS settings from configuration

CMS settings are effectively exposed as `AppSettings` through the configuration builder and can be accessed via `ConfigurationManager` using key names formatted following these patterns:

* `KeyName` for global settings
* `SiteName:KeyName` for site settings

The [Meeg.Configuration](https://github.com/CMeeg/meeg-configuration) library and its [multi-tenant extensions](https://github.com/CMeeg/meeg-configuration-extensions) make fetching CMS settings from configuration easier and more natural to those used to working with settings in the CMS, and are included as dependencies in the `Meeg.Kentico.Configuration` NuGet packages.

Here are some examples for getting CMS settings values from the configuration builder using the `Meeg.Configuration` library:

```c#
using Meeg.Configuration;
using Meeg.Configuration.Extensions.MultiTenant;

// We need an IAppConfigurationRoot instance to work with
// See the `Meeg.Configuration` docs for more info
var configManager = new ConfigurationManagerAdapter();
var config = new AppConfiguration(configManager);

// You will need a site name if you want to get site settings
const string siteName = "DancingGoat";

// This will return the site setting value for this key if there is one; or the global setting value if not
string faviconPath = config.GetValue("CmsFaviconPath", siteName);

// There is support for type conversion, and specifying default values
bool fooFeatureEnabled = GetValue<bool>("FooFeatureEnabled", siteName, false);

// If you have a group of related settings you can create a POCO to fetch multiple settings at once and bind them to an instance of that class
class ProductListOptions
{
  public int CMSStoreProductsAreNewFor { get; set; }
  public string CMSStoreNewProductStatus { get; set; }
  public string CMSDefaultProductImageUrl { get; set; }
}

// Each individual property will be fetched as a site setting or will fallback to the global setting if the site setting is not available
var productListOptions = config.Get<ProductListOptions>(siteName);
```

## Options

Configuration builders have several built-in [options](https://github.com/aspnet/MicrosoftConfigurationBuilders#keyvalue-config-builders) and the CMS settings configuration builder has a couple of distinct options itself.

To stick with the [conventions](https://github.com/aspnet/MicrosoftConfigurationBuilders#config-builders-in-this-project) used in the official configuration builder docs the options for the CMS settings configuration builder are defined below and the options that are distinct to the CMS settings config builder are then described in more detail.

```xml
<add name="CmsSettings"
  [mode|@prefix|@stripPrefix|tokenPattern|@escapeExpandedValues|@optional=true]
  (@procName="Proc_Meeg_Configuration_AllConfigCmsSettings" | @useCategorySections="false")
  type="Meeg.Kentico.Configuration.Cms.ConfigurationBuilders.CmsSettingsConfigBuilder, Meeg.Kentico.Configuration.Cms" />
```

### procName

This option defines the name of the stored procedure to be used when fetching CMS settings to be exposed via configuration.

The default stored procedure `Proc_Meeg_Configuration_AllConfigCmsSettings` will be added to the CMS database (defined by the `CMSConnectionString` connection string) when you install the `Meeg.Kentico.Configuration.Cms.Admin` package into your CMS admin app (`CMSApp`).

All CMS settings will be returned by default though this can be filtered by using the `prefix` attribute, which will cause only settings that have a `KeyName` that begin with the `prefix` to be returned.

If you want to further customise what settings are returned you can:

* Copy the query from the default stored procand make your customisations
  * Please ensure that you keep the fields in the select clause the same (name and order)
  * Only `KeyName` is required - the other fields can default to `NULL` if there is no suitable value
* Create a new stored proc that executes your modified query
* Add or change the `procName` attribute on the config builder declaration to the name of your custom stored proc

Customisation may be of benefit if you:

* Want to limit the settings returned by the query, but the `prefix` option does not give you enough control
* Want to draw from sources other than `CMS_SettingsKey`
  * For example, you could expose values from `CMS_Site` as configuration settings

### useCategorySections

This is a boolean option that controls if the `CategoryName` (returned from the stored procedure defined in the `procName` option) should be included as a section name in the config key.

For example, if `SiteName` is "DancingGoat"; `CategoryName` is "CMS.Content.Favicon"; and `KeyName` is "CMSFaviconPath":

* `CMS.Content.Favicon:CMSFaviconPath` will be the config key for the global setting
* `DancingGoat:CMS.Content.Favicon:CMSFaviconPath` will be the config key for the site setting (if there is one)

This option is included for scenarios where you would like to use "sections" to delineate and manage your configuration keys.

## Why should I use the CMS settings configuration builder?

Kentico's `SettingsKeyInfoProvider` is the usual entrypoint for accessing CMS settings and works well. Configuration builders are a more modern and flexible way of dealing with configuration in .NET Framework projects. The two are not mutually exclusive and the CMS settings configuration builder is not intended as a replacement for `SettingsKeyInfoProvider`, but is intended to complement it and give you more options when dealing with configuration in your Kentico applications.

This section tries to explain a bit more about how the two can be used together and when you might want to consider using one over the other. This is not prescriptive though - just some suggestions!

### Why use the configuration builder

* CMS settings that should not or do not change at runtime are good candidates for configuration settings
* Using the configuration builder moves these other settings alongside all of your other application configuration settings
* Using configuration builders on your project opens up other options for configuration as well as CMS settings, such as [environment variables](https://github.com/aspnet/MicrosoftConfigurationBuilders#environmentconfigbuilder), [json files](https://github.com/aspnet/MicrosoftConfigurationBuilders#simplejsonconfigbuilder), [Azure app configuration](https://github.com/aspnet/MicrosoftConfigurationBuilders#azureappconfigurationbuilder), [Azure key vault](https://github.com/aspnet/MicrosoftConfigurationBuilders#azurekeyvaultconfigbuilder), and more (including the ability to write your own custom builders such as this one!)
* The `Meeg.Configuration` libraries provide (admittedly, in my own biased opinion) a better developer experience than using `SettingsKeyInfoProvider`
  * The API is based on .NET Core configuration and provide lots of convenience methods
  * The multi-tenant extensions provide a similar experience to working with `SettingsKeyInfoProvider` insofar as they provide access to settings per tenant ("site" in Kentico terms) with fallback to "global" settings
* You can [customise the source](#procname) of the settings to include other data from the Kentico database that may be useful in configuration scenarios i.e. not just values from `CMS_SettingsKey`

### Why use `SettingsKeyInfoProvider`

* CMS settings that can or will change at runtime are not good candidates for configuration settings
* Configuration builders are implemented using the provider model and settings are exposed via `AppSettings`, which effectively has a singleton lifetime and there is currently no option to reload on change - changes will only be reloaded if the application restarts - settings accessed via `SettingsKeyInfoProvider` are cached and will bust cache on change so it does not have this problem
* If you cannot upgrade your project to .NET Framework 4.7.2 you will not be able to use `Meeg.Kentico.Configuration`

## Changelog

All notable changes to this project will be documented in the [changelog](CHANGELOG.md).

## Contributing

Please see the [main README](../../README.md) for more information about how to get started with the sample site, and how to contribute, ask questions and raise bugs.

## License

Licensed under the [MIT License](../../LICENSE).
