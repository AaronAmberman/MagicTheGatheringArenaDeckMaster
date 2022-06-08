using MagicTheGatheringArena.Core.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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
