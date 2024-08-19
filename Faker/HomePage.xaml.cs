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
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            LoadDatabaseProperties();
        }

        private void LoadDatabaseProperties()
        {
            DatabaseName.Content = SQL.database;
            Version.Content = SQL.GetOne("SELECT VERSION();");
            Size.Content = SQL.GetOne($"SELECT ROUND(SUM(data_length + index_length) / 1024 / 1024, 2) AS 'Size (MB)' FROM information_schema.tables WHERE table_schema = '{SQL.database}' GROUP BY table_schema;") + " MB";
            TablesCount.Content = SQL.Tables().Count();

            string uptimeQuery = "SHOW GLOBAL STATUS LIKE 'Uptime';";
            string uptimeSecondsStr = SQL.Query(uptimeQuery)[0][1];
            double uptimeSeconds = double.Parse(uptimeSecondsStr);
            double uptimeMinutes = Math.Round(uptimeSeconds / 60, 2);

            UpTime.Content = $"{uptimeMinutes} Min";

        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new HomePage();
        }

        private void FakeData_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new FakeMenu();
        }

        private void ManageDummy_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Do this...
            throw new NotImplementedException();
        }

        private void ManageDatabase_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Do this... Its good basically but no back button...
            Navigation.MainContent.Content = new DatabaseSet();
        }
    }
}
