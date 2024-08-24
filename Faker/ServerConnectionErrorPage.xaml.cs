using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Faker
{
    public partial class ServerConnectionErrorPage : Page
    {
        private MainWindow mainWindow;
        public ServerConnectionErrorPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            ErrorDisplay.Content = Controller.error.Message;
        }

        private void Reconnect_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
            MainWindow newMainWindow = new MainWindow();
            newMainWindow.Show();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            string exceptionDetails = $"Exception Message: {Controller.error.Message}\n" +
                           $"Stack Trace: {Controller.error.StackTrace}\n" +
                           $"Exception Type: {Controller.error.GetType().FullName}";

            Clipboard.SetText(exceptionDetails);
        }
    }
}
