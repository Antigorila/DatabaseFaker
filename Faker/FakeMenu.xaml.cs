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
        IsIncluded,
        IsUnique
    }
    public partial class FakeMenu : Page
    {
        class Column
        {
            public string Name { get; set; }
            public string DataType { get; set; }
            public DummyType DummyType { get; set; }
            public bool IsIncluded { get; set; }
            public bool IsUnique { get; set; }
            public Border Appearance { get; set; }

            public Column(string Name, string dataType)
            {
                this.Name = Name;
                this.DataType = dataType;
                this.DummyType = Controller.DummyTypes()[0];
            }

            public void CreateAppearance(StackPanel stackPanel)
            {
                Appearance = new Border
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
                    Content = $"{this.Name}:",
                    Margin = new Thickness(5, 5, 0, 5),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    BorderBrush = Brushes.Black,
                };

                Label dataTypeLabel = new Label
                {
                    Content = this.DataType,
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
                for (int i = 0; i < Controller.DummyTypes().Count; i++)
                {
                    dummyComboBox.Items.Add(Controller.DummyTypes()[i].Name);
                }
                dummyComboBox.SelectedIndex = 0;
                dummyComboBox.SelectionChanged += SetDummy;


                Label uniqueLabel = new Label
                {
                    Content = "Unique",
                    Margin = new Thickness(5, 5, 0, 5),
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1, 0, 0, 0)
                };

                CheckBox uniqueCheckBox = new CheckBox
                {
                    Margin = new Thickness(5),
                    VerticalAlignment = VerticalAlignment.Center,
                    Tag = this.Name
                };
                uniqueCheckBox.Checked += Unique_Checked;
                uniqueCheckBox.Unchecked += Unique_Unchecked;

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
                    Tag = this.Name
                };
                includeCheckBox.Checked += Column_Checked;
                includeCheckBox.Unchecked += Column_Unchecked;

                bottomStackPanel.Children.Add(dummyTypeLabel);
                bottomStackPanel.Children.Add(dummyComboBox);
                bottomStackPanel.Children.Add(uniqueLabel);
                bottomStackPanel.Children.Add(uniqueCheckBox);
                bottomStackPanel.Children.Add(includeLabel);
                bottomStackPanel.Children.Add(includeCheckBox);

                mainStackPanel.Children.Add(bottomStackPanel);

                Appearance.Child = mainStackPanel;

                stackPanel.Children.Add(Appearance);
            }

            private void Column_Checked(object sender, RoutedEventArgs e)
            {
                if (sender != null)
                {
                    SetColumnProperty(ColumnProperties.IsIncluded, true);
                }
            }
            private void Column_Unchecked(object sender, RoutedEventArgs e)
            {
                if (sender != null)
                {
                    SetColumnProperty(ColumnProperties.IsIncluded, false);
                }
            }

            private void Unique_Checked(object sender, RoutedEventArgs e)
            {
                if (sender != null)
                {
                    SetColumnProperty(ColumnProperties.IsUnique, true);
                }
            }
            private void Unique_Unchecked(object sender, RoutedEventArgs e)
            {
                if (sender != null)
                {
                    SetColumnProperty(ColumnProperties.IsUnique, false);
                }
            }

            private void SetDummy(object sender, RoutedEventArgs e)
            {
                if (sender != null)
                {
                    ComboBox DummyComboBox = sender as ComboBox;
                    if (DummyComboBox.SelectedIndex != 0)
                    {
                        if (DummyComboBox.SelectedItem == "Number")
                        {
                            MessageBox.Show("Number");
                        }
                        else if (DummyComboBox.SelectedItem == "Word")
                        {
                            MessageBox.Show("Word");
                        }
                        else if (DummyComboBox.SelectedItem == "ForeginID")
                        {
                            MessageBox.Show("ForeginID");
                        }
                        else
                        {
                            SetColumnProperty(ColumnProperties.DummyType, Controller.GetTypeByName(DummyComboBox.SelectedItem.ToString()));
                        }
                    }
                }
            }

            private void SetColumnProperty(ColumnProperties ColumnProperty, object Value)
            {
                switch (ColumnProperty)
                {
                    case ColumnProperties.DummyType:
                        this.DummyType = Value as DummyType;
                        break;
                    case ColumnProperties.IsIncluded:
                        this.IsIncluded = (bool)Value;
                        break;
                    case ColumnProperties.IsUnique:
                        this.IsUnique = (bool)Value;
                        break;
                }
            }
        }
        class SqlCommandBuilder
        {
            private static string Columns = string.Empty;
            private static string Values = string.Empty;
            private static List<string> Commands = new List<string>();
            public static void Add(Column column)
            {
                Columns += $"`{column.Name}`, ";
                Values += $"'{column.DummyType.GetOneRandomData().Data}', ";
                Console.WriteLine(Values);
            }
            public static void AddUnique(Column column, int index)
            {
                Columns += $"`{column.Name}`, ";
                Values += $"'{column.DummyType.GetOneRandomData(index).Data}', ";
                Console.WriteLine(Values);
            }
            public static void AddCommand(string TableName)
            {
                Columns = Columns.TrimEnd(' ', ',');
                Values = Values.TrimEnd(' ', ',');

                string command = $"INSERT INTO `{TableName}`({Columns}) VALUES ({Values})";
                Console.WriteLine(command);
                Commands.Add(command);

                Columns = string.Empty;
                Values = string.Empty ;
            }
            public static void Run()
            {
                for (int i = 0; i < Commands.Count; i++)
                {
                    try
                    {
                        Console.WriteLine(Commands[i]);
                        SQL.Command(Commands[i]);
                    }
                    catch (MySqlConnector.MySqlException ex)
                    {
                        switch (ex.ErrorCode)
                        {
                            case MySqlConnector.MySqlErrorCode.NoReferencedRow2:
                                MessageBox.Show("This table cannot be updated because it contains a foreign key, if you want to update the table, enter a foreign key as well!", "MySqlException: NoReferencedRow2", MessageBoxButton.OK, MessageBoxImage.Error);
                                Commands.Clear();
                                break;
                            case MySqlConnector.MySqlErrorCode.DuplicateKeyEntry:
                                MessageBox.Show("It seems like the database already contains a few data you want to upload as a unique data! ", "MySqlException: DuplicateKeyEntry", MessageBoxButton.OK, MessageBoxImage.Error);
                                Commands.Clear();
                                break;
                        }
                       
                        return;
                    }
                }

                Commands.Clear();
            }

            public static void Purge()
            {
                Columns = string.Empty;
                Values = string.Empty;
                Commands.Clear();
            }
        }
        private List<Column> ColumnList = new List<Column>();
        private TextBox Quantity;
        private string SelectedTableName;
        public FakeMenu()
        {
            InitializeComponent();
            DisplayAllTablesOnScrollViewer();
            RefreshColumnList(SQL.Tables()[0]);
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
            SqlCommandBuilder.Purge();
            SelectedTableName = TableName;
            Columns.Children.Clear();
            DisplayHeaderToColumn(TableName);
            List<string[]> ColumnsList = SQL.Query($"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{SQL.database}' AND TABLE_NAME = '{TableName}';");
            for (int i = 0; i < ColumnsList.Count; i++)
            {
                ColumnList[i].CreateAppearance(Columns);
            }
        }
        private void Run_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Add numbers and words with own quantity, number between and also add the ForeginID type(good luck...)
            List<Column> SelectedColumns = new List<Column>();
            if (Quantity.Text != string.Empty && int.TryParse(Quantity.Text, out int quantity))
            {
                for (int i = 0; i < ColumnList.Count; i++)
                {
                    if (ColumnList[i].IsIncluded)
                    {
                        if (ColumnList[i].DummyType.Name == string.Empty)
                        {
                            MessageBox.Show("There must be a selected Dummy Type at all included column!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        if (ColumnList[i].IsUnique && quantity > Controller.GetTypeByName(ColumnList[i].DummyType.Name).GetAllData().Count())
                        {
                            MessageBox.Show($"The Unique Faking of the {ColumnList[i].Name} column is not possible because the chosen DummyType({ColumnList[i].DummyType.Name}) not having enough specimen in it, you need to expand that first!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        SelectedColumns.Add(ColumnList[i]);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            for (int i = 0; i < int.Parse(Quantity.Text); i++)
            {
                for (int j = 0; j < SelectedColumns.Count; j++)
                {
                    if (SelectedColumns[j].IsUnique)
                    {
                        SqlCommandBuilder.AddUnique(SelectedColumns[j], i);
                    }
                    else
                    {
                        SqlCommandBuilder.Add(SelectedColumns[j]);
                    }
                }
                SqlCommandBuilder.AddCommand(SelectedTableName);          
            }
            SqlCommandBuilder.Run();
            MessageBox.Show("The data has been uploaded successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshColumnList(string TableName) 
        {
            ColumnList.Clear();
            List<string[]> ColumnsNameList = SQL.Query($"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{SQL.database}' AND TABLE_NAME = '{TableName}';");
            for (int i = 0; i < ColumnsNameList.Count; i++)
            {
                ColumnList.Add(new Column(ColumnsNameList[i][0], ColumnsNameList[i][1]));
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
