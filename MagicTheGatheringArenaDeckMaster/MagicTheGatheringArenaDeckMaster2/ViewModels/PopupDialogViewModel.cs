using MagicTheGatheringArena.Core.MVVM;

namespace MagicTheGatheringArenaDeckMaster2.ViewModels
{
    internal class PopupDialogViewModel : ViewModelBase
    {
        #region Fields

        private AddSetToSettingsViewModel addSetToSettingsViewModel;
        private AboutDialogViewModel aboutDialogViewModel;
        private CardDownloadViewModel cardDownloadViewModel;
        private DataViewModel dataViewModel;
        private MessageBoxViewModel messageBoxViewModel;
        private ProgressViewModel progressViewModel;
        private SetFilterViewModel setFilterViewModel;
        private SettingsViewModel settingsViewModel;

        #endregion

        #region Properties

        public AddSetToSettingsViewModel AddSetToSettingsViewModel
        {
            get => addSetToSettingsViewModel;
            set
            {
                addSetToSettingsViewModel = value;
                OnPropertyChanged();
            }
        }

        public AboutDialogViewModel AboutDialogViewModel
        {
            get => aboutDialogViewModel;
            set
            {
                aboutDialogViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardDownloadViewModel CardDownloadViewModel
        {
            get => cardDownloadViewModel;
            set
            {
                cardDownloadViewModel = value;
                OnPropertyChanged();
            }
        }

        public DataViewModel DataViewModel 
        { 
            get => dataViewModel; 
            set
            {
                dataViewModel = value;
                OnPropertyChanged();
            }
        }

        public MessageBoxViewModel MessageBoxViewModel
        {
            get => messageBoxViewModel;
            set
            {
                messageBoxViewModel = value;
                OnPropertyChanged();
            }
        }

        public ProgressViewModel ProgressViewModel
        {
            get => progressViewModel;
            set
            {
                progressViewModel = value;
                OnPropertyChanged();
            }
        }

        public SetFilterViewModel SetFilterViewModel
        {
            get => setFilterViewModel;
            set
            {
                setFilterViewModel = value;
                OnPropertyChanged();
            }
        }

        public SettingsViewModel SettingsViewModel
        {
            get => settingsViewModel;
            set
            {
                settingsViewModel = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
