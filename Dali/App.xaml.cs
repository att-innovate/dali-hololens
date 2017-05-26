
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Dali
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs launchEvent)
        {
            ReadGlobalDaliServerAddress();
            Type nextPage = ResolveNextPage(launchEvent);

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
                rootFrame = new Frame();

            rootFrame.Navigate(nextPage, null);
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        private static void ReadGlobalDaliServerAddress()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["serverUrl"];

            if (value == null)
                Globals.serverUrl = "";
            else
                Globals.serverUrl = (string)value;
        }

        private Type ResolveNextPage(LaunchActivatedEventArgs launchEvent)
        {
            System.Diagnostics.Debug.WriteLine("----- Start TileId: " + launchEvent.TileId);
            string tileId = launchEvent.TileId;
            Globals.tileId = tileId;

            if (!Globals.tileIdForFirstViewSet)
            {
                TryToSetTileIdForFirstView(tileId);
            }

            if (!Globals.selectedMarksOnView.ContainsKey(tileId))
            {
                Globals.selectedMarksOnView[tileId] = null;
                Globals.selectedMark = null;
                return typeof(Views.MainPage);
            }

            Mark selectedMarkOnView = Globals.selectedMarksOnView[tileId];
            if (selectedMarkOnView == null)
                return typeof(Views.MainPage);

            Globals.selectedMark = selectedMarkOnView;
            return typeof(Views.WebView);
        }

        async private void TryToSetTileIdForFirstView(string currentTileId)
        {
            //Needed because first time OnLaunched gets called
            //the tileId is set to "App", and the correct tileId isn't known at that time.
            //We will set it the next time OnLaunched gets called.
            string expectedTileId = null;

            var list = await SecondaryTile.FindAllAsync();
            if (list.Count == 1)
            {
                expectedTileId = currentTileId;
            }
            else
            {
                foreach (SecondaryTile liveTile in list)
                {
                    if (liveTile.TileId != currentTileId)
                        expectedTileId = liveTile.TileId;
                }
            }

            if (expectedTileId == null)
                return;

            Mark mark = Globals.selectedMarksOnView["App"];
            Globals.selectedMarksOnView[expectedTileId] = mark;
            Globals.selectedMarksOnView.Remove("App");

            Globals.tileIdForFirstViewSet = true;
        }
    }
}
