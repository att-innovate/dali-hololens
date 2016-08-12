using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Web.Http.Headers;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using Windows.UI.Core;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Dali.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class webView : Page
    {
        private static Timer timer;


        public webView()
        {
            this.InitializeComponent();

            var selectedMark = Globals.selectedMark;

            switch (selectedMark.type)
            {
                case "note":
                    var path = "Note.html?id=" + selectedMark.id;

                    if (!File.Exists(path))
                    {
                        WebViewControl.Navigate(new Uri("ms-appx-web:///" + path));
                    }
                    break;
                case "tasklist":
                    path = "Tasklist.html?id=" + selectedMark.id;

                    if (!File.Exists(path))
                    {
                        WebViewControl.Navigate(new Uri("ms-appx-web:///" + path));
                    }
                    break;
                case "image":
                    path = "Image.html?id=" + selectedMark.id;

                    if (!File.Exists(path))
                    {
                        WebViewControl.Navigate(new Uri("ms-appx-web:///" + path));
                    }
                    break;
                case "url":
                    try
                    {
                        Uri targetUri = new Uri(selectedMark.content[0]);
                        System.Diagnostics.Debug.WriteLine("Address is invalid, try again.");
                        WebViewControl.Navigate(targetUri);
                    }
                    catch (UriFormatException ex)
                    {
                        // Bad address
                        System.Diagnostics.Debug.WriteLine("Address is invalid, try again.");
                    }
                    break;
                case "":
                    break;
            }

            var statusChecker = new StatusChecker();
            timer = new Timer(statusChecker.CheckStatus,
                                       null, 1000, 500);

            //timer.Dispose();
        }

        class StatusChecker
        {
            private int invokeCount;
            public CoreDispatcher Dispatcher { get; set; }

            public StatusChecker()
            {
                invokeCount = 0;
            }

            // This method is called by the timer delegate.
            public async void CheckStatus(Object stateInfo)
            {

                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    WebView newView = new WebView();
                    newView.Refresh();
                });
            }
        }

        private static string UriToString(Uri uri)
        {
            return (uri != null) ? uri.ToString() : "";
        }

        void WebViewControl_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            string url = UriToString(args.Uri);
        }

        void WebViewControl_UnviewableContentIdentified(WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {

        }

        void WebViewControl_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {

        }

        void WebViewControl_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
        }

        void WebViewControl_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {

        }

        private void WebViewControl_LoadCompleted(object sender, NavigationEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(OldMarks));
        }
    }
}