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

namespace Dali.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfigureMark: Page
    {
        public ConfigureMark()
        {
            this.InitializeComponent();
        }

        private void labelBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void notesBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Mark currentMark = new Mark();
            currentMark = e.Parameter as Mark;
            labelText.Text = currentMark.label;
            noteText.Text = currentMark.type;
        }

    }
}
