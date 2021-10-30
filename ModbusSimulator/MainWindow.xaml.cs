using System;
using System.IO.Ports;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using EasyModbus;
using ScadaCommon;

namespace ModbusSimulator
{
    public partial class MainWindow : Window
    {
        private ModbusServer m_server;
        private StatusLogger m_logger;

        public MainWindow()
        {
            InitializeComponent();
            PortsComboBox.ItemsSource = SerialPort.GetPortNames();
            if (PortsComboBox.Items.Count <= 0)
                StartButton.IsEnabled = false;
            else
                PortsComboBox.SelectedIndex = 0;
            ProtocolComboBox.Items.Add("ASCII/RTU");
            ProtocolComboBox.Items.Add("TCP/IP");
            ProtocolComboBox.SelectedIndex = 0;
            m_logger = new StatusLogger(StatusTextBox);
        }

        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            m_server = new ModbusServer();
            if (ProtocolComboBox.SelectedItem.ToString() == "ASCII/RTU")
            {
                string comPort = PortsComboBox.SelectedItem as string;
                if (!SerialPortsHelper.CheckIfPortIsOpen(comPort))
                {
                    MessageBox.Show($"{comPort} is already open!");
                    return;
                }
                m_server.SerialPort = comPort;
                m_server.SerialFlag = true;
                Dispatcher.Invoke(() => m_logger.Log($"Server started on port {m_server.SerialPort}!"));
            }
            else
            {
                m_server.SerialFlag = false;
                Dispatcher.Invoke(() => m_logger.Log($"Server started on {m_server.LocalIPAddress}:{m_server.Port}!"));
            }

            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            PortsComboBox.IsEnabled = false;
            ProtocolComboBox.IsEnabled = false;

            CoilRegistersTable.ItemsSource = m_server.coils.localArray;
            DiscreteInputsTable.ItemsSource = m_server.discreteInputs.localArray;
            HoldingRegistersTable.ItemsSource = m_server.holdingRegisters.localArray;
            InputRegistersTable.ItemsSource = m_server.inputRegisters.localArray;

            m_server.Listen();


            RandomizeValues();
        }

        private void OnStopButtonClick(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            PortsComboBox.IsEnabled = true;
            ProtocolComboBox.IsEnabled = true;

            CoilRegistersTable.ItemsSource = null;
            DiscreteInputsTable.ItemsSource = null;
            HoldingRegistersTable.ItemsSource = null;
            InputRegistersTable.ItemsSource = null;

            m_server.StopListening();
            m_server = null;

            Dispatcher.Invoke(() => m_logger.Log("Server stopped"));
        }

        private void RandomizeValues()
        {
            if (m_server == null)
                return;
            var rand = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 100; i++)
            {
                m_server.coils[i] = rand.Next(0, 10) > 5 ? false : true;
                m_server.discreteInputs[i] = rand.Next(0, 10) > 5 ? false : true;
                m_server.holdingRegisters[i] = (short)rand.Next(0, 10000);
                m_server.inputRegisters[i] = (short)rand.Next(0, 10000);
            }

            Dispatcher.Invoke(() =>
            {
                RefreshDisplayedValues();
                m_logger.Log("Values randomized");
            });

            Task.Run(() =>
            {
                Thread.Sleep(1000);
                RandomizeValues();
            });
        }

        private void RefreshDisplayedValues()
        {
            CoilRegistersTable.Items.Refresh();
            DiscreteInputsTable.Items.Refresh();
            HoldingRegistersTable.Items.Refresh();
            InputRegistersTable.Items.Refresh();
        }

        private void SetRowIndex(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }
    }
}
