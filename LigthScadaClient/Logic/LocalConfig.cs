using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using LigthScadaClient.DataModels;
using Newtonsoft.Json;

namespace LigthScadaClient.Logic
{
    public static class LocalConfig
    {
        public static event Action DataLoaded;

        public static ObservableCollection<DiscreteRegister> CoilRegisters;
        public static ObservableCollection<DiscreteRegister> DiscreteInputs;
        public static ObservableCollection<ValueRegister> InputRegisters;
        public static ObservableCollection<ValueRegister> HoldingRegisters;

        private static bool hasConfigChanged = false;

        static LocalConfig()
        {
            if (!LoadData())
            {
                CoilRegisters = new ObservableCollection<DiscreteRegister>();
                DiscreteInputs = new ObservableCollection<DiscreteRegister>();
                InputRegisters = new ObservableCollection<ValueRegister>();
                HoldingRegisters = new ObservableCollection<ValueRegister>();
            }
            CoilRegisters.CollectionChanged += ConfigChanged;
            DiscreteInputs.CollectionChanged += ConfigChanged;
            InputRegisters.CollectionChanged += ConfigChanged;
            HoldingRegisters.CollectionChanged += ConfigChanged;

        }

        private static void ConfigChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            hasConfigChanged = true;
            CoilRegisters.CollectionChanged -= ConfigChanged;
            DiscreteInputs.CollectionChanged -= ConfigChanged;
            InputRegisters.CollectionChanged -= ConfigChanged;
            HoldingRegisters.CollectionChanged -= ConfigChanged;
        }

        public static bool SaveData()
        {
            if (hasConfigChanged)
            {
                var result = MessageBox.Show(Application.Current.MainWindow, Application.Current.Resources.MergedDictionaries[0]["saveFileDialog"].ToString(), "Save config", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    DataSet dataSet = new DataSet
                    {
                        CoilRegisters = new List<DiscreteRegister>(LocalConfig.CoilRegisters),
                        DiscreteInputs = new List<DiscreteRegister>(LocalConfig.DiscreteInputs),
                        InputRegisters = new List<ValueRegister>(LocalConfig.InputRegisters),
                        HoldingRegisters = new List<ValueRegister>(LocalConfig.HoldingRegisters)
                    };
                    string json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);
                    File.WriteAllText("config.cfg", json);
                    return true;
                }
                else if (result == MessageBoxResult.No)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            return true;

        }
        public static bool LoadData()
        {
            if (File.Exists("config.cfg"))
            {
                string json;
                using (StreamReader reader = new("config.cfg"))
                {
                    json = reader.ReadToEnd();
                }
                DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(json);
                CoilRegisters = new ObservableCollection<DiscreteRegister>(dataSet.CoilRegisters);
                DiscreteInputs = new ObservableCollection<DiscreteRegister>(dataSet.DiscreteInputs);
                InputRegisters = new ObservableCollection<ValueRegister>(dataSet.InputRegisters);
                HoldingRegisters = new ObservableCollection<ValueRegister>(dataSet.HoldingRegisters);
                DataLoaded?.Invoke();
                return true;
            }
            return false;
        }
    }
}
