using MagicTheGatheringArena.Core.MVVM;
using MagicTheGatheringArenaDeckMaster.Collections;
using MagicTheGatheringArenaDeckMaster.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WPF.InternalDialogs;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private CardCollectionViewModel cardCollectionViewModel;
        private CardCollection cards;
        private ObservableCollection<SetFilter> filterSetNames;
        private PopupDialogViewModel popupDialogViewModel;
        private Visibility setFilterMessageVisibility = Visibility.Collapsed;
        private ObservableCollection<string> setNames;
        private ObservableCollection<string> standardOnlySetNames;
        private string statusMessage = "Ready";

        #endregion

        #region Properties

        public CardCollectionViewModel CardCollectionViewModel
        {
            get => cardCollectionViewModel;
            set
            {
                cardCollectionViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardCollection Cards
        {
            get => cards;
            set
            {
                cards = value;
                OnPropertyChanged();
            }
        }

        public Dispatcher Dispatcher { get; set; }

        public ObservableCollection<SetFilter> FilterSetNames
        {
            get => filterSetNames;
            set
            {
                filterSetNames = value;
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

        public Visibility SetFilterMessageVisibility
        {
            get => setFilterMessageVisibility;
            set
            {
                setFilterMessageVisibility = value;
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

        #region Constructors

        public MainWindowViewModel()
        {
            FilterSetNames = new ObservableCollection<SetFilter>();
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
