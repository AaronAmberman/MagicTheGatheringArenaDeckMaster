using MagicTheGatheringArena.Core.MVVM;
using MagicTheGatheringArena.Core.Scryfall.Data;
using MagicTheGatheringArenaDeckMaster.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF.InternalDialogs;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class DataViewModel : ViewModelBase
    {
        #region Fields

        private ICommand downloadDataCommand;
        private ICommand offlineModeCommand;
        private Visibility offlineModeVisibility = Visibility.Collapsed;
        private Visibility visibility = Visibility.Visible;

        #endregion

        #region Properties

        public ICommand DownloadDataCommand => downloadDataCommand ??= new RelayCommand(DownloadData);

        public ICommand OfflineModeCommand => offlineModeCommand ??= new RelayCommand(GoOffline);

        public Visibility OfflineModeVisibility
        {
            get => offlineModeVisibility;
            set
            {
                offlineModeVisibility = value;
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

        private void DownloadData()
        {
            Visibility = Visibility.Collapsed;

            ServiceLocator.Instance.MainWindowViewModel.StatusMessage = "Downloading data...";

            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.ProgressViewModel.ProgressTitle = "Downloading Data";
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.ProgressViewModel.ProgressMessage = "Downloading data from Scryfall.com...";
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.ProgressViewModel.ProgressIsIndeterminate = true;
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.ProgressViewModel.ProgressDialogVisbility = Visibility.Visible;

            Task.Run(() =>
            {
                return ServiceLocator.Instance.ScryfallService.GetUniqueArtworkUri();
            }).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    BulkDataType result = task.Result;

                    if (result == null)
                    {
                        ServiceLocator.Instance.MainWindowViewModel.Dispatcher.Invoke(() => 
                        {
                            ServiceLocator.Instance.MainWindowViewModel.StatusMessage = "Error downloading data.";
                            ServiceLocator.Instance.MainWindowViewModel.SetStatusMessageOnDelay("Ready", 10000);

                            ServiceLocator.Instance.MainWindowViewModel.ClearOutProgressDialog();

                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "Could not download information from Scryfall.com. See log for details.";
                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Download Failure";
                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true;
                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                            ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                            // show the need data screen again
                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.DataViewModel.Visibility = Visibility.Visible;
                        });
                    }
                    else
                    {
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.ProgressViewModel.ProgressMessage = "Downloading additional data from Scryfall.com...";

                        if (File.Exists(ServiceLocator.Instance.PathingService.CardDataFile))
                            File.Delete(ServiceLocator.Instance.PathingService.CardDataFile);

                        ServiceLocator.Instance.ScryfallService.DownloadUniqueArtworkFile(result, ServiceLocator.Instance.PathingService.CardDataFile);

                        ServiceLocator.Instance.MainWindowViewModel.ClearOutProgressDialog();

                        if (File.Exists(ServiceLocator.Instance.PathingService.CardDataFile)) // successful downloaded
                        {
                            ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                            ProcessFile();
                        }
                        else
                        {
                            ServiceLocator.Instance.MainWindowViewModel.Dispatcher.Invoke(() =>
                            {
                                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "Could not download data from Scryfall.com. Please see the log for more details.";
                                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Error Downloading Data";
                                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true;
                                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                                ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                                // show the need data screen again
                                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.DataViewModel.Visibility = Visibility.Visible;
                            });
                        }
                    }
                }
            }).ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    ServiceLocator.Instance.MainWindowViewModel.Dispatcher.Invoke(() =>
                    {
                        ServiceLocator.Instance.MainWindowViewModel.StatusMessage = "Error downloading data.";
                        ServiceLocator.Instance.MainWindowViewModel.SetStatusMessageOnDelay("Ready", 10000);

                        ServiceLocator.Instance.LoggerService.Error($"An error occurred downloaded data from Scryfall.com.{Environment.NewLine}{task.Exception}");

                        ServiceLocator.Instance.MainWindowViewModel.ClearOutProgressDialog();

                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = $"An error occurred downloading data from Scryfall.com. {task.Exception.Message}{Environment.NewLine}See the log for more details.";
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Error Downloading";
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true;
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                        ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                        // show the need data screen again
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.DataViewModel.Visibility = Visibility.Visible;
                    });
                }
            });
        }

        private void GoOffline()
        {
            Visibility = Visibility.Collapsed;

            ProcessFile();
        }

        private void ProcessFile()
        {
            ServiceLocator.Instance.MainWindowViewModel.StatusMessage = "Processing data...";

            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.ProgressViewModel.ProgressTitle = "Processing Data";
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.ProgressViewModel.ProgressMessage = "Processing data from Scryfall.com...";
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.ProgressViewModel.ProgressIsIndeterminate = true;
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.ProgressViewModel.ProgressDialogVisbility = Visibility.Visible;

            Task.Run(() => 
            {
                string text = File.ReadAllText(ServiceLocator.Instance.PathingService.CardDataFile);

                List<UniqueArtType> cards = JsonConvert.DeserializeObject<List<UniqueArtType>>(text);

                // get distinct card names within each set
                List<UniqueArtType> uniqueCards = new();

                var groupedBySet = cards?.GroupBy(c => c.set_name);

                if (groupedBySet != null)
                {
                    foreach (var group in groupedBySet)
                    {
                        var nameGrouped = group.GroupBy(c => c.name_field).ToList();

                        foreach (var nameGroup in nameGrouped)
                        {
                            uniqueCards.Add(nameGroup.First());
                        }
                    }
                }

                uniqueCards = uniqueCards.Where(card => card.image_uris != null).ToList();
                uniqueCards = uniqueCards.OrderBy(uat => uat.set_name).ThenBy(uat => uat.name_field).ToList();

                // all set names
                ServiceLocator.Instance.MainWindowViewModel.SetNames = 
                    new ObservableCollection<string>(uniqueCards.Select(c => c.set_name).Distinct().ToList());

                // standard set names             
                ServiceLocator.Instance.MainWindowViewModel.StandardOnlySetNames = new ObservableCollection<string>(
                    uniqueCards.Where(c => c.legalities.standard == "legal").Select(c => c.set_name).Distinct().ToList());

                // get a collection to verify the paths
                List<UniqueArtTypeViewModel> cardVerificationCollection = new List<UniqueArtTypeViewModel>(uniqueCards.Select(c => new UniqueArtTypeViewModel(c)).ToList());

                // build our vm representations
                ServiceLocator.Instance.MainWindowViewModel.Cards = new CardCollection();
                ServiceLocator.Instance.MainWindowViewModel.Cards.AddMany(cardVerificationCollection);

                // check to see if we have already downloaded cards
                List<string> setPaths = Directory.GetDirectories(ServiceLocator.Instance.PathingService.CardImagePath).ToList();

                foreach (string setPath in setPaths)
                {
                    List<UniqueArtTypeViewModel> setCards = cardVerificationCollection
                        .Where(card => card.Model.set_name.ReplaceBadWindowsCharacters() == setPath.Substring(setPath.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1))
                        .ToList();

                    // loop through all our matching cards
                    foreach (UniqueArtTypeViewModel card in setCards)
                    {
                        // get all the paths in our set directory
                        List<string> cardPaths = Directory.GetFiles(setPath).ToList();

                        foreach (string cardPath in cardPaths)
                        {
                            string cardName = card.Model.name_field.ReplaceBadWindowsCharacters();
                            string cardNameFromPath = Path.GetFileNameWithoutExtension(cardPath);

                            // if the card directory name matches the last part of our path (the card name part)
                            if (cardName == cardNameFromPath)
                            {
                                card.ImagePath = cardPath;
                            }
                        }
                    }
                }

                cardVerificationCollection.Clear();
            }).ContinueWith(task => 
            {
                if (task.Exception != null)
                {
                    ServiceLocator.Instance.MainWindowViewModel.Dispatcher.Invoke(() =>
                    {
                        ServiceLocator.Instance.MainWindowViewModel.StatusMessage = "Error processing data.";
                        ServiceLocator.Instance.MainWindowViewModel.SetStatusMessageOnDelay("Ready", 10000);

                        ServiceLocator.Instance.LoggerService.Error($"An error occurred processing data from Scryfall.com.{Environment.NewLine}{task.Exception}");

                        ServiceLocator.Instance.MainWindowViewModel.ClearOutProgressDialog();

                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = $"An error occurred processing data from Scryfall.com. {task.Exception.Message}{Environment.NewLine}See the log for more details.";
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Error Processing";
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true;
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                        ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                        // show the need data screen again
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.DataViewModel.Visibility = Visibility.Visible;
                    });
                }
                else
                {
                    ServiceLocator.Instance.MainWindowViewModel.ClearOutProgressDialog();
                    ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                    ServiceLocator.Instance.MainWindowViewModel.StatusMessage = "Data has been processed.";
                    ServiceLocator.Instance.MainWindowViewModel.SetStatusMessageOnDelay("Ready", 10000);

                    ServiceLocator.Instance.MainWindowViewModel.SetFilterMessageVisibility = Visibility.Visible;
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SetFilterViewModel.ShowSetFilter();
                }
            });
        }

        #endregion
    }
}
