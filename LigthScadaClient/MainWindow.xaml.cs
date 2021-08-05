using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using DatabaseClasses;
using LigthScadaClient.Logic;

namespace LigthScadaClient
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private void RefreshButtonHandler(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ApiKeyTextBox.Text))
            {
                string apiKey = ApiKeyTextBox.Text;
                RefreshIcon.Visibility = Visibility.Visible;
                RefreshConfigsButton.IsEnabled = false;
                Task.Run(async () =>
               {
                   List<ClientConfigEntity> configs = null;
                   try
                   {
                       configs = await ServerCommunication.Instance.GetConfigurations(apiKey);
                   }
                   catch (Exception exception)
                   {
                       Dispatcher.Invoke(() => MessageBox.Show(exception.Message));
                   }

                   Dispatcher.Invoke(() =>
                   {
                       ConfigComboBox.ItemsSource = configs;
                       if (configs != null && configs.Count > 0)
                           ConfigComboBox.SelectedIndex = 0;
                       ConfigComboBox.IsEnabled = configs != null ? true : false;
                       RefreshIcon.Visibility = Visibility.Hidden;
                       RefreshConfigsButton.IsEnabled = true;
                   });
               });
            }
        }
    }
}

