using EA.DesktopApp.DiSetup;
using EA.DesktopApp.ViewModels;
using System;
using System.Windows;
using Autofac;

namespace EA.DesktopApp.View
{
    public partial class AdminForm : Window
    {
        public AdminForm()
        {
            InitializeComponent();
            DataContext = AutofacConfigure.Container.Resolve<AdminViewModel>();
        }
    }
}