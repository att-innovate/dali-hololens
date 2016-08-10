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
                switch(selectedMark.type)
                {
                    case "note":
                        WebViewControl.NavigateToString(htmlNote);
                        break;
                    case "tasklist":
                        WebViewControl.NavigateToString(htmlTasklist);
                        break;
                    case "image":
                        WebViewControl.NavigateToString(htmlImage);
                        break;
                    case "url":
                        Uri targetUri = new Uri(selectedMark.content[0]);
                        WebViewControl.Navigate(targetUri);
                        break;
                    case "":
                        //string path = File.ReadAllText(@"C:\User\Sarah Radzihovsky\Desktop\HtmlTest.txt");
                        // var path = Path.Combine(Directory.GetCurrentDirectory(), "HtmlTest.txt");

                        var allLines = File.ReadAllLines("NoteHtml.txt").ToList();
                        allLines.Insert(3, "This is a new line...");
                        File.WriteAllLines("NoteHtml.txt", allLines.ToArray());

                       // StreamWriter file = new StreamWriter((@"C:\HtmlTest.txt");
                      //  file.WriteLine(lines);
                        string path = File.ReadAllText("NoteHtml.txt");

                       /* var sb = new StringBuilder();
                        using (var sr = new StreamReader("inputFileName"))
                        {
                            string line;
                            do
                            {
                                line = sr.ReadLine();
                                sb.AppendLine(line);
                            } while (!line.Contains("<body>"));

                            sb.Append(selectedMark.content.);
                            sb.Append(sr.ReadToEnd());
                        }

                        using (var sr = new StreamWriter("outputFileName"))
                        {
                            sr.Write(sb.ToString());
                        }


                        // Example #4: Append new text to an existing file.
                        // The using statement automatically flushes AND CLOSES the stream and calls 
                        // IDisposable.Dispose on the stream object.
                        using (System.IO.StreamWriter file =
                            new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt", true))
                        {
                            file.WriteLine("Fourth line");
                        }
                        */

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


        //private static string html1 = System.IO.File.ReadAllText(@"c:\Desktop\HtmlTest.txt");

        private static string html2 =
            "<html><body>Label:<br/>" +
            "</body></html>";

        private static string html = 
                        @"<html>
                    <head>
                    <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
                    </head>
                      <div></div>
                    <body>
                    </body>
                    </html>";

        private const string htmlNote =
                   "<html><head><script type='text/javascript'>" +
                  "function doubleIt(incoming){ " +
                  "  var intIncoming = parseInt(incoming, 10);" +
                  "  var doubled = intIncoming * 2;" +
                  "  document.body.style.fontSize= doubled.toString() + 'px';" +
                  "  window.external.notify('The script says the doubled value is ' + doubled.toString());" +
                  "};" +
                  "</script></head><body><div id='myDiv'>I AM CONTENT</div></body></html>";

        private const string htmlTasklist =
                  "<html><head><script type='text/javascript'>" +
                  "function doubleIt(incoming){ " +
                  "  var intIncoming = parseInt(incoming, 10);" +
                  "  var doubled = intIncoming * 2;" +
                  "  document.body.style.fontSize= doubled.toString() + 'px';" +
                  "  window.external.notify('The script says the doubled value is ' + doubled.toString());" +
                  "};" +
                  "</script></head><body><div id='myDiv'>I AM CONTENT</div></body></html>";

        private const string htmlImage =
                  "<html><head><script type='text/javascript'>" +
                  "function doubleIt(incoming){ " +
                  "  var intIncoming = parseInt(incoming, 10);" +
                  "  var doubled = intIncoming * 2;" +
                  "  document.body.style.fontSize= doubled.toString() + 'px';" +
                  "  window.external.notify('The script says the doubled value is ' + doubled.toString());" +
                  "};" +
                  "</script></head><body><div id='myDiv'>I AM CONTENT</div></body></html>";

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