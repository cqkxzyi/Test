﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Extension.Configuration.Consul {
    using System;
    using System.Runtime.Serialization;


    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Extension.Configuration.Consul.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }


        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性
        ///   重写当前线程的 CurrentUICulture 属性。
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
        ///   查找类似 文件名不能为null 的本地化字符串。
        /// </summary>
        internal static string Error_InvalidFilePath {
            get {
                return ResourceManager.GetString("Error_InvalidFilePath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Consul的服务名不存在 的本地化字符串。
        /// </summary>
        internal static string Error_InvalidService {
            get {
                return ResourceManager.GetString("Error_InvalidService", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 转换成json错误 的本地化字符串。
        /// </summary>
        internal static string Error_JSONParseError {
            get {
                return ResourceManager.GetString("Error_JSONParseError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 {key}不存在 的本地化字符串。
        /// </summary>
        internal static string Error_KeyNotExist {
            get {
                return ResourceManager.GetString("Error_KeyNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 服务器返回异常 的本地化字符串。
        /// </summary>
        internal static string Error_Request {
            get {
                return ResourceManager.GetString("Error_Request", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 不存在属性值 的本地化字符串。
        /// </summary>
        internal static string Error_ValueNotExist {
            get {
                return ResourceManager.GetString("Error_ValueNotExist", resourceCulture);
            }
        }

    }
}
