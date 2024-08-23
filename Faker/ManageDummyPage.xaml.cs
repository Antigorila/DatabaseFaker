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
using System.Text.Json;
using System.IO;
using System.Xml.Linq;

namespace Faker
{
    public partial class ManageDummyPage : Page
    {
        public class DummyType
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
        public class DummyData
        {
            public string Type { get; set; }
            public string Data { get; set; }
        }

        public ManageDummyPage()
        {
            InitializeComponent();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new HomePage();
        }

        private void FakeData_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new FakeMenu();
        }

        private void ManageDatabase_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new DatabaseSet();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new ManageDummyPage();
        }

        private void CreateNewCategory_Click(object sender, RoutedEventArgs e)
        {
            AddNewCategory addNewCategory = new AddNewCategory();
            addNewCategory.ShowDialog();
        }

        List<DummyData> Names = new List<DummyData>
        {
            new DummyData { Type = "Name", Data = "John" },
            new DummyData { Type = "Name", Data = "Bozo" }
        };

        //private void Write_Click(object sender, RoutedEventArgs e)
        //{
        //    //string jsonString = JsonSerializer.Serialize({ArrayName}, new JsonSerializerOptions { WriteIndented = true });
        //    string jsonString = JsonSerializer.Serialize(Names, new JsonSerializerOptions { WriteIndented = true });
        //    string filePath = "dummies/names.json";
        //    File.WriteAllText(filePath, jsonString);
        //}
    }
}
