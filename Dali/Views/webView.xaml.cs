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
            var statusChecker = new StatusChecker();
            timer = new Timer(statusChecker.CheckStatus,
                                       null, 1000, 500);

            //timer.Dispose();
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
            public CoreDispatcher Dispatcher { get; set; }

            public StatusChecker()
            {
                invokeCount = 0;
            }

            // This method is called by the timer delegate.
            public async void CheckStatus(Object stateInfo)
            {
                System.Diagnostics.Debug.WriteLine("{0} Checking status {1,2}.",
                    DateTime.Now.ToString("h:mm:ss.fff"),
                    (++invokeCount).ToString());

                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {

                    GetRequest("http://10.250.3.24:8085", Globals.selectedMark.id);
                    WebView newView = new WebView();
                    newView.Refresh();
                    System.Diagnostics.Debug.WriteLine("refresh");
                });
            }

            public async void GetRequest(string url, string id)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(url);
                        client.DefaultRequestHeaders
                                    .Accept
                                     .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "relativeAddress");

                        using (var response = client.GetAsync("/mark/" + id).Result)
                        {

                            string responseString = response.Content.ReadAsStringAsync().Result;
                            System.Diagnostics.Debug.WriteLine("GET SUCCESS:");
                            System.Diagnostics.Debug.WriteLine(responseString);


                        }
                    }
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine("CAUGHT EXCEPTION:");
                    System.Diagnostics.Debug.WriteLine(exception);
                }
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