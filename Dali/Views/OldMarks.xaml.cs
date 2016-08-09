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



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

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
    }

    public sealed partial class OldMarks : Page
    {
        public OldMarks()
        {
            this.InitializeComponent();
            GetRequest("http://10.250.3.24:8085");
        }

        async void GetRequest(string url)
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

                    using (var response = client.GetAsync("/mark/").Result)
                    {

                        string responseString = response.Content.ReadAsStringAsync().Result;
                        System.Diagnostics.Debug.WriteLine("GET SUCCESS:");
                        System.Diagnostics.Debug.WriteLine(responseString);


                        this.listView.ItemsSource = getLabels(responseString);

                        JObject o = JObject.Parse(responseString);
                        System.Diagnostics.Debug.WriteLine("DeserializedJson:", o.ToString());


                        //    getMarks(responseString);
                        // dynamic mList = JsonConvert.DeserializeObject<Dictionary<string,Mark[]>>(responseString);


                        // var results = JsonConvert.DeserializeObject<List<Mark>>(responseString);
                        //System.Diagnostics.Debug.WriteLine("DeserializedJson:", mList);

                        /*                        for (int i = 0; i < results.Count; i++)
                                                {
                                                    var markId = results[i].id;
                                                    var markLabel = results[i].label;
                                                    System.Diagnostics.Debug.WriteLine(markId);
                                                    System.Diagnostics.Debug.WriteLine(markLabel);

                                                } */


                        /* if (response.IsSuccessStatusCode)
                         {
                             var result = JsonConvert.DeserializeObject<Mark>(response.Content.ReadAsStringAsync().Result);
                             System.Diagnostics.Debug.WriteLine("HELO");

                         }*/

                    }

                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("CAUGHT EXCEPTION:");
                System.Diagnostics.Debug.WriteLine(exception);
            }
        }

        //parsing string from http request to extract the labels
        private static List<string> getLabels(String responseString)
        {
            string[] stringSeparators = new string[] { "\"Label\":", ",\"Type\":" };
            List<string> labels = new List<string>();
            string[] segments;
            segments = responseString.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < segments.Length; i = i + 2)
            {
                //check if this label has already been added to labels array 
                if (labels.Contains(segments[i]) == false)
                {
                    labels.Add(segments[i]);
                }
            }
            return labels;
        }

        private static string getNote(string label, string responseString)
        {
            string[] stringSeparators = new string[] { /*label +*/ ",\"Note\":", "}," };
            string[] segments;
            segments = responseString.Split(stringSeparators, StringSplitOptions.None);
            var note = "";
            for (int i = 0; i < segments.Length; i++)
            {
                if (segments[i].Contains(label))
                {
                    note = segments[i + 1];
                }
            }
            return note;
        }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void textBlock_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //string note = getNote(this.listView.SelectedItem.ToString(), Globals.responseString);

            //populate mark because c# only lets you pass one parameter between frames
            Mark mark = new Mark();
            mark.label = this.listView.SelectedItem.ToString();
            mark.id = "";

            // this.Frame.Navigate(typeof(ConfigureMark), mark);
            this.Frame.Navigate(typeof(webView), mark);

        }
    }
}
