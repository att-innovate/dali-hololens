//using Dali.Common;
using Dali.ViewModels;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Dali.Views
{
    public sealed partial class MasterDetailPage : Page
    {
        private MainViewModel ViewModel;
        private bool isCurrentFeedNew = false;

        public MasterDetailPage()
        {

            /*  this.InitializeComponent();
              System.Diagnostics.Debug.WriteLine("MASTER DETAIL PAGE");


              ViewModel.PropertyChanged += ViewModel_PropertyChanged;

              ArticleWebView.NavigationStarting += async (s, e) =>
              {
                  if (!await ViewModel.CurrentArticle.Link.LaunchBrowserForNonMatchingUriAsync(e))
                  {
                      // In-app navigation, so hide the WebView control and display the progress 
                      // animation until the page load is completed.
                      ArticleWebView.Visibility = Visibility.Collapsed;
                      LoadingProgressBar.Visibility = Visibility.Visible;
                  }
              };

              ArticleWebView.LoadCompleted += (s, e) =>
              {
                  ArticleWebView.Visibility = Visibility.Visible;
                  LoadingProgressBar.Visibility = Visibility.Collapsed;
              };
          }

          private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
          {
              // Set a flag so that, in narrow mode, details-only navigation doesn't occur if 
              // the CurrentArticle is changed solely as a side-effect of changing the CurrentFeed.
              if (e.PropertyName == nameof(ViewModel.CurrentFeed)) isCurrentFeedNew = true;
              else if (e.PropertyName == nameof(ViewModel.CurrentArticle))
              {
                  if (ViewModel.CurrentArticle != null)
                  {
                      ArticleWebView.Navigate(ViewModel.CurrentArticle.Link);
                  }
                  else
                  {
                      ArticleWebView.NavigateToString(string.Empty);
                  }

                  if (AdaptiveStates.CurrentState == NarrowState)
                  {
                      bool switchToDetailsView = !isCurrentFeedNew;
                      isCurrentFeedNew = false;
                      if (switchToDetailsView)
                      {
                          // Use "drill in" transition for navigating from master list to detail view
                          Frame.Navigate(typeof(DetailPage), null, new DrillInNavigationTransitionInfo());
                      }
                  }
              }
          }

          protected override async void OnNavigatedTo(NavigationEventArgs e)
          {
              base.OnNavigatedTo(e);

              if (e.Parameter is Uri || e.Parameter == null ||
                  (e.Parameter is string && String.IsNullOrEmpty(e.Parameter as string)))
              {
                  if (MasterFrame.CurrentSourcePageType != typeof(FeedView))
                  {
                      MasterFrame.Navigate(typeof(FeedView));
                  }

                  var feedUri = e.Parameter as Uri;
                  if (feedUri != null)
                  {
                      var feed = ViewModel.Feeds.FirstOrDefault(f => f.Link == feedUri);
                      ViewModel.CurrentFeed = feed;
                      await feed.RefreshAsync();
                  }
              }
              else
              {
                  var viewType = e.Parameter as Type;

                  if (viewType != null && MasterFrame.CurrentSourcePageType != viewType)
                  {
                      MasterFrame.Navigate(viewType);
                  }

                  UpdateForVisualState(AdaptiveStates.CurrentState);
              }
          }

          private void AdaptiveStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
          {
              UpdateForVisualState(e.NewState, e.OldState);
          }

          private void UpdateForVisualState(VisualState newState, VisualState oldState = null)
          {
              var isNarrow = newState == NarrowState;

              if (isNarrow && oldState == DefaultState)
              {
                  // Resize down to the detail item. Don't play a transition.
                  Frame.Navigate(typeof(DetailPage), null, new SuppressNavigationTransitionInfo());
              }
          }
          */
        }
    }
}
