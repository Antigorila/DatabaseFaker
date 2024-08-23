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
    enum ColumnProperties
    {
        DummyType,
        IsIncluded
    }
    public partial class FakeMenu : Page
    {
        class Column
        {
            public string Name { get; set; }
            public string DummyType { get; set; }
            public bool IsIncluded { get; set; }

            public Column(string Name)
            {
                this.Name = Name;
            }
        }
        private List<Column> ColumnList = new List<Column>();
        private TextBox Quantity;
        public FakeMenu()
        {
            InitializeComponent();
            DisplayAllTablesOnScrollViewer();
            DisplayAllColumnsToOneTable(SQL.Tables()[0]);
            RefreshColumnList(SQL.Tables()[0]);
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
                Width = 150,
                Tag = ColumnName
            };
            dummyComboBox.SelectedIndex = 0;
            dummyComboBox.SelectionChanged += SetDummy;
            dummyComboBox.Items.Add("item 1");
            dummyComboBox.Items.Add("item 2");
            dummyComboBox.Items.Add("item 3");
            dummyComboBox.Items.Add("item 4");

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
                VerticalAlignment = VerticalAlignment.Center,
                Tag = ColumnName
            };
            includeCheckBox.Checked += Column_Checked;
            includeCheckBox.Unchecked += Column_Unchecked;

            bottomStackPanel.Children.Add(dummyTypeLabel);
            bottomStackPanel.Children.Add(dummyComboBox);
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

            ColumnDefinition column3 = new ColumnDefinition();
            column3.Width = GridLength.Auto;
            grid.ColumnDefinitions.Add(column3);

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

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Margin = new Thickness(0, 5, 0, 5);

            Label label1 = new Label();
            label1.Content = "Quantity: ";
            label1.Foreground = Brushes.White;
            label1.BorderThickness = new Thickness(0, 0, 0, 2);
            label1.BorderBrush = Brushes.Black;

            stackPanel.Children.Add(label1);

            TextBox textBox = new TextBox();
            textBox.Width = 100;
            textBox.VerticalContentAlignment = VerticalAlignment.Center;
            textBox.BorderThickness = new Thickness(2);
            textBox.BorderBrush = Brushes.Black;
            stackPanel.Children.Add(textBox);
            Quantity = textBox;

            grid.Children.Add(stackPanel);
            Grid.SetColumn(stackPanel, 1);

            Button button = new Button
            {
                Content = "Run",
                Style = (Style)FindResource("DefaultButtonStyle"),
                Margin = new Thickness(0,5,5,5),
                BorderThickness = new Thickness(2)
            };
            button.Click += Run_Click;
            grid.Children.Add(button);
            Grid.SetColumn(button, 2);

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
        private void SetDummy(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                ComboBox DummyComboBox = sender as ComboBox;
                if (DummyComboBox.SelectedIndex != 0)
                {
                    SetColumnProperty(DummyComboBox.Tag.ToString(), ColumnProperties.DummyType, DummyComboBox.SelectedItem);
                }
            }
        }
        private void Run_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < ColumnList.Count; i++)
            {
                if (ColumnList[i].IsIncluded && ColumnList[i].DummyType == string.Empty)
                {
                    MessageBox.Show("There must be a selected Dummy Type at all included column!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            //TODO: Do the Dummy upload
            //Remeber ther is a Quantity TextBox
        }
        private void Column_Checked(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                SetColumnProperty((sender as CheckBox).Tag.ToString(), ColumnProperties.IsIncluded, true);
            }
        }
        private void Column_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                SetColumnProperty((sender as CheckBox).Tag.ToString(), ColumnProperties.IsIncluded, false);
            }
        }
        private void SetColumnProperty(string ColumnName, ColumnProperties ColumnProperty, object Value)
        {
            for (int i = 0; i < ColumnList.Count; i++)
            {
                if (ColumnList[i].Name == ColumnName)
                {
                    switch (ColumnProperty)
                    {
                        case ColumnProperties.DummyType:
                            ColumnList[i].DummyType = Value.ToString();
                            break;
                        case ColumnProperties.IsIncluded:
                            ColumnList[i].IsIncluded = (bool)Value;
                            break;
                    }
                }
            }
        }
        private void RefreshColumnList(string TableName) 
        {
            ColumnList.Clear();
            List<string[]> ColumnsNameList = SQL.Query($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{SQL.database}' AND TABLE_NAME = '{TableName}';");
            for (int i = 0; i < ColumnsNameList.Count; i++)
            {
                ColumnList.Add(new Column(ColumnsNameList[i][0]));
            }
        }
        private void SelectTable(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                string TableName = (sender as Button).Tag.ToString();
                RefreshColumnList(TableName);
                DisplayAllColumnsToOneTable(TableName);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new FakeMenu();
        }

        private void ManageDummy_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new ManageDummyPage();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new HomePage();
        }

        private void ManageDatabase_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainContent.Content = new DatabaseSet();
        }
    }
}
