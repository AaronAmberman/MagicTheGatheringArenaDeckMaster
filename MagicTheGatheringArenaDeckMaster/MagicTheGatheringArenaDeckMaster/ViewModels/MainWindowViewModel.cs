using MagicTheGatheringArena.Core.MVVM;
using MagicTheGatheringArenaDeckMaster.Collections;
using MagicTheGatheringArenaDeckMaster.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WPF.InternalDialogs;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private BitmapImage bigCardImage;
        private Visibility bigCardView = Visibility.Collapsed;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private CardCollectionViewModel cardCollectionViewModel;
        private CardCollection cards;
        private DeckBuilderViewModel deckBuilderViewModel;
        private DeckCollectionViewModel deckCollectionViewModel;
        private ObservableCollection<SetFilter> filterSetNames;
        private bool isDeckTabEnabled = true;
        private PopupDialogViewModel popupDialogViewModel;
        private int selectedTabControlIndex;
        private Visibility setFilterMessageVisibility = Visibility.Collapsed;
        private ObservableCollection<string> setNames;
        private ObservableCollection<string> standardOnlySetNames;
        private string statusMessage = "Ready";        

        #endregion

        #region Properties

        public BitmapImage BigCardImage
        {
            get => bigCardImage;
            set
            {
                bigCardImage = value;
                OnPropertyChanged();
            }
        }

        public Visibility BigCardViewVisibility
        {
            get => bigCardView;
            set
            {
                bigCardView = value;
                OnPropertyChanged();
            }
        }

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

        public DeckBuilderViewModel DeckBuilderViewModel
        {
            get => deckBuilderViewModel;
            set
            {
                deckBuilderViewModel = value;
                OnPropertyChanged();
            }
        }

        public DeckCollectionViewModel DeckCollectionViewModel
        {
            get => deckCollectionViewModel;
            set
            {
                deckCollectionViewModel = value;
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

        public bool IsDeckTabEnabled
        {
            get => isDeckTabEnabled;
            set
            {
                isDeckTabEnabled = value;
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

        public int SelectedTabControlIndex
        {
            get => selectedTabControlIndex;
            set
            {
                selectedTabControlIndex = value;
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

                if (cts != null)
                {
                    cts.Cancel(); // cancel previous action
                    cts = null;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            FilterSetNames = new ObservableCollection<SetFilter>();

            bigCardImage = new BitmapImage();
            bigCardImage.BeginInit();
            bigCardImage.CacheOption = BitmapCacheOption.None;
            bigCardImage.DecodePixelWidth = 521;
            bigCardImage.UriSource = new Uri("pack://application:,,,/Images/card-back.png");
            bigCardImage.EndInit();
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

        public void SetStatusMessageOnDelay(string message, int millisecondDelay)
        {
            if (cts != null)
            {
                cts.Cancel(); // cancel previous action
                cts = null;
            }

            cts = new CancellationTokenSource();

            Task.Delay(millisecondDelay, cts.Token).ContinueWith(task =>
            {
                if (!task.IsCanceled)
                    StatusMessage = message;
            });
        }

        #endregion
    }
}
