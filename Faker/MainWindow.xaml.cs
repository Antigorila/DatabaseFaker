using System.IO;
using System.Text;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Navigation.MainContent = MainContent;

            if (File.Exists("database.txt"))
            {
                if (Controller.IsDatabaseTxtContainsAllData())
                {
                    Controller.RefreshDatabaseConnection();
                    if (Controller.CanConnectToDatabase())
                    {
                        Navigation.MainContent.Content = new FakeMenu();
                    }
                    else
                    {
                        //TODO: Do some page where the dear user can wait and spam a Connect againg button until the program can connect to the database
                        MessageBox.Show("Can not connect to database!", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    Navigation.MainContent.Content = new DatabaseSet();
                }
            }
            else
            {
                Navigation.MainContent.Content = new DatabaseSet();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new DatabaseSet();
        }
    }
}