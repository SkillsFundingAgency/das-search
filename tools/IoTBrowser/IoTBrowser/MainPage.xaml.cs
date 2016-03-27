// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IoTBrowser
{
    internal enum Pages
    {
        Status = 1, 
        BuildStatus = 2, 
        Sonarqube = 3
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string _webAddress;
        private DispatcherTimer _timer;
        private readonly IDictionary<Pages, string> _pageList;
        private Pages _currentPage;

        public MainPage()
        {
            this.InitializeComponent();

            _pageList = GeneratePageList();
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 5);
            _timer.Tick += _timer_Tick;
            webView.NavigationCompleted += WebView_NavigationCompleted;
            DoWebNavigate(Pages.Status);
        }

        private IDictionary<Pages, string> GeneratePageList()
        {
            var pages = new Dictionary<Pages, string>
            {
                [Pages.Status] = "http://sfa-das-status.azurewebsites.net/",
                [Pages.BuildStatus] = "http://sfa-das-status.azurewebsites.net/status/build",
                [Pages.Sonarqube] = "http://dassonar.westeurope.cloudapp.azure.com:10090/dashboard/index?id=SFA-DAS",
            };

            return pages;
        }

        private void _timer_Tick(object sender, object e)
        {
            var nextPageIndex = ((int)_currentPage % _pageList.Count) + 1;

            DoWebNavigate((Pages)nextPageIndex);
        }

        private void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            _timer.Start();
        }

        private void DoWebNavigate(Pages page)
        {
            _webAddress = _pageList[page];
            _currentPage = page;

            _timer.Stop();
            DismissMessage();

            try
            {
                if (!string.IsNullOrWhiteSpace(_webAddress))
                {
                    webView.Navigate(new Uri(_webAddress));
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

        private void Go_Status_Click(object sender, RoutedEventArgs e)
        {
            DoWebNavigate(Pages.Status);
        }

        private void Go_BuildStatus_Click(object sender, RoutedEventArgs e)
        {
            DoWebNavigate(Pages.BuildStatus);
        }

        private void Go_Sonarqube_Click(object sender, RoutedEventArgs e)
        {
            DoWebNavigate(Pages.Sonarqube);
        }

        private void DisplayMessage(String message)
        {
            Message.Text = message;
            MessageStackPanel.Visibility = Visibility.Visible;
            webView.Visibility = Visibility.Collapsed;

        }

        private void OnMessageDismiss_Click(object sender, RoutedEventArgs e)
        {
            DismissMessage();
        }

        private void DismissMessage()
        {
            webView.Visibility = Visibility.Visible;
            MessageStackPanel.Visibility = Visibility.Collapsed;
        }
    }
}
