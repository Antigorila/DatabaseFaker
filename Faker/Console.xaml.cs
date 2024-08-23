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
    public partial class Console : Window
    {
        private static ListBox StaticDisplay;
        public Console()
        {
            InitializeComponent();
            StaticDisplay = Display;
        }

        public static void WriteLine()
        {
            StaticDisplay.Items.Add("");
        }

        public static void WriteLine(object Message)
        {
            StaticDisplay.Items.Add(Message.ToString());
        }

        public static void Clear()
        {
            StaticDisplay.Items.Clear();
        }
    }
}
