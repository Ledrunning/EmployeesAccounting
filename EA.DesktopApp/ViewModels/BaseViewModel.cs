using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace EA.DesktopApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _login;
        private string _password;

        [Required(AllowEmptyStrings = false)]
        public string LoginField
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        [Required(AllowEmptyStrings = false)]
        public string PasswordField
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        public string Error => "Enter the data!";

        public string this[string columnName] => ValidateProperty(columnName);
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return;
            }

            field = value;
            OnPropertyChanged(propertyName);
        }

        protected virtual string ValidateProperty(string columnName)
        {
            return string.Empty;
        }
    }
}