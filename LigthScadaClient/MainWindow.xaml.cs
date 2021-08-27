using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO.Ports;
using System.Linq;
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
        private bool m_isInitialized = false;

        public MainWindow()
        {
            InitializeComponent();
            m_logger = new StatusLogger(StatusTextBox);
            SetupComboBoxes();
            LoadSavedData();
        }

        private void SetupComboBoxes()
        {
            ComPortComboBox.ItemsSource = SerialPort.GetPortNames();
            BaudrateComboBox.ItemsSource = new List<int>() { 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 57600, 76800, 115200 };
            ParityComboBox.ItemsSource = Enum.GetValues(typeof(Parity)).Cast<Parity>();
            StopBitsComboBox.ItemsSource = Enum.GetValues(typeof(StopBits)).Cast<StopBits>();
            BaudrateComboBox.SelectedIndex = 0;
            ParityComboBox.SelectedIndex = 0;
            StopBitsComboBox.SelectedIndex = 0;
        }

        private void LoadSavedData()
        {
            ApiKeyTextBox.Text = LocalConfiguration.Instance.ApiKey;
            NameTextBox.Text = LocalConfiguration.Instance.Name;
            IPTextBox.Text = LocalConfiguration.Instance.IP;
            PortTextBox.Text = LocalConfiguration.Instance.TCPPort.ToString();
            SlaveIdTextBox.Text = LocalConfiguration.Instance.SlaveID.ToString();

            ParityComboBox.SelectedItem = LocalConfiguration.Instance.Parity;
            StopBitsComboBox.SelectedItem = LocalConfiguration.Instance.StopBits;
            BaudrateComboBox.SelectedItem = LocalConfiguration.Instance.Baudrate;
            if (ComPortComboBox.Items.Contains(LocalConfiguration.Instance.COMPort))
                ComPortComboBox.SelectedItem = LocalConfiguration.Instance.COMPort;

            m_isInitialized = true;
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
                SetConfigurationView();
            }
            else
                DisableConfigurationView();
        }

        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            if (m_isConfigDirty)
            {
                m_modbusCommunication?.Dispose();
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

        private void OnConfigTextChange(object sender, TextChangedEventArgs e)
        {
            if (m_isInitialized)
            {
                LocalConfiguration.Instance.Name = NameTextBox.Text;
                LocalConfiguration.Instance.ApiKey = ApiKeyTextBox.Text;
                LocalConfiguration.Instance.IP = IPTextBox.Text;
                LocalConfiguration.Instance.SlaveID = 1;
                LocalConfiguration.Instance.SlaveID = int.TryParse(SlaveIdTextBox.Text, out int ID) ? ID : 1;
                _ = int.TryParse(PortTextBox.Text, out LocalConfiguration.Instance.TCPPort);
            }
        }

        private void OnConfigurationSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if (m_isInitialized)
            {
                LocalConfiguration.Instance.COMPort = ComPortComboBox.Text;
                LocalConfiguration.Instance.Parity = ParityComboBox.SelectedIndex == -1 ? Parity.None : (Parity)ParityComboBox.SelectedItem;
                LocalConfiguration.Instance.StopBits = StopBitsComboBox.SelectedIndex == -1 ? StopBits.None : (StopBits)StopBitsComboBox.SelectedItem;
                LocalConfiguration.Instance.Baudrate = (int)BaudrateComboBox.SelectedItem;
            }
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
            //ToDo sprawdzac poprawnosc configu
            StartButton.IsEnabled = isStartEnabled;
            StopButton.IsEnabled = !isStartEnabled;
        }

        private void SetConfigurationView()
        {
            bool isTCP = LocalConfiguration.Instance.IsTCP;
            TCPConfigurationGrid.IsEnabled = isTCP;
            TCPConfigurationGrid.Visibility = isTCP ? Visibility.Visible : Visibility.Hidden;
            RTUConfigurationGrid.IsEnabled = !isTCP;
            RTUConfigurationGrid.Visibility = isTCP ? Visibility.Hidden : Visibility.Visible;
        }

        private void DisableConfigurationView()
        {
            TCPConfigurationGrid.IsEnabled = false;
            RTUConfigurationGrid.IsEnabled = false;
        }
    }
}

