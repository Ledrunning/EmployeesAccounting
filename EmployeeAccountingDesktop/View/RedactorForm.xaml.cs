using System;
using System.Windows;

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
        }

        public RedactorForm(bool test)
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