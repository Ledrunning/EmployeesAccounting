using System.Windows;
using Autofac;
using EA.DesktopApp.DiSetup;
using EA.DesktopApp.ViewModels;

namespace EA.DesktopApp.View
{
    /// <summary>
    ///     Логика взаимодействия для RegistrationForm.xaml
    /// </summary>
    public partial class RedactorForm : Window
    {
        public RedactorForm()
        {
            InitializeComponent();
            DataContext = AutofacConfigure.Container.Resolve<RedactorViewModel>();
        }
    }
}