using System.Collections.Generic;
using System.Linq;
using System.Windows;
using EA.DesktopApp.Contracts.ViewContracts;

namespace EA.DesktopApp.Services.ViewServices
{
    public class WindowManager : IWindowManager
    {
        private readonly Dictionary<Window, bool> _windowClosedFlags = new Dictionary<Window, bool>();
        private readonly IWindowFactory _windowFactory;

        public WindowManager(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public void ShowWindow<T>() where T : Window, new()
        {
            T windowInstance;

            var existingWindow = _windowClosedFlags.Keys.OfType<T>().FirstOrDefault();

            if (existingWindow == null || _windowClosedFlags[existingWindow])
            {
                windowInstance = (T)_windowFactory.GetWindow<T>();
                windowInstance.Closed += (sender, e) =>
                {
                    if (sender is Window win)
                    {
                        _windowClosedFlags[win] = true;
                    }
                };
                _windowClosedFlags[windowInstance] = false;
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
            var existingWindow = _windowClosedFlags.Keys.OfType<T>().FirstOrDefault();

            if (existingWindow == null || _windowClosedFlags[existingWindow])
            {
                return;
            }

            existingWindow.Close();
            _windowClosedFlags[existingWindow] = true;
        }
    }
}