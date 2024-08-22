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
    public partial class FakeMenu : Page
    {
        public FakeMenu()
        {
            InitializeComponent();
            DisplayAllTablesOnScrollViewer();
            DisplayAllColumnsToOneTable(SQL.Tables()[0]);
        }

        private void DisplayOneTableOnScrollViewer(string TableName)
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
                Text = TableName,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            Button button = new Button
            {
                Tag = TableName,
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Foreground = Brushes.Transparent,
                Style = (Style)FindResource("NoButtonHoover"),
                Cursor = Cursors.Hand,
            };
            button.Click += SelectTable;

            grid.Children.Add(textBlock);
            grid.Children.Add(button);

            border.Child = grid;

            TablesDisplay.Children.Add(border);
        }

        private void DisplayAllTablesOnScrollViewer()
        {
            TablesDisplay.Children.Clear();
            List<string> Tables = SQL.Tables();
            for (int i = 0; i < Tables.Count; i++)
            {
                DisplayOneTableOnScrollViewer(Tables[i]);
            }
        }

        private void DisplayOneColumn(string ColumnName, string ColumnType)
        {
            Border mainBorder = new Border
            {
                BorderBrush = Brushes.Black,
                Background = Brushes.White,
                BorderThickness = new Thickness(2),
                Margin = new Thickness(10),
                CornerRadius = new CornerRadius(10),
            };

            StackPanel mainStackPanel = new StackPanel();

            Border topBorder = new Border
            {
                Margin = new Thickness(5),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 0, 0, 1)
            };

            StackPanel topStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Label columnNameLabel = new Label
            {
                Content = $"{ColumnName}:",
                Margin = new Thickness(5, 5, 0, 5),
                HorizontalAlignment = HorizontalAlignment.Center,
                BorderBrush = Brushes.Black,
            };

            Label dataTypeLabel = new Label
            {
                Content = ColumnType,
                Margin = new Thickness(0, 5, 5, 5),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            topStackPanel.Children.Add(columnNameLabel);
            topStackPanel.Children.Add(dataTypeLabel);
            topBorder.Child = topStackPanel;

            mainStackPanel.Children.Add(topBorder);

            StackPanel bottomStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Label dummyTypeLabel = new Label
            {
                Content = "Dummy Type",
                Margin = new Thickness(0, 5, 5, 5),
                BorderBrush = Brushes.Black
            };

            ComboBox dummyComboBox = new ComboBox
            {
                Margin = new Thickness(5),
                Width = 150
            };

            Label quantityLabel = new Label
            {
                Content = "Quantity",
                Margin = new Thickness(5),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1, 0, 0, 0)
            };

            TextBox quantityTextBox = new TextBox
            {
                Margin = new Thickness(5),
                Width = 75
            };

            Label includeLabel = new Label
            {
                Content = "Include",
                Margin = new Thickness(5, 5, 0, 5),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1, 0, 0, 0)
            };

            CheckBox includeCheckBox = new CheckBox
            {
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center
            };

            bottomStackPanel.Children.Add(dummyTypeLabel);
            bottomStackPanel.Children.Add(dummyComboBox);
            bottomStackPanel.Children.Add(quantityLabel);
            bottomStackPanel.Children.Add(quantityTextBox);
            bottomStackPanel.Children.Add(includeLabel);
            bottomStackPanel.Children.Add(includeCheckBox);

            mainStackPanel.Children.Add(bottomStackPanel);

            mainBorder.Child = mainStackPanel;

            Columns.Children.Add(mainBorder);
        }
        private void DisplayHeaderToColumn(string TableName)
        {
            Grid grid = new Grid();

            ColumnDefinition column1 = new ColumnDefinition();
            column1.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(column1);

            ColumnDefinition column2 = new ColumnDefinition();
            column2.Width = GridLength.Auto;
            grid.ColumnDefinitions.Add(column2);

            Label label = new Label();
            label.Content = TableName;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.BorderBrush = Brushes.Black;
            label.Margin = new Thickness(5,5,0,5);
            label.Foreground = Brushes.White;
            label.BorderThickness = new Thickness(0, 0, 0, 2);
            label.BorderBrush = Brushes.Black;
            grid.Children.Add(label);
            Grid.SetColumn(label, 0);

            Button button = new Button
            {
                Content = "Run",
                Style = (Style)FindResource("DefaultButtonStyle"),
                Margin = new Thickness(0,5,5,5),
                BorderThickness = new Thickness(2)
            };
            grid.Children.Add(button);
            Grid.SetColumn(button, 1);

            Columns.Children.Add(grid);
        }
        private void DisplayAllColumnsToOneTable(string TableName)
        {
            Columns.Children.Clear();
            DisplayHeaderToColumn(TableName);
            List<string[]> ColumnsList = SQL.Query($"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{SQL.database}' AND TABLE_NAME = '{TableName}';");
            for (int i = 0; i < ColumnsList.Count; i++)
            {
                DisplayOneColumn(ColumnsList[i][0], ColumnsList[i][1]);
            }
        }

        private void SelectTable(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                DisplayAllColumnsToOneTable((sender as Button).Tag.ToString());
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new FakeMenu();
        }

        private void ManageDummy_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Do this...
            throw new NotImplementedException();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new HomePage();
        }

        private void ManageDatabase_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Do this... Its good basically but no back button...
            Navigation.MainContent.Content = new DatabaseSet();
        }
    }
}
