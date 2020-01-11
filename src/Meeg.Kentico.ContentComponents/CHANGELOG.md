# Changelog

All notable changes to this project will be documented in this file.

## 0.2.0 (2020-01-11)

### Features

* Content components now targets .NET 4.6.1 instead of 4.7.2 ([436fcbb](https://github.com/CMeeg/kentico-contrib/commit/436fcbb))
  * 4.6.1 is the default for new Kentico 12 installations and the projects don't use any particular features of 4.7.2 so this should make it easier for consumers to pick up and use these libs from NuGet i.e. not requiring that you change your target framework from the default.

### Bug fixes

* Prevent optional fields being serialised to component xml by the form control if they have no value set ([c5e053f](https://github.com/CMeeg/kentico-contrib/commit/c5e053f))
  * Optional fields should not be serialised as it can cause an exception to be thrown by the deserialiser if it attempts to convert the value to a type other than string.
  * If you are affected by this issue, please re-save (and publish if required) any affected pages - this will cause components to be re-serialised, removing any empty fields from the component XML in the process.
