
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Net.Http.Headers;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Dali.Data;
using System.Threading.Tasks;

namespace Dali.Views
{
    public sealed partial class SetNewMark : Page
    {
        public SetNewMark()
        {
            this.InitializeComponent();
        }

        async private void PostRequest(string url, string newLabel)
        {
            string newId = GetId(url);
            if (newId == "") return;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);

                    var mark = new MarkStringContent();
                    mark.id = newId;
                    mark.label = newLabel;
                    mark.type = "undefined";
                    mark.content = "";

                    Globals.selectedMark = mark;
                    Globals.selectedMarksOnView[Globals.tileId] = mark;

                    var json = JsonConvert.SerializeObject(mark);
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                    var httpResponse = await client.PostAsync("/mark/", httpContent);

                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(responseContent);
                    }
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("CAUGHT EXCEPTION:");
                System.Diagnostics.Debug.WriteLine(exception);
            }
        }

        private string GetId(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders
                                .Accept
                                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "relativeAddress");

                    var response = client.GetAsync("/id").Result;

                    var responseString = response.Content.ReadAsStringAsync().Result;
                    System.Diagnostics.Debug.WriteLine(responseString);

                    var resultObject = JObject.Parse(responseString);
                    var newId = resultObject.ToObject<NewId>();

                    return newId.newid;
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("CAUGHT EXCEPTION:");
                System.Diagnostics.Debug.WriteLine(exception);
            }

            return "";
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            PostRequest(Globals.serverUrl, textBox.Text);
            this.Frame.Navigate(typeof(WebView));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(OldNewMenu));
        }

    }
}
