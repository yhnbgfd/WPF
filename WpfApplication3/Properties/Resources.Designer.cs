﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wpf.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Wpf.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to insert into main.T_User(username,password,status) values(&apos;root&apos;,&apos;Hh123123&apos;,1);
        ///insert into main.T_User(username,password,status) values(&apos;admin&apos;,&apos;123&apos;,1);
        ///insert into main.T_User(username,password) values(&apos;guest&apos;,&apos;&apos;);
        ///
        ///INSERT INTO &quot;T_Type&quot;(key,value) VALUES (&apos;1&apos;, &apos;预算内户&apos;);
        ///INSERT INTO &quot;T_Type&quot;(key,value) VALUES (&apos;2&apos;, &apos;预算外户&apos;);
        ///INSERT INTO &quot;T_Type&quot;(key,value) VALUES (&apos;3&apos;, &apos;周转金户&apos;);
        ///INSERT INTO &quot;T_Type&quot;(key,value) VALUES (&apos;4&apos;, &apos;计生专户&apos;);
        ///INSERT INTO &quot;T_Type&quot;(key,value) VALUES (&apos;5&apos;, &apos;政粮补贴资金专户&apos;);
        ///INSERT INTO [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string InitData {
            get {
                return ResourceManager.GetString("InitData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE &quot;main&quot;.&quot;T_Report&quot; (
        ///&quot;ID&quot;  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
        ///&quot;DateTime&quot;  TIMESTAMP,
        ///&quot;UnitsName&quot;  TEXT DEFAULT NULL,
        ///&quot;Use&quot;  TEXT DEFAULT NULL,
        ///&quot;Income&quot;  decimal DEFAULT 0,
        ///&quot;Expenses&quot;  decimal DEFAULT 0,
        ///&quot;Type&quot;  INTEGER,
        ///&quot;DeleteTime&quot;  TIMESTAMP DEFAULT NULL
        ///)
        ///;
        ///
        ///CREATE TABLE &quot;main&quot;.&quot;T_User&quot; (
        ///&quot;id&quot;  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
        ///&quot;username&quot;  TEXT NOT NULL,
        ///&quot;password&quot;  TEXT,
        ///&quot;Status&quot;  INTEGER DEFAULT 0,
        ///&quot;remark&quot;  TEXT
        ///)
        ///;
        ///
        ///CREATE TABLE &quot;main&quot;.&quot;T_Log&quot; (
        ///&quot;I [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string InitTable {
            get {
                return ResourceManager.GetString("InitTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon Pyramid_Logo_white_128x128 {
            get {
                object obj = ResourceManager.GetObject("Pyramid_Logo_white_128x128", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
    }
}
