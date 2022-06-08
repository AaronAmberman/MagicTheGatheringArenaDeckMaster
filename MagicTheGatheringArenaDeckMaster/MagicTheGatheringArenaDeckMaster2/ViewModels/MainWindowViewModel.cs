using MagicTheGatheringArena.Core.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using WPF.InternalDialogs;

namespace MagicTheGatheringArenaDeckMaster2.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<UniqueArtTypeViewModel> cards;
        private PopupDialogViewModel popupDialogViewModel;
        private ObservableCollection<string> setNames;
        private ObservableCollection<string> standardOnlySetNames;
        private string statusMessage = "Ready";

        #endregion

        #region Properties

        public ObservableCollection<UniqueArtTypeViewModel> Cards
        {
            get => cards;
            set
            {
                cards = value;
                OnPropertyChanged();
            }
        }

        public PopupDialogViewModel PopupDialogViewModel
        {
            get => popupDialogViewModel;
            set
            {
                popupDialogViewModel = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> SetNames
        {
            get => setNames;
            set
            {
                setNames = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> StandardOnlySetNames
        {
            get => standardOnlySetNames;
            set
            {
                standardOnlySetNames = value;
                OnPropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => statusMessage;
            set
            {
                statusMessage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        public void ClearOutProgressDialog()
        {
            PopupDialogViewModel.ProgressViewModel.ProgressDialogVisbility = Visibility.Collapsed;
            PopupDialogViewModel.ProgressViewModel.ProgressIsIndeterminate = false;
            PopupDialogViewModel.ProgressViewModel.ProgressMessage = string.Empty;
            PopupDialogViewModel.ProgressViewModel.ProgressTitle = string.Empty;
            PopupDialogViewModel.ProgressViewModel.ProgressValue = 0.0;
            PopupDialogViewModel.ProgressViewModel.ProgressMax = 1.0;
            PopupDialogViewModel.ProgressViewModel.ProgressMin = 0.0;
        }

        public void ClearOutMessageBoxDialog()
        {
            PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = string.Empty;
            PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = string.Empty;
            PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = false;
            PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
            PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.Information;
            PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Collapsed;
        }

        public void ResetStatusMessage10Seconds()
        {
            // wait 10 seconds then set back to "Ready"
            Task.Delay(10000).ContinueWith(task =>
            {
                if (!StatusMessage.Equals("Ready", StringComparison.OrdinalIgnoreCase))
                {
                    StatusMessage = "Ready";
                }
            });
        }

        #endregion
    }
}
