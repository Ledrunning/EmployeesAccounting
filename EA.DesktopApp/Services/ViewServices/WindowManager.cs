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
        private readonly Dictionary<(Type, Window), bool> _windowClosedFlags = new Dictionary<(Type, Window), bool>();


        private readonly IWindowFactory _windowFactory;

        public WindowManager(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public void ShowWindow<T>() where T : Window, new()
        {
            T windowInstance;

            var existingWindow = _windowClosedFlags.Keys.FirstOrDefault(k => k.Item1 == typeof(T) && !_windowClosedFlags[k]).Item2 as T;

            if (existingWindow == null)
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

            windowInstance.Owner = Application.Current.MainWindow;
            windowInstance.Show();
        }

        public void CloseWindow<T>() where T : Window
        {
            var existingWindow = _windowClosedFlags.Keys.FirstOrDefault(k => k.Item1 == typeof(T) && !_windowClosedFlags[k]).Item2 as T;

            if (existingWindow == null)
            {
                return;
            }

            existingWindow.Close();
            _windowClosedFlags[(typeof(T), existingWindow)] = true;
        }

        public void ShowModalWindow(string message)
        {
            var modalWindow = _windowFactory.GetWindow<ModalWindow>(message);
            modalWindow.Owner = Application.Current.MainWindow;
            modalWindow.Show();
        }
    }
}