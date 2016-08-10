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
            try
            {
                var selectedMark = Globals.selectedMark;

                System.IO.File.WriteAllText("NoteHtml.txt", string.Empty);
                List<string> lines = new List<string>
                    { "<html>", "<body>", "Label:", "</body>", "</html>" };

                switch (selectedMark.type)
                {
                    case "note":
                        lines.Insert(3, selectedMark.label);
                        lines.Insert(4, "Note: " + selectedMark.content[0]);
                        File.WriteAllLines("NoteHtml.txt", lines.ToArray());
                        string path = File.ReadAllText("NoteHtml.txt");

                        if (!File.Exists(path))
                        {
                            WebViewControl.NavigateToString(path);
                        }
                        break;
                    case "tasklist":
                        lines.Insert(3, selectedMark.label);
                        lines.Insert(4, "Tasklist <br/>");
                        for (int i = 5; i < (selectedMark.content.Length + 4); i++)
                        {
                            lines.Insert(i, selectedMark.content[i - 5]);

                        }
                        File.WriteAllLines("NoteHtml.txt", lines.ToArray());
                        path = File.ReadAllText("NoteHtml.txt");

                        if (!File.Exists(path))
                        {
                            WebViewControl.NavigateToString(path);
                        }
                        break;
                    case "image":
                        lines.Insert(3, selectedMark.label);
                        lines.Insert(4, "< h2 >" + selectedMark.content[0] + "</ h2 >");
                        lines.Insert(5, "<img src='pic_mountain.jpg' alt='Mountain View' style='width: 304px; height: 228px; '>");
                        File.WriteAllLines("NoteHtml.txt", lines.ToArray());
                        path = File.ReadAllText("NoteHtml.txt");

                        if (!File.Exists(path))
                        {
                            WebViewControl.NavigateToString(path);
                        }
                        break;
                    case "url":
                        Uri targetUri = new Uri(selectedMark.content[0]);
                        WebViewControl.Navigate(targetUri);
                        break;
                    case "":
                        lines.Insert(3, "This is a new line...");
                        File.WriteAllLines("NoteHtml.txt", lines.ToArray());
                        path = File.ReadAllText("NoteHtml.txt");

                        if (!File.Exists(path))
                        {
                            WebViewControl.NavigateToString(path);
                        }
                        break;
                }

            }
            catch (UriFormatException ex)
            {
                // Bad address
                AppendLog($"Address is invalid, try again. Error: {ex.Message}.");
            }
        }

        static string UriToString(Uri uri)
        {
            return (uri != null) ? uri.ToString() : "";
        }

        /// <summary>
        /// Handle the event that indicates that WebView is starting a navigation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void WebViewControl_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            string url = UriToString(args.Uri);
            AppendLog($"Starting navigation to: \"{url}\".");
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