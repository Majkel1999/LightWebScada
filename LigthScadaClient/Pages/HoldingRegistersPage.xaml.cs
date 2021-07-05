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
    public partial class HoldingRegistersPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<ValueRegister> m_holdingRegisters;

        public List<ValueRegister> HoldingRegisters
        {
            get => m_holdingRegisters.OrderBy(x => x.RegisterNumber).ToList();
        }

        public HoldingRegistersPage()
        {
            InitializeComponent();
            m_holdingRegisters = LocalConfig.HoldingRegisters;
            DataContext = this;
            LocalConfig.DataLoaded += DataLoaded;
        }

        private void DataLoaded()
        {
            Notify("HoldingRegisters");
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
            if (singleDialog.ShowDialog(false, out Register register))
            {
                if (!m_holdingRegisters.Any(x => x.RegisterNumber == register.RegisterNumber))
                {
                    m_holdingRegisters.Add((ValueRegister)register);
                    Notify("HoldingRegisters");
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
            if (rangeDialog.ShowDialog(false, out List<Register> registers))
            {
                foreach (Register reg in registers)
                {
                    if (!m_holdingRegisters.Any(x => x.RegisterNumber == reg.RegisterNumber))
                    {
                        m_holdingRegisters.Add((ValueRegister)reg);
                    }
                }
                Notify("HoldingRegisters");
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
                    m_holdingRegisters.Remove((ValueRegister)items[0]);
                    items.RemoveAt(0);
                }
            }
            Notify("HoldingRegisters");
        }
    }
}
