using System;
using System.Windows;
using System.Windows.Controls;

namespace EA.DesktopApp.Helpers
{
    public class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxHelper),
                new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static string GetBoundPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject obj, string value)
        {
            obj.SetValue(BoundPasswordProperty, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject dObj, DependencyPropertyChangedEventArgs e)
        {
            if (!(dObj is PasswordBox passwordBox))
            {
                return;
            }

            passwordBox.PasswordChanged -= PasswordChanged;

            // Use a dispatcher to ensure that this code runs on the UI thread
            passwordBox.Dispatcher.BeginInvoke((Action)(() =>
            {
                passwordBox.Password = e.NewValue != null ? e.NewValue.ToString() : string.Empty;
            }));

            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetBoundPassword(passwordBox, passwordBox.Password);
            }
        }
    }
}