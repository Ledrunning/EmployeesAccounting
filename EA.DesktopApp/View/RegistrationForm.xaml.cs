﻿using EA.DesktopApp.DiSetup;
using EA.DesktopApp.ViewModels;
using System;
using System.Windows;
using Autofac;

namespace EA.DesktopApp.View
{
    /// <summary>
    ///     Логика взаимодействия для RegistrationForm.xaml
    /// </summary>
    public partial class RegistrationForm : Window
    {
        public RegistrationForm()
        {
            InitializeComponent();
            DataContext = AutofacConfigure.Container.Resolve<RegistrationViewModel>();
        }

        public bool IsClosed { get; private set; }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsClosed = true;
        }
    }
}