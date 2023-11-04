using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.View;

namespace EA.DesktopApp.Services.ViewServices
{
    public class WindowManager : IWindowManager
    {
        private const double ResizeScale = 0.25;

        private readonly Dictionary<(Type type, Window window), bool> _windowClosedFlags =
            new Dictionary<(Type, Window), bool>();


        private readonly IWindowFactory _windowFactory;

        private ModalWindow _currentModalWindow;

        public WindowManager(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public void ShowWindow<T>() where T : Window, new()
        {
            var windowInstance = WindowMemorize<T>();

            windowInstance.Owner = Application.Current.MainWindow;
            windowInstance.Show();
        }

        public void CloseWindow<T>() where T : Window
        {
            var existingWindow = _windowClosedFlags.Keys
                .FirstOrDefault(k => k.Item1 == typeof(T) && !_windowClosedFlags[k]).Item2 as T;

            if (existingWindow == null)
            {
                return;
            }

            existingWindow.Close();
            _windowClosedFlags[(typeof(T), existingWindow)] = true;
        }

        public void ShowModalWindow(string message)
        {
            _currentModalWindow = _windowFactory.GetWindow<ModalWindow>(message);
            _currentModalWindow.Owner = Application.Current.MainWindow;
            _currentModalWindow.Height = SystemParameters.PrimaryScreenHeight * ResizeScale;
            _currentModalWindow.Width = SystemParameters.PrimaryScreenWidth * ResizeScale;
            _currentModalWindow.Show();
        }

        public void CloseModalWindow()
        {
            _currentModalWindow?.Close();
        }

        private T WindowMemorize<T>() where T : Window, new()
        {
            T windowInstance;

            if (!(_windowClosedFlags.Keys.FirstOrDefault(k => k.type == typeof(T) && !_windowClosedFlags[k]).window is T
                    existingWindow))
            {
                windowInstance = (T)_windowFactory.GetWindow<T>();
                windowInstance.Closed += (sender, e) =>
                {
                    if (sender is Window window)
                    {
                        _windowClosedFlags[(typeof(T), window)] = true;
                    }
                };
                _windowClosedFlags[(typeof(T), windowInstance)] = false;
            }
            else
            {
                windowInstance = existingWindow;
            }

            return windowInstance;
        }
    }
}