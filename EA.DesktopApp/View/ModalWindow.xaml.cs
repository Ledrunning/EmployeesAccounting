using EA.DesktopApp.DiSetup;
using EA.DesktopApp.ViewModels;
using System.Windows;
using Autofac;

namespace EA.DesktopApp.View
{
    /// <summary>
    ///     Логика взаимодействия для ModalWindow.xaml
    /// </summary>
    public partial class ModalWindow : Window
    {
        public ModalWindow()
        {
            InitializeComponent();
            DataContext = AutofacConfigure.Container.Resolve<ModalViewModel>();
        }
    }
}