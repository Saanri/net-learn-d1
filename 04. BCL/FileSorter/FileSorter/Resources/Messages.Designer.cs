﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FileSorter.Resources {
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
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FileSorter.Resources.Messages", typeof(Messages).Assembly);
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
        ///   Looks up a localized string similar to {0} : The monitoring is activated. The directory : {1}.
        /// </summary>
        internal static string ActivateDirWatcherMsg {
            get {
                return ResourceManager.GetString("ActivateDirWatcherMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The default directory for moving files: {0}.
        /// </summary>
        internal static string DefaultDirMoveMsg {
            get {
                return ResourceManager.GetString("DefaultDirMoveMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Directories for watch:.
        /// </summary>
        internal static string DirsWatchMsg {
            get {
                return ResourceManager.GetString("DirsWatchMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} : File detected: {1}.
        /// </summary>
        internal static string FileDetectedMsg {
            get {
                return ResourceManager.GetString("FileDetectedMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} : File {1} moved to {2}.
        /// </summary>
        internal static string FileMovedMsg {
            get {
                return ResourceManager.GetString("FileMovedMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: File {1} is not migrated. This file already exists..
        /// </summary>
        internal static string FileNotMovedMsg {
            get {
                return ResourceManager.GetString("FileNotMovedMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} : Press Ctrl+C or Ctrl+Brake to exit..
        /// </summary>
        internal static string ForExitMsg {
            get {
                return ResourceManager.GetString("ForExitMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loading settings....
        /// </summary>
        internal static string LoadSettingsMsg {
            get {
                return ResourceManager.GetString("LoadSettingsMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rule (mask) {0} - directory {1}.
        /// </summary>
        internal static string RuleDirMsg {
            get {
                return ResourceManager.GetString("RuleDirMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} : Rule found for file &quot;{1}&quot;....
        /// </summary>
        internal static string RuleFoundMsg {
            get {
                return ResourceManager.GetString("RuleFoundMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} : Rule not found for file &quot;{1}&quot;....
        /// </summary>
        internal static string RuleNotFoundMsg {
            get {
                return ResourceManager.GetString("RuleNotFoundMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}: The process is running....
        /// </summary>
        internal static string StartMsg {
            get {
                return ResourceManager.GetString("StartMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Uploaded rules:.
        /// </summary>
        internal static string UploadedRulesMsg {
            get {
                return ResourceManager.GetString("UploadedRulesMsg", resourceCulture);
            }
        }
    }
}
