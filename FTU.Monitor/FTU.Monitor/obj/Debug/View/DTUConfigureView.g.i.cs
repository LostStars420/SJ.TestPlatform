﻿#pragma checksum "..\..\..\View\DTUConfigureView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1B3928071429E736BEEB83661180FEA1"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using FTU.Monitor.Model;
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


namespace FTU.Monitor.View {
    
    
    /// <summary>
    /// DTUConfigureView
    /// </summary>
    public partial class DTUConfigureView : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 19 "..\..\..\View\DTUConfigureView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView TreeView_NodeList;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\View\DTUConfigureView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContextMenu ContextMenu_EditNode;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\View\DTUConfigureView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem MenuItem_AddNode;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\View\DTUConfigureView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem MenuItem_DeleteNode;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\View\DTUConfigureView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem MenuItem_AddChildrenNode;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\View\DTUConfigureView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid ConfigureData;
        
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
            System.Uri resourceLocater = new System.Uri("/FTU.Monitor;component/view/dtuconfigureview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\DTUConfigureView.xaml"
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
            
            #line 9 "..\..\..\View\DTUConfigureView.xaml"
            ((FTU.Monitor.View.DTUConfigureView)(target)).Loaded += new System.Windows.RoutedEventHandler(this.DTUConfigure_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TreeView_NodeList = ((System.Windows.Controls.TreeView)(target));
            
            #line 19 "..\..\..\View\DTUConfigureView.xaml"
            this.TreeView_NodeList.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.DoubleClickedNode);
            
            #line default
            #line hidden
            
            #line 19 "..\..\..\View\DTUConfigureView.xaml"
            this.TreeView_NodeList.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(this.SingleClickedNode);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ContextMenu_EditNode = ((System.Windows.Controls.ContextMenu)(target));
            return;
            case 4:
            this.MenuItem_AddNode = ((System.Windows.Controls.MenuItem)(target));
            
            #line 29 "..\..\..\View\DTUConfigureView.xaml"
            this.MenuItem_AddNode.Click += new System.Windows.RoutedEventHandler(this.MenuItem_AddNode_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.MenuItem_DeleteNode = ((System.Windows.Controls.MenuItem)(target));
            
            #line 30 "..\..\..\View\DTUConfigureView.xaml"
            this.MenuItem_DeleteNode.Click += new System.Windows.RoutedEventHandler(this.MenuItem_DeleteNode_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.MenuItem_AddChildrenNode = ((System.Windows.Controls.MenuItem)(target));
            
            #line 31 "..\..\..\View\DTUConfigureView.xaml"
            this.MenuItem_AddChildrenNode.Click += new System.Windows.RoutedEventHandler(this.MenuItem_AddChildrenNode_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ConfigureData = ((System.Windows.Controls.DataGrid)(target));
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
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 7:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.PreviewMouseRightButtonDownEvent;
            
            #line 36 "..\..\..\View\DTUConfigureView.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.TreeViewItem_PreviewMouseRightButtonDown);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}
