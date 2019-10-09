# Meeg.Kentico.ContentComponents

Content Components provide a way of constructing your Page Types in Kentico using composition rather than inheritance.

> If you are familiar with [Kentico Kontent](https://kontent.ai) they are analogous to [content type snippets](https://docs.kontent.ai/tutorials/set-up-projects/define-content-models/reusing-groups-of-elements-with-snippets) though the same concept exists in other content management platforms.

🙋 For further discussion of where Content Components are intended to sit in with other content modelling options in Kentico, please see [composing content](#composing-content).

## Getting started

There are two NuGet packages you will need to install:

* `Meeg.Kentico.ContentComponents.Cms.Admin` must be installed into your CMS Administration project (typically named `CMSApp`)
  * This package contains the `Page type component` Form Control that is used to [add and edit Content Components](#usage) within your existing Page Types
* `Meeg.Kentico.ContentComponents` must be installed into any project outside of your main CMS project where you intend to [use the data stored in Content Component fields](#fetch-and-use-component-content)

> Please note that:
>
> * These packages require a minimum Kentico version of **12.0.39**
> * Content Components has only been tested in an MVC project, but there is no known reason why they couldn't be used on a project that uses the Portal Engine approach

## Usage

To use Content Components you must:

* Define the content model (Page Type) for your component(s)
* Add the component(s) as fields on other Page Types
* Fetch and use the component(s) content

### Define the component content model

Create the content model for your component by [creating a Page Type](https://docs.kentico.com/k12sp/developing-websites/defining-website-content-structure/creating-and-configuring-page-types) in the CMS:

* Use a content-only page type
* Add categories and fields as you normally would including required fields and validation rules etc
  * A component Page Type can itself have Content Component fields if you want to compose components using other components
* Unless you want to use it for [creating content items](#composing-content) also, remove all allowed parent types
* Assign it to the appropriate site(s)
* Save and create your Page Type

### Add Content Components to Page Types

Create a new Page Type or edit an existing Page Type that will use your component:

* Add a new field for your component
* Set the data type to `Long text`
* (Optional) Instead of setting a field caption you may want to consider placing the component under a category as this puts the component fields in its own "section" when editing the content
* Change the form control to `Page type component`
* Under "Editing control settings" set the `Page type` to the Page Type that represents your component content model
* (Optional) Enter the code name of an [Alternative form](https://docs.kentico.com/k12sp/developing-websites/defining-website-content-structure/creating-and-configuring-page-types/configuring-page-types/creating-alternative-forms-for-page-types) that will be used to present your component on the editing form (Form or Content tab)
* Add any additional fields required for your content model
* Save the field

Create or edit a Page that includes a Content Component:

* Edit the fields of the component on the editing form alongside the other fields that belong to the Page Type
* Save your page

### Fetch and use component content

To fetch Content Component data:

* Use a [DocumentQuery](https://docs.kentico.com/k12sp/custom-development/working-with-pages-in-the-api) to fetch the page(s) that include content components
  * Ensure that you [include the column](https://docs.kentico.com/k12sp/custom-development/working-with-pages-in-the-api#WorkingwithpagesintheAPI-DocumentQueryreference) that the Content Component data is stored in

To use content component data:

* Add a `using` statement for `Meeg.Kentico.ContentComponents.Cms`
* Use one of the `GetPageTypeComponent` extension methods that exist in the above namespace on the `TreeNode` instances returned by your `DocumentQuery`

There are three extension methods available:

```csharp
using Meeg.Kentico.ContentComponents.Cms;

// 1. Use this if you haven't generated a class for your component Page Type

TreeNode component = node.GetPageTypeComponent("Custom.ComponentPageType", "ComponentFieldColumnName");

// 2. Use this if you have generated a class for your component Page Type (`MyComponent` in the below example) and you don't need to use the function approach used in the next example

MyComponent component = node.GetPageTypeComponent<MyComponent>("ComponentFieldColumnName");

// 3. Use this if you have generated classes for your component ('MyComponent`) and page (`MyPage`) Page Types and you want to provide your own function delegate for accessing component data of the page

MyComponent component => page.GetPageTypeComponent<MyPage, MyComponent>(myPage => myPage.ComponentField);
```

#### Recommendations

The recommended approach to fetching page and component data is to:

* [Generate classes](https://docs.kentico.com/k12sp/developing-websites/generating-classes-for-kentico-objects#GeneratingclassesforKenticoobjects-Workingwithpagetypecodegenerators) for all your Page Types
* Add a new partial class to [extend the generated classes](https://docs.kentico.com/k12sp/developing-websites/generating-classes-for-kentico-objects#GeneratingclassesforKenticoobjects-Customizinggeneratedclasses) for Page Types that have Content Component fields
* Add a new property to your partial class for each Content Component field that will return the component as an instance of a generated Page Type class
* Use a strongly typed `DocumentQuery` to fetch data of pages that include content components
* Access components using the properties that you added to your partial classes

Example:

```csharp
/*
In this example:

* There is a Page Type called `Home` and a Page Type called `PageMetadata`
* `Home` has a Content Component field named `HomeMetadata` that uses the `PageMetadata` Page Type
* Classes have been generated for both Page Types

*/

// 1. In a new file, create a partial class to extend the `Home` Page Type

using Meeg.Kentico.ContentComponents.Cms;

namespace CMS.DocumentEngine.Types.Custom
{
    public partial class Home
    {
        public PageMetadata Metadata => this.GetPageTypeComponent<PageMetadata>(nameof(HomeMetadata));
    }
}

// 2. Wherever you want to fetch and use the component, use a strongly typed document query to fetch the home page, and then use the property from the partial class to access the component data

...

Home page = DocumentHelper.GetDocuments<Home>()
    .Columns(nameof(Home.HomeMetadata)) // Add other columns as necessary
    .ToList()
    .FirstOrDefault();

PageMetadata component = page.Metadata;

...
```

## How it works

There are three main parts to this module:

* The `Page type component` form control
* `PageTypeComponentSerializer`
* `PageTypeComponentDeserializer`

The form control uses a `BasicForm` to load the relevant component Page Type's editing form on to the Content or Form tab in the CMS (using an Alternative Form if one was specified).

When the page is saved the form control first validates the `BasicForm` and then if there are no errors it creates a new `TreeNode` instance using the appropriate Page Type and populates its fields using the values entered into the `BasicForm`.

The component `TreeNode` instance is then serialised to XML by the `PageTypeComponentSerializer` and the XML is returned and stored against the page that is being edited in the corresponding component field. The values of any [Page fields](#page-fields) present on the component Page Type are also stored against the corresponding field(s) on the page.

The component field data can then be deserialised using the `PageTypeComponentDeserializer`. This is used by the extension methods discussed in the [usage section](#fetch-and-use-component-content), and also by the `Page type component` form control to populate the `BasicForm` with any existing data so that it can be edited. [Page fields](#page-fields) are stored with the component data, but can also be accessed via the corresponding field on the page.

### Page fields

If any of the Page Type's fields are Page fields (i.e. you selected the Field type as "Page field" rather than "Standard field"), the values are also used to set the corresponding fields on the page that is being edited.

For example, if you have added the Page field `DocumentPageTitle` to your component, the value entered into this field on the Content or Form tab will be saved with the component data, but also saved to the `DocumentPageTitle` field of the page you are editing.

The value of that field can then be retrieved via the [component](#fetch-and-use-component-content), and by accessing the `DocumentPageTitle` field of the page.

## Composing content

Content Components are intended to complement existing features for content modelling in Kentico such as Page Type inheritance and [Pages fields](https://docs.kentico.com/k12sp/developing-websites/defining-website-content-structure/creating-and-configuring-page-types/configuring-page-types/reusing-existing-page-content).

As with all features there are some scenarios where Content Components will fit and others where they will not. Below is some further information about the intention of Content Components, [known limitations](#known-limitations) and trade-offs that you may wish to consider when choosing if they are suitable to use based on your requirements:

* ✔ Do use components where you need a reusable group of fields that you can place on any Page Type
  * Including Page Types that represent other components
  * Because the component is added as a single field you also will never have to reorder multiple fields on every Page Type that uses the component should you add a new field to the component, which you sometimes have to do when adding a new field to an inherited Page Type
* ✔ Do use components if you have a group of fields that need to be repeated on a Page Type
  * For example, if you had a group of fields that represented a "Call to action" (CTA) (e.g. Title, Description, Image, Link URL, Link text) and you want to have those groups repeated to model multiple CTAs
* ✔ Do use components if the content you are modelling "belongs" to the page the component is placed on and doesn't require its own workflow
  * The component data is stored in a field of the page it has been placed on so does not have its own workflow - the data will be versioned along with the rest of the page's data
* ✔ Do use components where the component will set [Page fields](#page-fields) on the page that the component belongs to
  * For example, if want a component to set the page's `DocumentPageTitle` and `DocumentPageDescription` you would add these as Page fields of your component Page Type
* ✔ Do use components instead of inheritance chains
  * For example, if you have a `BasePage` that has some fields that are common to all Page Types and then a `BasePageWithHero` that inherits from `BasePage`, but also has some fields that are common to a subset of Page Types that need a "Hero" element - you could instead still stick with a `BasePage`, but also have a `Hero` Content Component, or scrap the `BasePage` and have discreet components for common groups of fields
* ❌ Do not use components if you require the component data to have its own versioning and workflow separate to the page
  * Consider using [Pages fields](https://docs.kentico.com/k12sp/developing-websites/defining-website-content-structure/creating-and-configuring-page-types/configuring-page-types/reusing-existing-page-content) instead
* ❌ Do not use components where you need to easily query the component data via SQL or as part of a `DocumentQuery`
  * The data is stored as XML and although it is possible to query it, it's not going to be as straight-forward or performant as using Standard or Inherited fields
  * ⚠ An exception is if you only need to query against [Page fields](#page-fields) as these are also set directly against the page
* ❌ Do not use components where you need to search against component data using [Smart Search](https://docs.kentico.com/k12sp/configuring-kentico/setting-up-search-on-your-website)
  * Because components are based on Page Types it is possible to configure their [search fields](https://docs.kentico.com/k12sp/configuring-kentico/setting-up-search-on-your-website/using-locally-stored-search-indexes/creating-local-search-indexes/defining-local-page-indexes#Defininglocalpageindexes-Configuringsearchsettingsforpagefields), but the indexer will not know how to extract the component data and apply the search configuration because the data is stored as XML - see [known limitations](#smart-search) for some further discussion about Smart Search and potential workarounds
  * ⚠ An exception is if you only need to query against [Page fields](#page-fields) as these are also set directly against the page and would therefore be available to the indexer

## Known limitations

Known limitations with Content Components are described below along with a discussion of any known or possible workarounds.

### Smart Search

Content Components are based on Page Types so it is possible to configure [search fields](https://docs.kentico.com/k12sp/configuring-kentico/setting-up-search-on-your-website/using-locally-stored-search-indexes/creating-local-search-indexes/defining-local-page-indexes#Defininglocalpageindexes-Configuringsearchsettingsforpagefields) against them, but the indexer will not know how to extract the component data and apply the search configuration because the data is stored as XML.

A possible (currently untested) workaround is to:

* Handle events related to smart search for [local indexes](https://docs.kentico.com/k12sp/configuring-kentico/setting-up-search-on-your-website/customizing-the-content-of-search-indexes) or [Azure Search](#https://docs.kentico.com/k12sp/configuring-kentico/setting-up-search-on-your-website/using-azure-search/customizing-azure-search#CustomizingAzureSearch-Reference-AzureSearchevents)
* In the event handler(s) [deserialise the component data](#fetch-and-use-component-content), fetch and apply search configuration, and operate on the search index as appropriate to index the component content alongside the standard page fields

## Samples

The repository that contains this module includes a sample site, "KenticoContrib", that uses Content Components and you can explore the code to discover how components are being used to set page metadata.

Please see the main repo README for how to get started with the sample site.
