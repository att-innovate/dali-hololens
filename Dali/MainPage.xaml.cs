using System;
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
    /// 

    public static class Globals
    {
        public static String label = ""; // Modifiable in Code
        public static String note = ""; // Modifiable in Code
    }

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            // WebBrowsing();
            GetRequest("http://10.250.3.24:8085/mark");
            this.InitializeComponent();

        }

        private static void Message(string[] labels, string responseString)
        {
            MessageDialog dlg = new MessageDialog("Would you like to create a new mark or view a previously created mark?");
            dlg.Commands.Add(new UICommand("Old", delegate (IUICommand command)
            {
                //if "old" is picked, prompt the user with another message dialog with old mark options

                System.Diagnostics.Debug.WriteLine(labels.Length);

                MessageDialog dlgOld = new MessageDialog("Which mark would you like to view here?");
                dlgOld.Commands.Add(new UICommand(labels[0], delegate (IUICommand command2)
                {
                    Globals.label = labels[0];
                    //get corresponding note
                    var note = getNote(labels[0], responseString);
                    Globals.note = note;
                    System.Diagnostics.Debug.WriteLine("Press the Done Button.");

                    //configure view
                    // TextBlock textBlock = new TextBlock();
                    //textBlock.Text = getNote(labels[0], responseString);
                    //TextBox textBox = new TextBox();
                    //textBox.Text = labels[0];
                    //Controls.Add
                }));
               
              /*   for (int i = 0; i < labels.Length; i++)
                {
                    dlgOld.Commands.Add(new UICommand("Hello", null));
                }*/
                dlgOld.CancelCommandIndex = 0;
                var result = dlgOld.ShowAsync();

               /* if ((int)result.Id == 0)

                 {
                    System.Diagnostics.Debug.WriteLine("added");
                    var note = getNote(labels[0], responseString);
                    System.Diagnostics.Debug.WriteLine(note);
                }*/

                //create a button for each old mark
                // for (int i = 0; i < labels.Length; i++)
                //{

                // System.Diagnostics.Debug.WriteLine(labels[1]);
                //  dlgOld.Commands.Add(new UICommand(labels[1], new UICommandInvokedHandler(CommandInvokedHandler))); //delegate (IUICommand command2)
                // {
                //    System.Diagnostics.Debug.WriteLine(labels[1]);
                //  }));
                // var result = dlgOld.ShowAsync();

                // dlgOld.Commands.Add(new Windows.UI.Popups.UICommand("Maybe later"), null);

                // }

            }));

            dlg.Commands.Add(new UICommand("New", null));

            var op = dlg.ShowAsync();
        }

        private static string getNote(string label, string responseString)
        {
            string[] stringSeparators = new string[] { label + ",\"Note\":", "}," };
            string[] segments;
            segments = responseString.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            var note = segments[1];
            return note;
        }

        // private static void WebBrowsing()
        // {
        //  WebBrowser wb = new WebBrowser()

        async static void GetRequest(string url)
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

                    var response = client.GetAsync("/mark").Result;

                    /*
                                        seekLocation = 0;
                                        IBuffer buffer;
                                        using (IRandomAccessStream irandomAccessStream = await storageFileIn.OpenAsync(FileAccessMode.Read))
                                        {
                                            IInputStream inputStream = irandomAccessStream.GetInputStreamAt(seekLocation);
                                            using (DataReader dataReader = new DataReader(inputStream))
                                            {
                                                await dataReader.LoadAsync( (uint)irandomAccessStream.Size );

                                                uint length = dataReader.ReadUInt32();
                                                buffer = dataReader.ReadBuffer(length);

                                                seekLocation += ((ulong)e.FrameBuffer.Length + 4);
                                            }
                                        }
                    #endregion
                    */

                    //  var responseStream = response.Content.ReadAsStreamAsync().Result;
                    // var result = streamToString(responseStream);
                    // System.Diagnostics.Debug.WriteLine(result);

                    var responseString = response.Content.ReadAsStringAsync().Result;
                    System.Diagnostics.Debug.WriteLine(responseString);
                    System.Diagnostics.Debug.WriteLine("GET SUCCESS:");

                    string[] labels = getLabels(responseString);

                   // Message(labels, responseString);
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("CAUGHT EXCEPTION:");
                System.Diagnostics.Debug.WriteLine(exception);
            }
        }

        //parsing string from http request to extract the labels
        private static string[] getLabels(String responseString)
        {
            string[] stringSeparators = new string[] { "\"Label\":", ",\"Note\":" };
            List<string> labels = new List<string>();
            string[] labelsArray, segments;
            segments = responseString.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < segments.Length; i = i + 2)
            {
                //check if this label has already been added to labels array 
                if (labels.Contains(segments[i]) == false)
                {
                    labels.Add(segments[i]);
                    System.Diagnostics.Debug.WriteLine(segments[i]);

                }
            }
            labelsArray = labels.ToArray();
            return labelsArray;
        }

        /*  private static Array streamToString(Stream responseStream)
          {
              var list = new List<string>();
              using (var sr = new StreamReader(responseStream))
              {
                  string line;
                  while ((line = sr.ReadLine()) != null)
                  {
                      list.Add(line);
                      System.Diagnostics.Debug.WriteLine("Stream line");
                      System.Diagnostics.Debug.WriteLine(line);
                  }
              }
              string[] result = list.ToArray();
              return result;
          }
          */

       /* async static void PostRequest(string url, string newLabel)
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
                    System.Diagnostics.Debug.WriteLine("POST SUCCESS:");
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("CAUGHT EXCEPTION:");
                System.Diagnostics.Debug.WriteLine(exception);
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
           
/*
            if (textBox.Text != "")
            {
                 PostRequest("http://10.250.3.24:8085", textBox.Text);
            } else
            {
                textBlock.Text = Globals.note;
                textBox.Text = Globals.label;
            }*/
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage1));

        }

        private void oldButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage2));
        }
    }
}