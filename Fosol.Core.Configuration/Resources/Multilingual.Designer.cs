﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fosol.Core.Configuration.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Multilingual {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Multilingual() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Fosol.Core.Configuration.Resources.Multilingual", typeof(Multilingual).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attribute &quot;{0}&quot; is required but missing..
        /// </summary>
        internal static string CDataConfigurationElement_Attribute_Missing {
            get {
                return ResourceManager.GetString("CDataConfigurationElement_Attribute_Missing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attribute &quot;{0}&quot; must only have one occurance..
        /// </summary>
        internal static string CDataConfigurationElement_Attribute_Multiple {
            get {
                return ResourceManager.GetString("CDataConfigurationElement_Attribute_Multiple", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ConfigurationElement &quot;{0}&quot; must be of type string..
        /// </summary>
        internal static string CDataConfigurationElement_InvalidType {
            get {
                return ResourceManager.GetString("CDataConfigurationElement_InvalidType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Configuration key &quot;{0}&quot; does not exist..
        /// </summary>
        internal static string ConfigurationHelper_GetAppSetting_KeyDoesNotExist {
            get {
                return ResourceManager.GetString("ConfigurationHelper_GetAppSetting_KeyDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter &quot;{0}&quot; must point to an existing file (&quot;{1}&quot;)..
        /// </summary>
        internal static string ConfigurationSectionFileWatcher_DeserializeSection_FileNotFound {
            get {
                return ResourceManager.GetString("ConfigurationSectionFileWatcher_DeserializeSection_FileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File &quot;{0}&quot; was not found..
        /// </summary>
        internal static string ConfigurationSectionFileWatcher_LoadConfigWithoutLock_FileNotFound {
            get {
                return ResourceManager.GetString("ConfigurationSectionFileWatcher_LoadConfigWithoutLock_FileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Section &quot;{0}&quot; was not found..
        /// </summary>
        internal static string ConfigurationSectionFileWatcher_LoadConfigWithoutLock_SectionNotFound {
            get {
                return ResourceManager.GetString("ConfigurationSectionFileWatcher_LoadConfigWithoutLock_SectionNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The URI value is not a valid URI..
        /// </summary>
        internal static string Validators_UriValidator_InvalidUri {
            get {
                return ResourceManager.GetString("Validators_UriValidator_InvalidUri", resourceCulture);
            }
        }
    }
}
