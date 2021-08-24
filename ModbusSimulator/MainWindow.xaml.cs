using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EasyModbus;

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
            m_logger = new StatusLogger(StatusTextBox);
        }

        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            var rand = new Random(DateTime.Now.Millisecond);
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            m_server = new ModbusServer();
            m_server.LogFileFilename = "log.txt";
            for (int i = 0; i < 100; i++)
            {
                m_server.coils[i] = rand.Next(0, 10) > 5 ? false : true;
                m_server.discreteInputs[i] = rand.Next(0, 5) > 5 ? false : true;
                m_server.holdingRegisters[i] = (short)rand.Next(0, 10000);
                m_server.inputRegisters[i] = (short)rand.Next(0, 10000);
            }
            m_server.SerialPort = PortsComboBox.SelectedItem as string;
            m_server.Listen();
            m_logger.Log("Server started!");
            m_server.NumberOfConnectedClientsChanged += () => m_logger.Log("Clients connected : " + m_server.NumberOfConnections);
        }

        private void OnStopButtonClick(object sender, RoutedEventArgs e)
        {
            m_server.StopListening();
            m_logger.Log("Server stopped");
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }
    }
}
