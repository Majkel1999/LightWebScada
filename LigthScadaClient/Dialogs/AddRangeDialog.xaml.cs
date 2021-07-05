using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using DataRegisters;

namespace LigthScadaClient
{
    public partial class AddRangeDialog : Window
    {

        private static readonly Regex regex = new Regex("^(?:[1-9][0-9]{3}|[1-9][0-9]{2}|[1-9][0-9]|[1-9])$");

        private int m_startRegister = -1;
        private int m_endRegister = -1;
        public AddRangeDialog() => InitializeComponent();

        public bool ShowDialog(bool isDiscrete, out List<Register> registers, Window owner = null)
        {
            this.Owner = owner ?? App.Current.MainWindow;
            ShowDialog();
            if (m_startRegister == -1 || m_endRegister == -1)
            {
                registers = null;
                return false;
            }

            if (isDiscrete)
            {
                registers = new List<Register>();
                for (int i = m_startRegister; i <= m_endRegister; i++)
                {
                    registers.Add(new DiscreteRegister
                    {
                        RegisterNumber = i,
                        CurrentValue = false
                    });
                }
                return true;
            }
            else
            {
                registers = new List<Register>();
                for (int i = m_startRegister; i <= m_endRegister; i++)
                {
                    registers.Add(new ValueRegister
                    {
                        RegisterNumber = i,
                        CurrentValue = 0
                    });
                }
                return true;
            }
        }

        private void Confirm()
        {
            bool LengthCheck = RegisterStartTextBox.Text.Length > 0 && RegisterEndTextBox.Text.Length > 0;
            if (LengthCheck)
            {
                m_startRegister = int.Parse(RegisterStartTextBox.Text);
                m_endRegister = int.Parse(RegisterEndTextBox.Text);
                if (m_startRegister < m_endRegister)
                {
                    Close();
                }
                else
                    MessageBox.Show(Application.Current.Resources.MergedDictionaries[0]["startBiggerThanEndMessage"].ToString());
            }
            else
                MessageBox.Show(Application.Current.Resources.MergedDictionaries[0]["emptyFieldMessage"].ToString());
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Confirm();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Handle_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !regex.IsMatch((sender as TextBox).Text + e.Text);
        }

        private void RegisterEndTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Confirm();
            }
        }
    }
}
