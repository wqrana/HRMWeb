﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TimeAide.Web.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.8.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://tawqc.timeaide.com:9001/wsGetSessionToken.asmx")]
        public string TimeAide_Web_WebPunchSessionTokenService_wsGetSessionToken {
            get {
                return ((string)(this["TimeAide_Web_WebPunchSessionTokenService_wsGetSessionToken"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://tawqc.timeaide.com:9001/wsLogin.asmx")]
        public string TimeAide_Web_WebPunchLoginService_wsLogin {
            get {
                return ((string)(this["TimeAide_Web_WebPunchLoginService_wsLogin"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://tawqc.timeaide.com:9001/wsEmployeeLogin.asmx")]
        public string TimeAide_Web_WebPunchEmployeeLoginService_wsEmployeeLogin {
            get {
                return ((string)(this["TimeAide_Web_WebPunchEmployeeLoginService_wsEmployeeLogin"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://tawqc.timeaide.com:9001/wsValidateEvent.asmx")]
        public string TimeAide_Web_WebPunchValidateEventService_wsValidateEvent {
            get {
                return ((string)(this["TimeAide_Web_WebPunchValidateEventService_wsValidateEvent"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://tawqc.timeaide.com:9001/wsComputeTimeSheet.asmx")]
        public string TimeAide_Web_WebPunchComputeService2_wsComputeTimeSheet {
            get {
                return ((string)(this["TimeAide_Web_WebPunchComputeService2_wsComputeTimeSheet"]));
            }
        }
    }
}
