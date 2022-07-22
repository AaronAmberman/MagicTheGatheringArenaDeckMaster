using MagicTheGatheringArena.Core.MVVM;
using MagicTheGatheringArenaDeckMaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WPF.InternalDialogs;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class CardDownloadViewModel : ViewModelBase
    {
        #region Fields

        private string activeCard;
        private string activeSet;
        private int downloadCount;
        private int downloadTotal;
        private List<SetFilter> setFilters;
        private int setDownloadCount;
        private int setDownloadTotal;
        private Visibility visibility = Visibility.Collapsed;

        #endregion

        #region Properties

        public string ActiveCard
        { 
            get => activeCard; 
            set
            {
                activeCard = value;
                OnPropertyChanged();
            }
        }

        public string ActiveSet
        {
            get => activeSet;
            set
            {
                activeSet = value;
                OnPropertyChanged();
            }
        }

        public int DownloadCount
        {
            get => downloadCount;
            set
            {
                downloadCount = value;
                OnPropertyChanged();
            }
        }

        public int DownloadTotal
        {
            get => downloadTotal;
            set
            {
                downloadTotal = value;
                OnPropertyChanged();
            }
        }

        public List<SetFilter> SetFilters
        {
            get => setFilters;
            set
            {
                setFilters = value;
                OnPropertyChanged();
            }
        }

        public int SetDownloadCount
        {
            get => setDownloadCount;
            set
            {
                setDownloadCount = value;
                OnPropertyChanged();
            }
        }

        public int SetDownloadTotal
        {
            get => setDownloadTotal;
            set
            {
                setDownloadTotal = value;
                OnPropertyChanged();
            }
        }

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

        #region Methods

        public void BeingDownloading()
        {
            // get our total download count (means we have to iterate twice...oh well)
            foreach (SetFilter setFilter in SetFilters)
            {
                try
                {
                    int setCount = ServiceLocator.Instance.MainWindowViewModel.Cards[setFilter.Name].Count;
                    DownloadTotal += setCount;

                    string setDir = Path.Combine(ServiceLocator.Instance.PathingService.CardImagePath, setFilter.Name);
                    
                    if (Directory.Exists(setDir))
                    {
                        // then let's take away the cards we have already downloaded in case something errored out
                        List<string> existingCards = Directory.GetFiles(setDir).ToList();

                        DownloadTotal -= existingCards.Count;
                    }
                }
                catch (Exception ex)
                {
                    ServiceLocator.Instance.LoggerService.Error($"An error occurred attempting to count files.{Environment.NewLine}{ex}");

                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Error Counting Files";
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = $"An error occurred attempting to count files for set {setFilter.Name}. See log for details.";
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true;
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                    ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                    continue;
                }
            }

            Task.Run(() =>
            {
                foreach (SetFilter setFilter in SetFilters)
                {
                    ActiveSet = setFilter.Name;
                    SetDownloadCount = 0;
                    SetDownloadTotal = 0;

                    List<UniqueArtTypeViewModel> cards = ServiceLocator.Instance.MainWindowViewModel.Cards[setFilter.Name];

                    SetDownloadTotal = cards.Count;

                    string setDir = Path.Combine(ServiceLocator.Instance.PathingService.CardImagePath, setFilter.Name);

                    if (Directory.Exists(setDir))
                    {
                        // then let's take away the cards we have already downloaded in case something errored out
                        List<string> existingCards = Directory.GetFiles(setDir).ToList();

                        SetDownloadTotal -= existingCards.Count;
                    }

                    foreach (UniqueArtTypeViewModel card in cards)
                    {
                        ActiveCard = card.Name;

                        string setPath = Path.Combine(ServiceLocator.Instance.PathingService.CardImagePath, card.Set.ReplaceBadWindowsCharacters());

                        // lets check to see if the card exists and if it does then don't download it
                        string cardPath = Path.Combine(setPath, card.Name.ReplaceBadWindowsCharacters() + ".png");

                        if (!File.Exists(cardPath))
                        {
                            if (!ServiceLocator.Instance.ScryfallService.DownloadArtworkFile(card.Model, setPath))
                            {
                                ServiceLocator.Instance.MainWindowViewModel.StatusMessage = $"Failed to downloaded {card.Name}. See log for more details.";
                            }

                            DownloadCount++;
                            SetDownloadCount++;
                        }
                    }
                }
            }).ContinueWith(task => 
            {
                if (task.Exception != null)
                {
                    ServiceLocator.Instance.MainWindowViewModel.Dispatcher.Invoke(() =>
                    {
                        ServiceLocator.Instance.LoggerService.Error($"An error occurred attempting to download image files.{Environment.NewLine}{task.Exception}");

                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Error Downloading Images";
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = $"An error occurred attempting to download card image files. See log for details.";
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true;
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                        ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();
                    });
                }
                else
                {
                    ServiceLocator.Instance.MainWindowViewModel.StatusMessage = "All cards have been downloaded. For any errors please see the log file.";
                    ServiceLocator.Instance.MainWindowViewModel.SetStatusMessageOnDelay(
                        ServiceLocator.Instance.MainWindowViewModel.IsDeckTabButtonsEnabled 
                            ? "Viewing card collection" 
                            : "Creating deck", 
                        7000);

                    ActiveCard = string.Empty;
                    ActiveSet = string.Empty;
                    DownloadCount = 0;
                    DownloadTotal = 0;
                    SetDownloadCount = 0;
                    SetDownloadTotal = 0;
                    SetFilters.Clear();
                    Visibility = Visibility.Collapsed;

                    ServiceLocator.Instance.MainWindowViewModel.CardCollectionViewModel.Cards.Clear();

                    List<UniqueArtTypeViewModel> cards = new List<UniqueArtTypeViewModel>();

                    foreach (SetFilter setFilter in SetFilters)
                    {
                        cards.AddRange(ServiceLocator.Instance.MainWindowViewModel.Cards[setFilter.Name, false]);

                        //ServiceLocator.Instance.MainWindowViewModel.CardCollectionViewModel.Cards.AddRange(ServiceLocator.Instance.MainWindowViewModel.Cards[setFilter.Name, false]);
                        //ServiceLocator.Instance.MainWindowViewModel.CardCollectionViewModel.Cards.AddRange(ServiceLocator.Instance.MainWindowViewModel.Cards[setFilter.Name, true]);
                    }

                    // sort the collection going to the UI
                    cards = cards.OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList();

                    ServiceLocator.Instance.MainWindowViewModel.CardCollectionViewModel.Cards.AddRange(cards);
                }
            });
        }

        #endregion
    }
}
