using MagicTheGatheringArena.Core.MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPF.InternalDialogs;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class SettingsViewModel : ViewModelBase
    {
        #region Fields

        private ICommand cancelCommand;
        private ICommand okCommand;
        private MessageBoxResult result;
        private ICommand showSettingsCommand;
        private Visibility visibility = Visibility.Collapsed;

        #endregion

        #region Properties

        public ICommand CancelCommand => cancelCommand ??= new RelayCommand(Cancel);

        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok);

        public MessageBoxResult Result
        {
            get => result;
            set
            {
                result = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowSettingsCommand => showSettingsCommand ??= new RelayCommand(ShowSettings);

        public Visibility Visibility
        {
            get => visibility;
            set
            {
                visibility = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        public SettingsViewModel()
        {
        }

        #endregion

        #region Methods

        private void Cancel()
        {
            Result = MessageBoxResult.Cancel;
            Visibility = Visibility.Collapsed;
        }

        private void Ok()
        {
            Result = MessageBoxResult.OK;
            Visibility = Visibility.Collapsed;
        }

        private void ShowSettings()
        {
            Visibility = Visibility.Visible;
        }

        #endregion
    }
}
