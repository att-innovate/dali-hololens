
/*
The MIT License(MIT)

Copyright(c) 2017 AT&T

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
fu
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
rnished to do so, subject to the following conditions:

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using Dali.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Dali.Views
{
    public sealed partial class WebView : Page
    {
        private bool shouldRefresh = false;
        private string previousLink = "";
        private Mark activeMark;

        public WebView()
        {
            this.InitializeComponent();
            activeMark = Globals.selectedMark;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            shouldRefresh = true;
            previousLink = "";
            LoadAndRefresh(activeMark, WebViewControl);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            shouldRefresh = false;
            base.OnLostFocus(e);
        }

        async private void LoadAndRefresh(Mark selectedMark, Windows.UI.Xaml.Controls.WebView WebViewControl)
        {
            if (selectedMark.type == "undefined") // just to be sure mark got created
                await Task.Delay(TimeSpan.FromSeconds(2));

            while (shouldRefresh)
            {

                if (selectedMark.type == "link")
                {
                    GetRequest(Globals.serverUrl);
                }
                else
                {
                    GetRequest(Globals.serverUrl);
                    var file = selectedMark.type + ".html?id=" + selectedMark.id;
                    WebViewControl.Navigate(new Uri(Globals.serverUrl + "/template/" + file));
                }

                if (shouldRefresh)
                    await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        private void GetRequest(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    System.Diagnostics.Debug.WriteLine(Globals.selectedMark.id);

                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders
                                .Accept
                                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "relativeAddress");

                    using (var response = client.GetAsync("/mark/" + Globals.selectedMark.id).Result)
                    {

                        string responseString = response.Content.ReadAsStringAsync().Result;
                        System.Diagnostics.Debug.WriteLine(responseString);

                        var resultObject = JObject.Parse(responseString);
                        var newMark = resultObject.ToObject<Mark>();

                        Globals.selectedMark.id = newMark.id;
                        Globals.selectedMark.label = newMark.label;
                        Globals.selectedMark.type = newMark.type;

                        if (newMark.type == "link")
                        {
                            MarkStringContent linkMark = resultObject.ToObject<MarkStringContent>();
                            if (linkMark.content != previousLink)
                            {
                                previousLink = linkMark.content;
                                NavigateToURL(linkMark);
                            }
                        }
                        else
                        {
                            previousLink = "";
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("CAUGHT EXCEPTION:");
                System.Diagnostics.Debug.WriteLine(exception);
            }
        }

        private void NavigateToURL(MarkStringContent linkMark)
        {
            try
            {
                Uri targetUri = new Uri(linkMark.content);
                WebViewControl.Navigate(targetUri);
            }
            catch (UriFormatException)
            {
                System.Diagnostics.Debug.WriteLine("Address is invalid, try again.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(OldMarks));
        }
    }
}