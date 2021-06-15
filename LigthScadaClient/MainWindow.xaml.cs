using System.ComponentModel;
using System.Windows;
using LigthScadaClient.Logic;
using LigthScadaClient.Pages;

namespace LigthScadaClient
{
    public partial class MainWindow : Window
    {
        public StatusPage statusPage;
        public CoilRegistersPage coilRegistersPage;
        public DiscreteInputsPage discreteInputsPage;
        public InputRegistersPage inputRegistersPage;
        public HoldingRegistersPage holdingRegistersPage;

        public MainWindow() => InitializeComponent();

        private void InitializePages()
        {
            statusPage = StatusPageFrame.Content as StatusPage;
            coilRegistersPage = CoilsFrame.Content as CoilRegistersPage;
            discreteInputsPage = DiscreteInputsFrame.Content as DiscreteInputsPage;
            inputRegistersPage = InputRegistersFrame.Content as InputRegistersPage;
            holdingRegistersPage = HoldingRegistersFrame.Content as HoldingRegistersPage;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = !LocalConfig.SaveData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializePages();
        }
    }
}
