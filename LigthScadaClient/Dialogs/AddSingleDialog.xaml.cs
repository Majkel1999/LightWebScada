using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataRegisters;

namespace LigthScadaClient
{
    public partial class AddSingleDialog : Window
    {
        private static readonly Regex regex = new Regex("^(?:[1-9][0-9]{3}|[1-9][0-9]{2}|[1-9][0-9]|[1-9])$");

        private int m_registerNumber = -1;
        public AddSingleDialog() => InitializeComponent();

        public bool ShowDialog(bool isDiscrete, out Register register, Window owner = null)
        {
            this.Owner = owner ?? App.Current.MainWindow;
            ShowDialog();
            if (m_registerNumber == -1)
            {
                register = null;
                return false;
            }

            if (isDiscrete)
            {
                register = new DiscreteRegister { RegisterNumber = m_registerNumber, CurrentValue = false };
                return true;
            }
            else
            {
                register = new ValueRegister { RegisterNumber = m_registerNumber, CurrentValue = 0 };
                return true;
            }

        }

        private void Confirm()
        {
            if (RegisterNumberTextBox.Text.Length > 0)
            {
                m_registerNumber = int.Parse(RegisterNumberTextBox.Text);
                Close();
            }
            else
                MessageBox.Show(Application.Current.Resources.MergedDictionaries[0]["emptyFieldMessage"].ToString());
        }

        private void RegisterNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !regex.IsMatch((sender as TextBox).Text + e.Text);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Confirm();
        }

        private void RegisterNumberTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Confirm();
            }
        }
    }
}
