using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.DiSetup;
using EA.DesktopApp.ViewModels;
using NLog;

namespace EA.DesktopApp.View
{
    /// <summary>
    ///     Логика взаимодействия для RegistrationForm.xaml
    /// </summary>
    public partial class RedactorForm : Window
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public RedactorForm()
        {
            InitializeComponent();

            var viewModel = AutofacConfigure.Container.Resolve<RedactorViewModel>();
            DataContext = viewModel;

            // Handle unhandled exceptions
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Info("Failed to load employees from server {e}", e);

            // Prevent the application from crashing
            e.Handled = true;
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as RedactorViewModel;
            if (!(viewModel is IAsyncInitializer initializer))
            {
                return;
            }

            try
            {
                await initializer.InitializeDataAsync();
            }
            catch (Exception ex)
            {
                Logger.Info("Failed to load employees from server {ex}", ex);
            }
        }
    }
}