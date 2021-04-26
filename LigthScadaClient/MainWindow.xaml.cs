using LigthScadaClient.DataModels;
using LigthScadaClient.Pages;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace LigthScadaClient
{
    public partial class MainWindow : Window
    {
        public static event Action DataLoaded;

        public StatusPage statusPage;
        public CoilRegistersPage coilRegistersPage;
        public DiscreteInputsPage discreteInputsPage;
        public InputRegistersPage inputRegistersPage;
        public HoldingRegistersPage holdingRegistersPage;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializePages()
        {
            statusPage = StatusPageFrame.Content as StatusPage;
            coilRegistersPage = CoilsFrame.Content as CoilRegistersPage;
            discreteInputsPage = DiscreteInputsFrame.Content as DiscreteInputsPage;
            inputRegistersPage = InputRegistersFrame.Content as InputRegistersPage;
            holdingRegistersPage = HoldingRegistersFrame.Content as HoldingRegistersPage;
            LoadData();
        }

        private void LoadData()
        {
            if (File.Exists("config.cfg"))
            {
                string json;
                using (StreamReader reader = new("config.cfg"))
                {
                    json = reader.ReadToEnd();
                }
                DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(json);
                coilRegistersPage.CoilRegisters = dataSet.CoilRegisters;
                discreteInputsPage.DiscreteInputs = dataSet.DiscreteInputs;
                inputRegistersPage.InputRegisters = dataSet.InputRegisters;
                holdingRegistersPage.HoldingRegisters = dataSet.HoldingRegisters;
                DataLoaded?.Invoke();
            }
        }

        private void SaveData()
        {
            DataSet dataSet = new DataSet
            {
                CoilRegisters = coilRegistersPage.CoilRegisters,
                DiscreteInputs = discreteInputsPage.DiscreteInputs,
                InputRegisters = inputRegistersPage.InputRegisters,
                HoldingRegisters = holdingRegistersPage.HoldingRegisters
            };
            string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
            File.WriteAllText("config.cfg", json);
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            SaveData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializePages();
        }
    }
}
