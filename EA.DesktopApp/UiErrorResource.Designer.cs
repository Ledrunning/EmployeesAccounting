﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EA.DesktopApp {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class UiErrorResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal UiErrorResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EA.DesktopApp.UiErrorResource", typeof(UiErrorResource).Assembly);
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
        ///   Looks up a localized string similar to Please, enter valid data.
        /// </summary>
        internal static string DataError {
            get {
                return ResourceManager.GetString("DataError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter the login!.
        /// </summary>
        internal static string EmptyLogin {
            get {
                return ResourceManager.GetString("EmptyLogin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter the password!.
        /// </summary>
        internal static string EmptyPassword {
            get {
                return ResourceManager.GetString("EmptyPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Login cannot contain a space.
        /// </summary>
        internal static string SpaceInlogin {
            get {
                return ResourceManager.GetString("SpaceInlogin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password cannot contain a space.
        /// </summary>
        internal static string SpaceInPassword {
            get {
                return ResourceManager.GetString("SpaceInPassword", resourceCulture);
            }
        }
    }
}