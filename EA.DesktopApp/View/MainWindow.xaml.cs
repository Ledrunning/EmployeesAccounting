using EA.DesktopApp.ViewModels;
using System.Windows;
using Autofac;
using EA.DesktopApp.DiSetup;

namespace EA.DesktopApp.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = AutofacConfigure.Container.Resolve<MainViewModel>();
        }
    }
}