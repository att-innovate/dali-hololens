using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Dali
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage3 : Page
    {
        public BlankPage3()
        {
            this.InitializeComponent();
            AddControls();
        }



        private void AddControls()
        {
            StackPanel stkpanel = new StackPanel();
            stkpanel.Orientation = Orientation.Horizontal;
            int loc = 20;

            Button dynamicbutton = new Button();

            dynamicbutton.Name = "testButton";
            dynamicbutton.Height = 20;
            dynamicbutton.Width = 50;
            dynamicbutton.Tag = 1;
            dynamicbutton.Margin = new Thickness(5 + loc, 5, 5, 5);
            dynamicbutton.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            dynamicbutton.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            dynamicbutton.Content = "Test??";

            stkpanel.Children.Add(dynamicbutton);
            dynamicbutton.Click += btn_Click;
            //loc += 20;
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void textBlock_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
