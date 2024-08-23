using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace Faker
{
    public partial class App : Application
    {
        private static Console ConsoleWindow;
        protected override void OnStartup(StartupEventArgs e)
        {
            ConsoleWindow = new Console();
            base.OnStartup(e);

            EventManager.RegisterClassHandler(typeof(Window), Window.KeyDownEvent, new KeyEventHandler(OnKeyDown));
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.E)
            {
                Console.WriteLine();
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
                ConsoleWindow.Show();
            }
        }
    }
}
