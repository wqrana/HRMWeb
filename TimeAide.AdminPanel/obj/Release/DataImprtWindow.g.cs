﻿#pragma checksum "..\..\DataImprtWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F0EFCECED2C1258ADFD0EE2662E58AA32D2FF4D3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using TimeAide.AdminPanel;


namespace TimeAide.AdminPanel {
    
    
    /// <summary>
    /// DataImprtWindow
    /// </summary>
    public partial class DataImprtWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 19 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSourceClient;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtTA7Database;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cboExecutionType;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtImportFilePath;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnShowFieldsMapping;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCloseApplication;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl importTab;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtImportFileTemplateName;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtImportFileTemplateId;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSaveMappingData;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnProcessImportData;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtNotify;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgImportFile;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ProgressHeading;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar DocMigProgress;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\DataImprtWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtLogStatus;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TimeAide.AdminPanel;component/dataimprtwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\DataImprtWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\DataImprtWindow.xaml"
            ((TimeAide.AdminPanel.DataImprtWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtSourceClient = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.txtTA7Database = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.cboExecutionType = ((System.Windows.Controls.ComboBox)(target));
            
            #line 27 "..\..\DataImprtWindow.xaml"
            this.cboExecutionType.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cboExecutionType_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 29 "..\..\DataImprtWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnLoadTemplates_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtImportFilePath = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            
            #line 38 "..\..\DataImprtWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnBrowseFilePath_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnShowFieldsMapping = ((System.Windows.Controls.Button)(target));
            
            #line 45 "..\..\DataImprtWindow.xaml"
            this.btnShowFieldsMapping.Click += new System.Windows.RoutedEventHandler(this.btnShowFieldsMapping_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnCloseApplication = ((System.Windows.Controls.Button)(target));
            
            #line 46 "..\..\DataImprtWindow.xaml"
            this.btnCloseApplication.Click += new System.Windows.RoutedEventHandler(this.btnCloseApplication_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.importTab = ((System.Windows.Controls.TabControl)(target));
            return;
            case 11:
            this.txtImportFileTemplateName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 12:
            this.txtImportFileTemplateId = ((System.Windows.Controls.TextBox)(target));
            return;
            case 13:
            this.btnSaveMappingData = ((System.Windows.Controls.Button)(target));
            
            #line 66 "..\..\DataImprtWindow.xaml"
            this.btnSaveMappingData.Click += new System.Windows.RoutedEventHandler(this.btnSaveMappingData_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.btnProcessImportData = ((System.Windows.Controls.Button)(target));
            
            #line 67 "..\..\DataImprtWindow.xaml"
            this.btnProcessImportData.Click += new System.Windows.RoutedEventHandler(this.btnProcessImportData_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            this.txtNotify = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 16:
            this.dgImportFile = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 18:
            this.ProgressHeading = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 19:
            this.DocMigProgress = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 20:
            this.txtLogStatus = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 17:
            
            #line 84 "..\..\DataImprtWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnShowFieldMapping_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

