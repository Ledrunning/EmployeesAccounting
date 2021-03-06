﻿using System;
using System.Windows;

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
        }

        public RegistrationForm(bool test)
        {
            InitializeComponent();
            Test = test;
        }

        private bool Test { get; }

        public bool IsClosed { get; private set; }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsClosed = true;
        }
    }
}