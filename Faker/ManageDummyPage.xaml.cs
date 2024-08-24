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
        private DummyType SelectedType;

        public ManageDummyPage()
        {
            InitializeComponent();
            DisplayAllTypes();
            DisplayPropertiesOfType(Controller.DummyTypes()[0]);    
        }
        private void DisplayOneType(string TypeName)
        {
            Border border = new Border
            {
                BorderBrush = Brushes.Black,
                Background = Brushes.White,
                BorderThickness = new Thickness(2),
                Height = 40,
                Margin = new Thickness(5),
                CornerRadius = new CornerRadius(5),
            };

            Grid grid = new Grid();

            TextBlock textBlock = new TextBlock
            {
                Text = TypeName,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            Button button = new Button
            {
                Tag = Controller.GetTypeByName(TypeName),
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Foreground = Brushes.Transparent,
                Style = (Style)FindResource("NoButtonHoover"),
                Cursor = Cursors.Hand,
            };
            button.Click += SelectType;

            grid.Children.Add(textBlock);
            grid.Children.Add(button);

            border.Child = grid;

            Types.Children.Add(border);
        }
        private void DisplayAllTypes()
        {
            for (int i = 0; i < Controller.DummyTypes().Count; i++)
            {
                DisplayOneType(Controller.DummyTypes()[i].Name);
            }
        }
        private void SelectType(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                Button button = sender as Button;
                DisplayPropertiesOfType(button.Tag as DummyType);
            }
        }
        private void DisplayPropertiesOfType(DummyType Type)
        {
            DataDisplay.Items.Clear();
            SelectedType = Type;
            Description.Text = Type.Description;

            for (int i = 0; i < SelectedType.GetData().Count; i++)
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Tag = SelectedType.GetData()[i];
                listBoxItem.Content = SelectedType.GetData()[i].Data;

                DataDisplay.Items.Add(listBoxItem);
            }
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
            Navigation.MainContent.Content = new ManageDummyPage();
        }

        private void AddNewDummy_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedType != null)
            {
                AddDummyData addDummyData = new AddDummyData(SelectedType.Name);
                addDummyData.ShowDialog();
                Navigation.MainContent.Content = new ManageDummyPage();
            }
        }

        private void DeleteDummy_Click(object sender, RoutedEventArgs e)
        {
            if (DataDisplay.SelectedItem != null)
            {
                ListBoxItem listBoxItem = DataDisplay.SelectedItem as ListBoxItem;
                SelectedType.RemoveData(listBoxItem.Tag as DummyData);
                MessageBox.Show("Dummy Data has been deleted!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DisplayPropertiesOfType(SelectedType);
            }
            else
            {
                MessageBox.Show("Nothing to delete!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
