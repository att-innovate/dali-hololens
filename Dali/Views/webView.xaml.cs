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
using System.Threading;

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

            //set label at top of UI
            this.MarkName.Text = "Mark Label: " + selectedMark.label;

            //reset html
            System.IO.File.WriteAllText("NoteHtml.txt", string.Empty);
            List<string> lines = new List<string>
                    { "<html>", "<body>", "Label:", "</body>", "</html>" };

            switch (selectedMark.type)
            {
                case "note":
                    lines.Insert(3, selectedMark.label + "<br/>");
                    lines.Insert(4, "Note: " + selectedMark.content[0]);
                    File.WriteAllLines("NoteHtml.txt", lines.ToArray());
                    string path = File.ReadAllText("NoteHtml.txt");

                    if (!File.Exists(path))
                    {
                        WebViewControl.NavigateToString(path);
                    }
                    break;
                case "tasklist":
                    lines.Insert(3, selectedMark.label + "<br/>");
                    lines.Insert(4, "Tasklist <br/>");
                    for (int i = 5; i < (selectedMark.content.Count + 5); i++)
                    {
                        lines.Insert(i, "<li>" + selectedMark.content[i - 5] + "</li>");

                    }
                    File.WriteAllLines("NoteHtml.txt", lines.ToArray());
                    path = File.ReadAllText("NoteHtml.txt");

                    if (!File.Exists(path))
                    {
                        WebViewControl.NavigateToString(path);
                    }
                    break;
                case "image":
                    displayImage(selectedMark);
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

            // Create an AutoResetEvent to signal the timeout threshold in the
            // timer callback has been reached.
            var autoEvent = new AutoResetEvent(false);

            var statusChecker = new StatusChecker(10);

            // Create a timer that invokes CheckStatus after one second, 
            // and every 1/4 second thereafter.

            var timer = new Timer(statusChecker.CheckStatus,
                                       autoEvent, 1000, 500);
            while (true)
            {
                autoEvent.WaitOne();
                refresh();
                System.Diagnostics.Debug.WriteLine("refresh");
            }

            timer.Dispose();
            System.Diagnostics.Debug.WriteLine("\nDestroying timer.");
        }

        private void displayImage(Mark selectedMark)
        {
            switch (selectedMark.content[0])
            {
                case "Up Arrow":
                    Uri imageUri = new Uri("https://upload.wikimedia.org/wikipedia/commons/6/61/Black_Up_Arrow.png");
                    WebViewControl.Navigate(imageUri);
                    break;
                case "Down Arrow":
                    imageUri = new Uri("https://upload.wikimedia.org/wikipedia/en/e/e0/Black_Down_Arrow.png");
                    WebViewControl.Navigate(imageUri);
                    break;
                case "Danger":
                    imageUri = new Uri("https://lh6.ggpht.com/JWk0waKYxAVbuIavYsRimLK3859m_s-MWJpSXkoQ8ejLpPvge_iF_xHiomfMAgYb1oF-=w300");
                    WebViewControl.Navigate(imageUri);
                    break;
                case "Don't Touch":
                    imageUri = new Uri("http://www.alphaecological.com/wp-content/uploads/2014/10/Do-Not-Touch-300x265.jpg");
                    WebViewControl.Navigate(imageUri);
                    break;
            }
        }

        class StatusChecker
        {
            private int invokeCount;
            private int maxCount;

            public StatusChecker(int count)
            {
                invokeCount = 0;
                maxCount = count;
            }

            // This method is called by the timer delegate.
            public void CheckStatus(Object stateInfo)
            {
                AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
                System.Diagnostics.Debug.WriteLine("{0} Checking status {1,2}.",
                    DateTime.Now.ToString("h:mm:ss.fff"),
                    (++invokeCount).ToString());

                if (invokeCount == maxCount)
                {
                    // Reset the counter and signal the waiting thread.
                    invokeCount = 0;
                    autoEvent.Set();
                }
            }
        }

        private static void TimerCallback(Object state)
        {
            OldMarks newClass = new OldMarks();
            newClass.GetRequest("http://10.250.3.24:8085");

            var selectedMark = Globals.selectedMark;
            var newMarks = Globals.marks;
            for (int i = 0; i < newMarks.Count; i++)
            {
                if (newMarks[i].label == selectedMark.label)
                {
                    selectedMark = newMarks[i];
                }
            }
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("h:mm:ss"));
        }

        public static void refresh()
        {
            OldMarks newClass = new OldMarks();
            newClass.GetRequest("http://10.250.3.24:8085");

            var selectedMark = Globals.selectedMark;
            var newMarks = Globals.marks;
            for (int i = 0; i < newMarks.Count; i++)
            {
                if (newMarks[i].label == selectedMark.label)
                {
                    selectedMark = newMarks[i];
                }
            }
        }
        /* lines.Insert(3, selectedMark.label);
                         lines.Insert(4, "<h2>" + selectedMark.content[0] + "</h2>");
 //                        lines.Insert(4, "<h2>Danger</h2>");

                         lines.Insert(5, "<img src='Danger.jpg' alt='Danger' style='width: 210px; height: 240px; '>");
                         File.WriteAllLines("NoteHtml.txt", lines.ToArray());
                         path = File.ReadAllText("NoteHtml.txt");

                         if (!File.Exists(path))
                         {
                             WebViewControl.NavigateToString(path);
                         }*/


        private static string UriToString(Uri uri)
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
        }

        /// <summary>
        /// Handle the event that indicates that the WebView content is not a web page.
        /// For example, it may be a file download.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void WebViewControl_UnviewableContentIdentified(WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {

        }

        /// <summary>
        /// Handle the event that indicates that WebView has resolved the URI, and that it is loading HTML content.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void WebViewControl_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {

        }

        /// <summary>
        /// Handle the event that indicates that the WebView content is fully loaded.
        /// If you need to invoke script, it is best to wait for this event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void WebViewControl_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
        }

        /// <summary>
        /// Event to indicate webview has completed the navigation, either with success or failure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
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