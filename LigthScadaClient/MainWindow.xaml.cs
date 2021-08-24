using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DatabaseClasses;
using LigthScadaClient.Logic;

namespace LigthScadaClient
{
    public partial class MainWindow : Window
    {
        private StatusLogger m_logger;
        private ModbusCommunication m_modbusCommunication;
        private bool m_isConfigDirty = true;

        public MainWindow()
        {
            InitializeComponent();
            m_logger = new StatusLogger(StatusTextBox);
        }

        private void UpdateConfigsBox(List<ClientConfigEntity> configs)
        {
            ConfigComboBox.ItemsSource = configs;
            if (configs != null && configs.Count > 0)
                ConfigComboBox.SelectedIndex = 0;
            else
                ConfigComboBox.SelectedIndex = -1;
            ConfigComboBox.IsEnabled = configs != null ? true : false;
            RefreshIcon.Visibility = Visibility.Hidden;
            RefreshConfigsButton.IsEnabled = true;
            m_logger.Log("Configs Updated");
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
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
                        LocalConfiguration.Instance.SetApiKey(apiKey);
                    }
                    catch (Exception exception)
                    {
                        Dispatcher.Invoke(() => MessageBox.Show(exception.Message));
                    }
                    Dispatcher.Invoke(() => UpdateConfigsBox(configs));
                });
            }
        }

        private void OnConfigSelection(object sender, SelectionChangedEventArgs e)
        {
            if (ConfigComboBox.SelectedIndex != -1)
            {
                LocalConfiguration.Instance.SetConfiguration(ConfigComboBox.SelectedItem as ClientConfigEntity);
                StartButton.IsEnabled = true;
                m_isConfigDirty = true;
            }
        }

        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            if (m_isConfigDirty)
            {
                if (m_modbusCommunication != null)
                    m_modbusCommunication.Dispose();
                m_modbusCommunication = new ModbusCommunication();
                m_isConfigDirty = false;
            }
            m_modbusCommunication.Start();
            SwitchButtonStates(false);
        }

        private void OnStopButtonClick(object sender, RoutedEventArgs e)
        {
            m_modbusCommunication.Stop();
            SwitchButtonStates(true);
        }

        private void SwitchButtonStates(bool isStartEnabled)
        {
            StartButton.IsEnabled = isStartEnabled;
            StopButton.IsEnabled = !isStartEnabled;
        }
    }
}

