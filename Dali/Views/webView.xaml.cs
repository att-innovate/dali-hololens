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
    public sealed partial class webView : Page
    {
        public webView()
        {
            this.InitializeComponent();
        }

        static string UriToString(Uri uri)
        {
            return (uri != null) ? uri.ToString() : "";
        }

        /// <summary>
        /// This is the click handler for the "Go" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoButton_Click()
        {
            if (!pageIsLoading)
            {
                NavigateWebview(AddressBox.Text);
            }
            else
            {
                WebViewControl.Stop();
                pageIsLoading = false;
            }
        }

        /// <summary>
        /// Property to control the "Go" button text, forward/backward buttons and progress ring.
        /// </summary>
        private bool _pageIsLoading;
        bool pageIsLoading
        {
            get { return _pageIsLoading; }
            set
            {
                _pageIsLoading = value;
                GoButton.Content = (value ? "Stop" : "Go");
                ProgressControl.IsActive = value;

                if (!value)
                {
                    NavigateBackButton.IsEnabled = WebViewControl.CanGoBack;
                    NavigateForwardButton.IsEnabled = WebViewControl.CanGoForward;
                }
            }
        }

        /// <summary>
        /// Handler for the NavigateBackward button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigateBackward_Click()
        {
            if (WebViewControl.CanGoBack) WebViewControl.GoBack();
        }

        /// <summary>
        /// Handler for the GoForward button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigateForward_Click()
        {
            if (WebViewControl.CanGoForward) WebViewControl.GoForward();
        }

        /// <summary>
        /// This handles the enter key in the url address box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Address_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                NavigateWebview(AddressBox.Text);
            }
        }

        private const string htmlFragment =
                   "<html><head><script type='text/javascript'>" +
                   "function doubleIt(incoming){ " +
                   "  var intIncoming = parseInt(incoming, 10);" +
                   "  var doubled = intIncoming * 2;" +
                   "  document.body.style.fontSize= doubled.toString() + 'px';" +
                   "  window.external.notify('The script says the doubled value is ' + doubled.toString());" +
                   "};" +
                   "</script></head><body><div id='myDiv'>I AM CONTENT</div></body></html>";

        /// <summary>
        /// Helper to perform the navigation in webview
        /// </summary>
        /// <param name="url"></param>
        private void NavigateWebview(string url)
        {
            try
            {
                Uri targetUri = new Uri(url);
                WebViewControl.Navigate(targetUri);
               // WebViewControl.NavigateToString(htmlFragment);
            }
            catch (UriFormatException ex)
            {
                // Bad address
                AppendLog($"Address is invalid, try again. Error: {ex.Message}.");
            }
        }

        /// <summary>
        /// Handle the event that indicates that WebView is starting a navigation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void WebViewControl_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            string url = UriToString(args.Uri);
            AddressBox.Text = url;
            AppendLog($"Starting navigation to: \"{url}\".");
            pageIsLoading = true;
        }

        /// <summary>
        /// Handle the event that indicates that the WebView content is not a web page.
        /// For example, it may be a file download.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void WebViewControl_UnviewableContentIdentified(WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {
            AppendLog($"Content for \"{UriToString(args.Uri)}\" cannot be loaded into webview.");
            // We throw away the request. See the "Unviewable content" scenario for other
            // ways of handling the event.
            pageIsLoading = false;
        }

        /// <summary>
        /// Handle the event that indicates that WebView has resolved the URI, and that it is loading HTML content.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void WebViewControl_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            AppendLog($"Loading content for \"{UriToString(args.Uri)}\".");
        }


        /// <summary>
        /// Handle the event that indicates that the WebView content is fully loaded.
        /// If you need to invoke script, it is best to wait for this event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void WebViewControl_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            AppendLog($"Content for \"{UriToString(args.Uri)}\" has finished loading.");
        }

        /// <summary>
        /// Event to indicate webview has completed the navigation, either with success or failure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void WebViewControl_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            pageIsLoading = false;
            if (args.IsSuccess)
            {
                AppendLog($"Navigation to \"{UriToString(args.Uri)}\" completed successfully.");
            }
            else
            {
                AppendLog($"Navigation to: \"{UriToString(args.Uri)}\" failed with error {args.WebErrorStatus}.");
            }
        }

        /// <summary>
        /// Helper for logging
        /// </summary>
        /// <param name="logEntry"></param>
        void AppendLog(string logEntry)
        {
            logScroller.ChangeView(0, logScroller.ScrollableHeight, null);
        }
    }
}