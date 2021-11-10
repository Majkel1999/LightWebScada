using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ScadaCommon;
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
            DataContext = LocalConfiguration.Instance;
            SetupComboBoxes();
        }

        private void SetupComboBoxes()
        {
            ComPortComboBox.ItemsSource = SerialPort.GetPortNames();
            BaudrateComboBox.ItemsSource = new List<int>() { 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 57600, 76800, 115200 };
            ParityComboBox.ItemsSource = Enum.GetValues(typeof(Parity)).Cast<Parity>();
            StopBitsComboBox.ItemsSource = Enum.GetValues(typeof(StopBits)).Cast<StopBits>();
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(LocalConfiguration.Instance.ApiKey))
            {
                RefreshIcon.Visibility = Visibility.Visible;
                RefreshConfigsButton.IsEnabled = false;
                Task.Run(async () =>
                {
                    List<ClientConfigEntity> configs = null;
                    try
                    {
                        configs = await ServerCommunication.Instance.GetConfigurations(LocalConfiguration.Instance.ApiKey);
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
            string error = LocalConfiguration.Instance.IsConfigurationCorrect();
            if (error != null)
            {
                MessageBox.Show(error);
                return;
            }

            if (m_isConfigDirty)
            {
                m_modbusCommunication = new ModbusCommunication();
                m_isConfigDirty = false;
            }
            Task.Run(() =>
            {
                try
                {
                    if (m_modbusCommunication.Start())
                        Dispatcher.Invoke(() => SwitchButtonStates(false));
                }
                catch (Exception e)
                {
                    StatusLogger.Instance.Log(e.Message);
                }
            });
        }

        private void OnStopButtonClick(object sender, RoutedEventArgs e)
        {
            WorkingIndicator.IsActive = true;
            StopButton.IsEnabled = false;
            m_modbusCommunication.Stop(() => Dispatcher.Invoke(() =>
            {
                StartButton.IsEnabled = true;
                WorkingIndicator.IsActive = false;
            }));
        }

        private void OnWindowClose(object sender, EventArgs e)
        {
            LocalConfiguration.Instance.SaveConfiguration();
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

        private void SwitchButtonStates(bool isStartEnabled)
        {
            StartButton.IsEnabled = isStartEnabled;
            StopButton.IsEnabled = !isStartEnabled;
        }
    }
}

