﻿#pragma checksum "C:\Users\Sarah Radzihovsky\Documents\Dali-HoloLens\Dali\Views\OldNewMenu.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4A72C67979C29ABB99F5C1EA9F107BBF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dali.Views
{
    partial class OldNewMenu : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.textlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                    #line 15 "..\..\..\Views\OldNewMenu.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBlock)this.textlock).SelectionChanged += this.textBlock_SelectionChanged;
                    #line default
                }
                break;
            case 2:
                {
                    this.newButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 16 "..\..\..\Views\OldNewMenu.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.newButton).Click += this.newButton_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.oldButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 17 "..\..\..\Views\OldNewMenu.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.oldButton).Click += this.oldButton_Click;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

