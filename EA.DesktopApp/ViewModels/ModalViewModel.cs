﻿using System.Windows.Input;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Services;
using EA.DesktopApp.ViewModels.Commands;

namespace EA.DesktopApp.ViewModels
{
    public class ModalViewModel : BaseViewModel
    {
        private readonly IWindowManager _windowManager;
        private readonly ISoundPlayerService _soundPlayerHelper;
        private string _warningText;

        /// <summary>
        ///     .ctor
        /// </summary>
        /// <param name="windowManager"></param>
        /// <param name="initialMessage"></param>
        public ModalViewModel(IWindowManager windowManager, ISoundPlayerService soundPlayerHelper, string initialMessage)
        {
            _windowManager = windowManager;
            _soundPlayerHelper = soundPlayerHelper;
            WarningText = initialMessage;
            InitializeCommands();
        }

        /// <summary>
        ///     Property for message binding to text box
        ///     in View
        /// </summary>
        public string WarningText
        {
            get => _warningText;
            set
            {
                _warningText = value;
                SetField(ref _warningText, value);
            }
        }

        /// <summary>
        ///     Relay command for OK button execute
        /// </summary>
        public ICommand ToggleOkButtonCommand { get; set; }

        /// <summary>
        ///     Initialize relay command
        /// </summary>
        private void InitializeCommands()
        {
            ToggleOkButtonCommand = new RelayCommand(CloseWindowExecute);
        }

        /// <summary>
        ///     this.Close
        /// </summary>
        private void CloseWindowExecute()
        {
            _soundPlayerHelper.PlaySound(SoundPlayerService.ButtonSound);
            _windowManager.CloseModalWindow();
        }
    }
}