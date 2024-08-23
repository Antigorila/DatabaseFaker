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
using System.IO;

namespace Faker
{
    public partial class DatabaseSet : Page
    {
        public DatabaseSet()
        {
            InitializeComponent();

            if (File.Exists("database.txt"))
            {
                Datasource.Text = Controller.DatabaseTxt()[0];
                Port.Text = Controller.DatabaseTxt()[1];
                Username.Text = Controller.DatabaseTxt()[2];
                Password.Text = Controller.DatabaseTxt()[3];
                DatabaseName.Text = Controller.DatabaseTxt()[4];
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (! ValidateInputData(Datasource, "127.0.0.1"))
            {
                return;
            }

            if (! ValidateInputData(Port, "3306"))
            {
                return;
            }

            if (IsEmpty(Username))
            {
                return;
            }

            if (IsEmpty(DatabaseName))
            {
                return;
            }

            Controller.WriteDatabaseTxt(Datasource.Text, Port.Text, Username.Text, Password.Text, DatabaseName.Text);
            Navigation.MainContent.Content = new HomePage();
        }

        private bool ValidateInputData(TextBox textbox, string format)
        {
            if (textbox.Text != string.Empty)
            {
                if (!Controller.MatchingMask(format, textbox.Text))
                {
                    MessageBox.Show("Invalid format!", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Empty input field!", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            return true;
        }

        private bool IsEmpty(TextBox textbox)
        {
            if (textbox.Text == string.Empty)
            {
                MessageBox.Show("Empty input field!", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }

            return false;
        }

        private void FakeData_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new FakeMenu();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new HomePage();
        }

        private void ManageDummy_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Do this...
            throw new NotImplementedException();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new DatabaseSet();
        }
    }
}
