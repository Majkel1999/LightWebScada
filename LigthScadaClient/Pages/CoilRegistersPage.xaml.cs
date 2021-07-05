using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataRegisters;
using LigthScadaClient.Logic;

namespace LigthScadaClient.Pages
{
    public partial class CoilRegistersPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<DiscreteRegister> m_coilRegisters;
        public List<DiscreteRegister> CoilRegisters
        {
            get => m_coilRegisters.OrderBy(x => x.RegisterNumber).ToList();
        }

        public CoilRegistersPage()
        {
            InitializeComponent();
            m_coilRegisters = LocalConfig.CoilRegisters;
            DataContext = this;
            LocalConfig.DataLoaded += DataLoaded;
        }

        private void DataLoaded()
        {
            Notify("CoilRegisters");
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
                if (!m_coilRegisters.Any(x => x.RegisterNumber == register.RegisterNumber))
                {
                    m_coilRegisters.Add((DiscreteRegister)register);
                    Notify("CoilRegisters");
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
                    if (!m_coilRegisters.Any(x => x.RegisterNumber == reg.RegisterNumber))
                    {
                        m_coilRegisters.Add((DiscreteRegister)reg);
                    }
                }
                Notify("CoilRegisters");
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
                    Trace.WriteLine(m_coilRegisters.Remove((DiscreteRegister)items[0]));
                    items.RemoveAt(0);
                }
            }
            Notify("CoilRegisters");
        }
    }
}
