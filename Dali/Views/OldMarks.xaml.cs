
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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http.Headers;

namespace Dali.Views
{
    public sealed partial class OldMarks : Page
    {
        public OldMarks()
        {
            this.InitializeComponent();
            LoadAndRefresh();
        }

        private void LoadAndRefresh()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Globals.serverUrl);
                    client.DefaultRequestHeaders
                                .Accept
                                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "relativeAddress");

                    using (var response = client.GetAsync("/mark/").Result)
                    {

                        string responseString = response.Content.ReadAsStringAsync().Result;
                        System.Diagnostics.Debug.WriteLine(responseString);

                        GetMarks(responseString);

                        this.listView.ItemsSource = GetLabels();
                    }
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("CAUGHT EXCEPTION:");
                System.Diagnostics.Debug.WriteLine(exception);
            }
        }

        private void GetMarks(string responseString)
        {
            List<Mark> marks = new List<Mark>();

            var resultObjects = AllChildren(JObject.Parse(responseString))
                                 .First(c => c.Type == JTokenType.Array && c.Path.Contains("marks"))
                                 .Children<JObject>();

            foreach (JObject result in resultObjects)
            {
                var newMark = result.ToObject<Mark>();
                marks.Add(newMark);
            }
            Globals.marks = marks;
        }


        private IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var child in json.Children())
            {
                yield return child;
                foreach (var myChild in AllChildren(child))
                {
                    yield return myChild;
                }
            }
        }

        private List<string> GetLabels()
        {
            List<string> labels = new List<string>();

            var marks = Globals.marks;
            for (int i = 0; i < marks.Count; i++)
            {
                if (labels.Contains(marks[i].label) == false)
                {
                    labels.Add(marks[i].label);
                }
            }
            return labels;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Mark currentMark = new Mark();

            var selectedLabel = this.listView.SelectedItem.ToString();
            var marks = Globals.marks;
            for (int i = 0; i < marks.Count; i++)
            {
                if (marks[i].label == selectedLabel)
                {
                    currentMark.id = marks[i].id;
                    currentMark.label = marks[i].label;
                    currentMark.type = marks[i].type;
                }
            }
            Globals.selectedMark = currentMark;
            Globals.selectedMarksOnView[Globals.tileId] = currentMark;
            this.Frame.Navigate(typeof(WebView));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(OldNewMenu));
        }
    }
}
