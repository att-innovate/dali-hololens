using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Windows.Web.Http.Headers;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Dali.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public static class Globals
    {
        // public static String getResponseString = "";
        public static String newId = "";
        public static List<Mark> marks;
        public static Mark selectedMark;
    }

    public sealed partial class OldMarks : Page
    {
        public OldMarks()
        {
            this.InitializeComponent();
            GetRequest("http://10.250.3.24:8085");
        }

        public async void GetRequest(string url)
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

                    using (var response = client.GetAsync("/mark").Result)
                    {

                        string responseString = response.Content.ReadAsStringAsync().Result;
                        System.Diagnostics.Debug.WriteLine("GET SUCCESS:");
                        System.Diagnostics.Debug.WriteLine(responseString);


                        getMarks(responseString);

                        this.listView.ItemsSource = getLabels();
                    }
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("CAUGHT EXCEPTION:");
                System.Diagnostics.Debug.WriteLine(exception);
            }
        }

        private static void getMarks(string responseString)
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


        // recursively yield all children of json
        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }

        //parsing string from http request to extract the labels
        private static List<string> getLabels()
        {
            List<string> labels = new List<string>();

            var marks = Globals.marks;
            for (int i = 0; i < marks.Count; i++)
            {
                //check if this label has already been added to labels array 
                if (labels.Contains(marks[i].label) == false)
                {
                    labels.Add(marks[i].label);
                }
            }
            return labels;
        }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void textBlock_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //populate mark because c# only lets you pass one parameter between frames
            Mark currentMark = new Mark();

            //find mark associated with this label
            var currlabel = this.listView.SelectedItem.ToString();
            var marks = Globals.marks;
            for (int i = 0; i < marks.Count; i++)
            {
                if (marks[i].label == currlabel)
                {
                    currentMark.id = marks[i].id;
                    currentMark.label = marks[i].label;
                    currentMark.type = marks[i].type;
                    currentMark.content = marks[i].content;
                }
            }
            Globals.selectedMark = currentMark;
            // this.Frame.Navigate(typeof(ConfigureMark), mark);
            this.Frame.Navigate(typeof(webView));
        }
    }
}
