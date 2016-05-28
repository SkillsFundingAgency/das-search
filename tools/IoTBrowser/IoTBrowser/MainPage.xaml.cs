// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IoTBrowser
{
    public enum Side
    {
        Left,
        Right
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private IDictionary<Side, IList<string>> _viewPages;
        private DispatcherTimer _timer;
        private int _currentPageIndexLeft = 0;
        private int _currentPageIndexRight = 0;
        private bool _leftNavigating = false;
        private bool _rightNavigating = false;
        private bool _paused = false;

        public MainPage()
        {
            InitializeComponent();

            webViewLeft.NavigationCompleted += WebViewLeft_NavigationCompleted;
            webViewRight.NavigationCompleted += WebViewRight_NavigationCompleted;
            _viewPages = GeneratePagesList();
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 20);
            _timer.Tick += _timer_Tick;

            NavigateViews(0, 0);
            ShowIpInformation();

            _timer.Start();
        }

        private void NavigateViews(int leftIndex, int rightIndex)
        {
            _leftNavigating = true;
            _rightNavigating = true;
            DoWebNavigate(leftIndex, _viewPages[Side.Left], webViewLeft);
            DoWebNavigate(rightIndex, _viewPages[Side.Right], webViewRight);
        }

        private void WebViewLeft_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            _leftNavigating = false;

            TryAndStartTimer();
        }

        private void WebViewRight_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            _rightNavigating = false;

            TryAndStartTimer();
        }

        private void TryAndStartTimer()
        {
            if (_paused == false && _leftNavigating == false && _rightNavigating == false)
            {
                _timer.Start();
            }
        }

        private void ShowIpInformation()
        {
            var hosts = NetworkInformation.GetHostNames();
            var ipInformation = hosts.Where(x => x.Type == Windows.Networking.HostNameType.Ipv4).Select(i => i.DisplayName).ToArray();
            var ipToDisplay = string.Join(",", ipInformation);

            IpAddress.Text = $"IP Address: {ipToDisplay}";
        }

        private IDictionary<Side, IList<string>> GeneratePagesList()
        {
            var pagesList = new Dictionary<Side, IList<string>>();

            var leftPages = new List<string>
            {
                "http://sfa-das-status.azurewebsites.net/",
                "http://dassonar.westeurope.cloudapp.azure.com:10090/dashboard/index?id=SFA-DAS",
            };

            var rightPages = new List<string>
            {
                "https://rally1.rallydev.com/#/49818572937d/reports"
            };

            pagesList.Add(Side.Left, leftPages);
            pagesList.Add(Side.Right, rightPages);

            return pagesList;
        }

        private void _timer_Tick(object sender, object e)
        {
            _currentPageIndexLeft = _currentPageIndexLeft < _viewPages[Side.Left].Count - 1 ? _currentPageIndexLeft + 1 : 0;
            _currentPageIndexRight = _currentPageIndexRight < _viewPages[Side.Right].Count - 1 ? _currentPageIndexRight + 1 : 0;

            _timer.Stop();

            NavigateViews(_currentPageIndexLeft, _currentPageIndexRight);
        }

        private void DoWebNavigate(int index, IList<string> pages, WebView view)
        {
            var webAddress = pages[index];

            try
            {
                if (!string.IsNullOrWhiteSpace(webAddress))
                {
                    view.Navigate(new Uri(webAddress));
                }
                else
                {
                    DisplayMessage("You need to enter a web address.");
                }
            }
            catch (Exception e)
            {
                DisplayMessage("Error: " + e.Message);
            }
        }

        private void DisplayMessage(String message)
        {
            Message.Text = message;
            MessageStackPanel.Visibility = Visibility.Visible;
            webViewLeft.Visibility = Visibility.Collapsed;

        }

        private void OnMessageDismiss_Click(object sender, RoutedEventArgs e)
        {
            DismissMessage();
        }

        private void DismissMessage()
        {
            webViewLeft.Visibility = Visibility.Visible;
            MessageStackPanel.Visibility = Visibility.Collapsed;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_timer.IsEnabled)
            {
                _paused = true;
                _timer.Stop();
                PauseButton.Content = "Resume";
            }
            else
            {
                _timer.Start();
                PauseButton.Content = "Pause";
                _paused = false;
            }
        }
    }
}
