﻿using System;
using System.IO;
using System.Net;
using Windows.Web.Http.Headers;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Dali
{ 
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Message();
            WebBrowsing();
            // GetRequest("http://127.0.0.1:8085/mark");
            //PostRequest("http://10.250.3.24:8085/mark/");//"http://127.0.0.1:8085/ping");
        }

        public static void Message()
        {
            var dlg = new MessageDialog("Would you like to create a new mark or view a previously created mark?");
            dlg.Commands.Add(new UICommand("Old", delegate (IUICommand command)
            {
                //if "old" is picked, prompt the user with another message dialog with old mark options
                var dlgOld = new MessageDialog("Which mark would you like to view here?");
                var result = dlgOld.ShowAsync();

            }));

            dlg.Commands.Add(new UICommand("New", null));

            var op = dlg.ShowAsync();
        }

        private static void WebBrowsing()
        {
            WebBrowser wb = new WebBrowser()

        }
        async static void GetRequest(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                //get response
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        //string mycontent = await content.ReadAsStringAsync();
                        //HttpContentHeaders headers = content.Headers;
                        // Debug.WriteLine(headers);
                        var responseString = client.GetStringAsync(url);
                        System.Diagnostics.Debug.Write(responseString);

                    }
                }
            }
        }

        async static void PostRequest(string url, string newLabel)
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
                    var tempString = "{\"label\":\"#replace#\"}";
                    var result = tempString.Replace("#replace#", newLabel);
                    System.Diagnostics.Debug.Write(result);
                    request.Content = new StringContent(result,
                                                        Encoding.UTF8,
                                                        "application/json");//CONTENT-TYPE header

                    var response = client.PostAsync("/mark", request.Content).Result;
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    System.Diagnostics.Debug.Write(responseString);
                    System.Diagnostics.Debug.WriteLine("SUCCESS:");
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("CAUGHT EXCEPTION:");
                System.Diagnostics.Debug.WriteLine(exception);
            }
        }


        /*    private void alert()
            {
                try
                {
                    // do stuff
                }
                catch (Exception ex)
                {
                    this.ShowAlert("Alert");
                }
            }*/

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*FILL*/
            // GetRequest("http://127.0.0.1:8085/mark");

        }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            PostRequest("http://10.250.3.24:8085", textBox.Text);
        }
    }
}