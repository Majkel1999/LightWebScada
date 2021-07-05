using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataRegisters;
using LigthScadaClient.Logic;

namespace LigthScadaClient.Pages
{
    public partial class DiscreteInputsPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<DiscreteRegister> m_discreteInputs;

        public List<DiscreteRegister> DiscreteInputs
        {
            get { return m_discreteInputs.OrderBy(x => x.RegisterNumber).ToList(); }
        }

        public DiscreteInputsPage()
        {
            InitializeComponent();
            m_discreteInputs = LocalConfig.DiscreteInputs;
            DataContext = this;
            LocalConfig.DataLoaded += DataLoaded;
        }

        private void DataLoaded()
        {
            Notify("DiscreteInputs");
        }

        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private void AddSingleButton_Click(object sender, RoutedEventArgs e)
        {
            AddSingleDialog singleDialog = new();
            if (singleDialog.ShowDialog(true, out Register register))
            {
                if (!m_discreteInputs.Any(x => x.RegisterNumber == register.RegisterNumber))
                {
                    m_discreteInputs.Add((DiscreteRegister)register);
                    Notify("DiscreteInputs");
                }
                else
                {
                    MessageBox.Show(Application.Current.Resources.MergedDictionaries[0]["duplicate"].ToString());
                }
            }
        }

        private void AddRangeButton_Click(object sender, RoutedEventArgs e)
        {
            AddRangeDialog rangeDialog = new();
            if (rangeDialog.ShowDialog(true, out List<Register> registers))
            {
                foreach (Register reg in registers)
                {
                    if (!m_discreteInputs.Any(x => x.RegisterNumber == reg.RegisterNumber))
                    {
                        m_discreteInputs.Add((DiscreteRegister)reg);
                    }
                }
                Notify("DiscreteInputs");
            }
        }

        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteItemsFromList();
            }
        }

        private void DeleteItemsFromList()
        {
            if (RegistersListBox.SelectedItems.Count > 0)
            {
                var items = RegistersListBox.SelectedItems;
                int count = items.Count;
                for (int i = 0; i < count; i++)
                {
                    m_discreteInputs.Remove((DiscreteRegister)items[0]);
                    items.RemoveAt(0);
                }
            }
            Notify("DiscreteInputs");
        }
    }
}
