using EA.DesktopApp.Resources.Messages;
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

        private string _personDepartment;

        private string _personLastName;

        private string _personName;

        protected Dictionary<string, string> errors = new Dictionary<string, string>();

        [Required(AllowEmptyStrings = false)]
        public string LoginField
        {
            get => _login;
            set => SetField(ref _login, value, nameof(LoginField));
        }

        [Required(AllowEmptyStrings = false)]
        public string PasswordField
        {
            get => _password;
            set => SetField(ref _password, value, nameof(PasswordField));
        }

        /// <summary>
        ///     Binding person name to TextBox
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string PersonName
        {
            get => _personName;
            set => SetField(ref _personName, value, nameof(PersonName));
        }

        /// <summary>
        ///     Binding person last name TextBox
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string PersonLastName
        {
            get => _personLastName;
            set => SetField(ref _personLastName, value, nameof(PersonLastName));
        }

        /// <summary>
        ///     Binding person department TextBox
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string PersonDepartment
        {
            get => _personDepartment;
            set => SetField(ref _personDepartment, value, nameof(PersonDepartment));
        }

        private bool _hasErrors;
        public bool HasErrors
        {
            private get => _hasErrors;
            set
            {
                _hasErrors = value;
                SetField(ref _hasErrors, value, nameof(HasErrors));
                OnPropertyChanged(nameof(IsButtonEnable)); 
            }
        }

        public bool IsButtonEnable => !HasErrors;

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

        /// <summary>
        ///     Error indexer
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        protected virtual string ValidateProperty(string columnName)
        {
            {
                var error = string.Empty;

                switch (columnName)
                {
                    case nameof(PersonName):
                        if (string.IsNullOrEmpty(PersonName))
                        {
                            error = UiErrorResource.RegistrationName;
                        }

                        break;

                    case nameof(PersonLastName):
                        if (string.IsNullOrEmpty(PersonLastName))
                        {
                            error = UiErrorResource.RegistrationLastName;
                        }

                        break;

                    case nameof(PersonDepartment):
                        if (string.IsNullOrEmpty(PersonDepartment))
                        {
                            error = UiErrorResource.RegistrationDepartment;
                        }

                        break;
                }

                CheckFieldErrors(columnName, error);
                return error;
            }
        }

        protected void CheckFieldErrors(string columnName, string error)
        {
            if (string.IsNullOrEmpty(error) && errors.ContainsKey(columnName))
            {
                errors.Remove(columnName);
            }
            else if (!string.IsNullOrEmpty(error))
            {
                errors[columnName] = error;
            }

            HasErrors = errors.Count > 0;
        }
    }
}