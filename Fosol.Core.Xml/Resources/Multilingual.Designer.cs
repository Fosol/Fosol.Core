﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fosol.Core.Xml.Resources {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Fosol.Core.Xml.Resources.Multilingual", typeof(Multilingual).Assembly);
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
        ///   Looks up a localized string similar to Parameter &quot;{0}&quot; must be writable..
        /// </summary>
        internal static string Extensions_XmlObjectSerializerExtensions_ToStream_CanWrite {
            get {
                return ResourceManager.GetString("Extensions_XmlObjectSerializerExtensions_ToStream_CanWrite", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter &quot;{0}&quot; must be writable..
        /// </summary>
        internal static string Extensions_XmlSerializerExtensions_ToStream_CanWrite {
            get {
                return ResourceManager.GetString("Extensions_XmlSerializerExtensions_ToStream_CanWrite", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid CDATA within the XML..
        /// </summary>
        internal static string Serialization_CData_InvalidXML {
            get {
                return ResourceManager.GetString("Serialization_CData_InvalidXML", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter &quot;{0}.CanRead&quot; must be true..
        /// </summary>
        internal static string Serialization_XmlHelper_Deserialize_CanRead {
            get {
                return ResourceManager.GetString("Serialization_XmlHelper_Deserialize_CanRead", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter &quot;{0}.CanWrite&quot; must be true..
        /// </summary>
        internal static string Serialization_XmlHelper_Serialize_CanWrite {
            get {
                return ResourceManager.GetString("Serialization_XmlHelper_Serialize_CanWrite", resourceCulture);
            }
        }
    }
}