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


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Dali
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>


    public class Mark
    {
        public string id { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public string[] content { get; set; }
    }

        public sealed partial class SetNewMark : Page
    {
        public SetNewMark()
        {
            this.InitializeComponent();
        }

        async static void PostRequest(string url, string newLabel)
        {
            getId(url);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                
                    var mark = new Mark();
                    mark.id = Globals.newId;
                    mark.label = newLabel;
                    mark.type = "";
                    mark.content = new string[] {};

                    //serialize struct into a JSON String
                    var json = JsonConvert.SerializeObject(mark);
                    System.Diagnostics.Debug.WriteLine("JSON", json);

                    // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                    //  Product deserializedProduct = JsonConvert.DeserializeObject<Product>(json);

                    // Do the actual request and await the response
                    var httpResponse = await client.PostAsync("/mark/", httpContent);

                    // If the response contains content we want to read it!
                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(responseContent);

                        System.Diagnostics.Debug.WriteLine("POST SUCCESS:");
                    }

                    /// var response = client.PostAsync("/mark", request.Content).Result;
                    //var responseString = response.Content.ReadAsStringAsync().Result;
                    // System.Diagnostics.Debug.WriteLine(responseString);
                    //System.Diagnostics.Debug.WriteLine("POST SUCCESS:");
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("CAUGHT EXCEPTION:");
                System.Diagnostics.Debug.WriteLine(exception);
            }
        }

        async static void getId(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders
                                .Accept
                                .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "relativeAddress");

                    var response = client.GetAsync("/id/").Result;

                    var responseString = response.Content.ReadAsStringAsync().Result;
                    System.Diagnostics.Debug.WriteLine("GET SUCCESS:");

                    var id = extractIdFromString(responseString);
                    Globals.newId = id;
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("CAUGHT EXCEPTION:");
                System.Diagnostics.Debug.WriteLine(exception);
            }
        }

        //parsing string from http request to extract the labels
        private static string extractIdFromString(String responseString)
        {
            string[] stringSeparators = new string[] { "\"message\":", "}" };
            string[] segments;
            segments = responseString.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

            var id = segments[1].Remove(segments[1].Length - 2);
            id = id.Remove(0, 1);

            return id;
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            PostRequest("http://10.250.3.24:8085", textBox.Text);
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}
