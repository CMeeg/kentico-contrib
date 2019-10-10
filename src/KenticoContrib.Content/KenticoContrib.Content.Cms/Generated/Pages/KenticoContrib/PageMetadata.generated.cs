//--------------------------------------------------------------------------------------------------
// <auto-generated>
//
//     This code was generated by code generator tool.
//
//     To customize the code use your own partial class. For more info about how to use and customize
//     the generated code see the documentation at http://docs.kentico.com.
//
// </auto-generated>
//--------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using CMS;
using CMS.Base;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.DocumentEngine.Types.KenticoContrib;
using CMS.DocumentEngine;

[assembly: RegisterDocumentType(PageMetadata.CLASS_NAME, typeof(PageMetadata))]

namespace CMS.DocumentEngine.Types.KenticoContrib
{
	/// <summary>
	/// Represents a content item of type PageMetadata.
	/// </summary>
	public partial class PageMetadata : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "KenticoContrib.PageMetadata";


		/// <summary>
		/// The instance of the class that provides extended API for working with PageMetadata fields.
		/// </summary>
		private readonly PageMetadataFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// PageMetadataID.
		/// </summary>
		[DatabaseIDField]
		public int PageMetadataID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("PageMetadataID"), 0);
			}
			set
			{
				SetValue("PageMetadataID", value);
			}
		}


		/// <summary>
		/// PageMetadataPageTitleSuffix.
		/// </summary>
		[DatabaseField]
		public string PageMetadataPageTitleSuffix
		{
			get
			{
				return ValidationHelper.GetString(GetValue("PageMetadataPageTitleSuffix"), @"");
			}
			set
			{
				SetValue("PageMetadataPageTitleSuffix", value);
			}
		}


		/// <summary>
		/// PageMetadataOpenGraph.
		/// </summary>
		[DatabaseField]
		public string PageMetadataOpenGraph
		{
			get
			{
				return ValidationHelper.GetString(GetValue("PageMetadataOpenGraph"), @"");
			}
			set
			{
				SetValue("PageMetadataOpenGraph", value);
			}
		}


		/// <summary>
		/// PageMetadataTwitter.
		/// </summary>
		[DatabaseField]
		public string PageMetadataTwitter
		{
			get
			{
				return ValidationHelper.GetString(GetValue("PageMetadataTwitter"), @"");
			}
			set
			{
				SetValue("PageMetadataTwitter", value);
			}
		}


		/// <summary>
		/// Gets an object that provides extended API for working with PageMetadata fields.
		/// </summary>
		[RegisterProperty]
		public PageMetadataFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with PageMetadata fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class PageMetadataFields : AbstractHierarchicalObject<PageMetadataFields>
		{
			/// <summary>
			/// The content item of type PageMetadata that is a target of the extended API.
			/// </summary>
			private readonly PageMetadata mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="PageMetadataFields" /> class with the specified content item of type PageMetadata.
			/// </summary>
			/// <param name="instance">The content item of type PageMetadata that is a target of the extended API.</param>
			public PageMetadataFields(PageMetadata instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// PageMetadataID.
			/// </summary>
			public int ID
			{
				get
				{
					return mInstance.PageMetadataID;
				}
				set
				{
					mInstance.PageMetadataID = value;
				}
			}


			/// <summary>
			/// PageMetadataPageTitleSuffix.
			/// </summary>
			public string PageTitleSuffix
			{
				get
				{
					return mInstance.PageMetadataPageTitleSuffix;
				}
				set
				{
					mInstance.PageMetadataPageTitleSuffix = value;
				}
			}


			/// <summary>
			/// PageMetadataOpenGraph.
			/// </summary>
			public string OpenGraph
			{
				get
				{
					return mInstance.PageMetadataOpenGraph;
				}
				set
				{
					mInstance.PageMetadataOpenGraph = value;
				}
			}


			/// <summary>
			/// PageMetadataTwitter.
			/// </summary>
			public string Twitter
			{
				get
				{
					return mInstance.PageMetadataTwitter;
				}
				set
				{
					mInstance.PageMetadataTwitter = value;
				}
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="PageMetadata" /> class.
		/// </summary>
		public PageMetadata() : base(CLASS_NAME)
		{
			mFields = new PageMetadataFields(this);
		}

		#endregion
	}
}