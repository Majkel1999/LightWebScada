using LigthScadaClient.DataModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LigthScadaClient.Pages
{
    public partial class DiscreteInputsPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<DiscreteRegister> m_discreteInputs;

        public List<DiscreteRegister> DiscreteInputs
        {
            get { return m_discreteInputs.OrderBy(x => x.RegisterNumber).ToList(); }
            set { m_discreteInputs = value; }
        }

        public DiscreteInputsPage()
        {
            InitializeComponent();
            m_discreteInputs = new List<DiscreteRegister>();
            DataContext = this;
            MainWindow.DataLoaded += DataLoaded;
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
    }
}
