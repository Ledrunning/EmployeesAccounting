using System.Windows;
using Autofac;
using EA.DesktopApp.DiSetup;
using EA.DesktopApp.View;

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
            using (var scope = AutofacConfigure.Container.BeginLifetimeScope())
            {
                var window = scope.Resolve<MainWindow>();
                window.Show();
            }
        }
    }
}