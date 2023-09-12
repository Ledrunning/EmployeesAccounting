using System;
using System.ComponentModel;
using System.Windows;

namespace EA.DesktopApp.Helpers
{
    public static class WindowClosingBehavior
    {
        public static readonly DependencyProperty HandleClosingProperty =
            DependencyProperty.RegisterAttached(
                "HandleClosing",
                typeof(bool),
                typeof(WindowClosingBehavior),
                new PropertyMetadata(false, OnHandleClosingChanged));

        public static event EventHandler WindowClose;

        public static bool GetHandleClosing(DependencyObject obj)
        {
            return (bool)obj.GetValue(HandleClosingProperty);
        }

        public static void SetHandleClosing(DependencyObject obj, bool value)
        {
            obj.SetValue(HandleClosingProperty, value);
        }

        private static void OnHandleClosingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Window window))
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                window.Closing += OnWindowClosing;
            }
            else
            {
                window.Closing -= OnWindowClosing;
            }
        }

        private static void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (!(sender is Window window))
            {
                return;
            }

            WindowClose?.Invoke(sender, e);
            e.Cancel = true;
            window.Hide();
        }
    }
}