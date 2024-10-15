﻿#pragma checksum "..\..\DataExportWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "860E0365FD11644493E2686C28BFF8B418230141833D725ABBAB088BB73E7E33"
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
    /// DataExportWindow
    /// </summary>
    public partial class DataExportWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\DataExportWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSourceClient;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\DataExportWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cboExecutionType;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\DataExportWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtTA7Database;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\DataExportWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtTAWDatabase;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\DataExportWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtAppRootPath;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\DataExportWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnExportData;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\DataExportWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCloseApplication;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\DataExportWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl tcOpenPages;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\DataExportWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ProgressHeading;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\DataExportWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar DocMigProgress;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\DataExportWindow.xaml"
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
            System.Uri resourceLocater = new System.Uri("/TimeAide.AdminPanel;component/dataexportwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\DataExportWindow.xaml"
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
            
            #line 9 "..\..\DataExportWindow.xaml"
            ((TimeAide.AdminPanel.DataExportWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtSourceClient = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.cboExecutionType = ((System.Windows.Controls.ComboBox)(target));
            
            #line 21 "..\..\DataExportWindow.xaml"
            this.cboExecutionType.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cboExecutionType_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtTA7Database = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.txtTAWDatabase = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.txtAppRootPath = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            
            #line 40 "..\..\DataExportWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnBrowseFilePath_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnExportData = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\DataExportWindow.xaml"
            this.btnExportData.Click += new System.Windows.RoutedEventHandler(this.BtnExportData_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnCloseApplication = ((System.Windows.Controls.Button)(target));
            
            #line 48 "..\..\DataExportWindow.xaml"
            this.btnCloseApplication.Click += new System.Windows.RoutedEventHandler(this.btnCloseApplication_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.tcOpenPages = ((System.Windows.Controls.TabControl)(target));
            return;
            case 11:
            this.ProgressHeading = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 12:
            this.DocMigProgress = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 13:
            this.txtLogStatus = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

