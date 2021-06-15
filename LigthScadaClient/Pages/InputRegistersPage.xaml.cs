using LigthScadaClient.DataModels;
using LigthScadaClient.Logic;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LigthScadaClient.Pages
{
    public partial class InputRegistersPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<ValueRegister> m_inputRegisters;

        public List<ValueRegister> InputRegisters
        {
            get { return m_inputRegisters.OrderBy(x => x.RegisterNumber).ToList(); }
        }

        public InputRegistersPage()
        {
            InitializeComponent();
            m_inputRegisters = LocalConfig.InputRegisters;
            DataContext = this;
            LocalConfig.DataLoaded += DataLoaded;
        }

        private void DataLoaded()
        {
            Notify("InputRegisters");
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
                if (!m_inputRegisters.Any(x => x.RegisterNumber == register.RegisterNumber))
                {
                    m_inputRegisters.Add((ValueRegister)register);
                    Notify("InputRegisters");
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
                    if (!m_inputRegisters.Any(x => x.RegisterNumber == reg.RegisterNumber))
                    {
                        m_inputRegisters.Add((ValueRegister)reg);
                    }
                }
                Notify("InputRegisters");
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
                    m_inputRegisters.Remove((ValueRegister)items[0]);
                    items.RemoveAt(0);
                }
            }
            Notify("InputRegisters");
        }
    }
}
