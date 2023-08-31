using System.Windows;
using EA.DesktopApp.DiSetup;

namespace EA.DesktopApp
{
    /// <summary>
    ///     Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AutofacConfigure.ConfigureContainer();
        }
    }
}