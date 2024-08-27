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
using System.Windows.Shapes;

namespace Faker
{
    public partial class AddDummyData : Window
    {
        private DummyType Type;
        public AddDummyData(string typeName)
        {
            InitializeComponent();
            this.Type = Controller.GetTypeByName(typeName);
            TypeName.Content = typeName;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Data.Text == string.Empty)
            {
                MessageBox.Show("Data must be filled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (IsDataAlreadyExist())
            {
                MessageBox.Show("This data already exists, come up with something else.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DummyData newDummyData = new DummyData(Type, Data.Text);
            newDummyData.Create();
            MessageBox.Show("Data has been created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private bool IsDataAlreadyExist()
        {
            for (int i = 0; i < Type.GetAllData().Count; i++)
            {
                if (Type.GetAllData()[i].Data == Data.Text)
                {
                    return true;
                }
            }

            return false;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
