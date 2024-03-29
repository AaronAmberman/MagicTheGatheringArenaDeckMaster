﻿using MagicTheGatheringArenaDeckMaster.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF.InternalDialogs;

namespace MagicTheGatheringArenaDeckMaster
{
    public partial class MainWindow : Window
    {
        MainWindowViewModel viewModel;

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Instance.LoggerService.LogFile = Path.Combine(ServiceLocator.Instance.PathingService.BaseDataPath, "Logs", "DeckMaster.log");

            viewModel = new MainWindowViewModel
            {
                Dispatcher = Dispatcher,
                CardCollectionViewModel = new CardCollectionViewModel
                {
                    CardListBox = cardListBox
                },
                DeckCollectionViewModel = new DeckCollectionViewModel(),
                PopupDialogViewModel = new PopupDialogViewModel
                {
                    AboutDialogViewModel = new AboutDialogViewModel(),
                    CardDownloadViewModel = new CardDownloadViewModel(),
                    DataViewModel = new DataViewModel(),
                    MessageBoxViewModel = new MessageBoxViewModel(),
                    ProgressViewModel = new ProgressViewModel(),
                    SetFilterViewModel = new SetFilterViewModel(),
                    SettingsViewModel = new SettingsViewModel(),
                }
            };

            ServiceLocator.Instance.MainWindowViewModel = viewModel;

            try
            {
                viewModel.PopupDialogViewModel.AboutDialogViewModel.Version = Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred attempting to get the version information.{Environment.NewLine}{ex}");
                ServiceLocator.Instance.LoggerService.Error($"An error occurred attempting to get the version information.{Environment.NewLine}{ex}");

                viewModel.PopupDialogViewModel.AboutDialogViewModel.Version = "Unable to retrieve version information.";
            }

            try
            {
                // if we have a previous card data file, enable offline mode
                if (File.Exists(ServiceLocator.Instance.PathingService.CardDataFile))
                {
                    viewModel.PopupDialogViewModel.DataViewModel.OfflineModeVisibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred attempting to determine if an existing card data file existed.{Environment.NewLine}{ex}");
                ServiceLocator.Instance.LoggerService.Error($"An error occurred attempting to determine if an existing card data file existed.{Environment.NewLine}{ex}");
            }

            DataContext = viewModel;

            if (!ServiceLocator.Instance.PathingService.EnsureDirectories(ServiceLocator.Instance.LoggerService))
            {
                viewModel.StatusMessage = "Error. Exiting application";
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Needed Directories Missing";
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "Could not create the needed directories for the application to run. Exiting.";
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true;
                viewModel.PopupDialogViewModel.MessageBoxViewModel.CloseAction = () => 
                {
                    Application.Current.Shutdown(-1);
                };
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                return;
            }

            ServiceLocator.Instance.DatabaseService.ConnectionString = $"Data Source={ServiceLocator.Instance.PathingService.DatabaseFile}";

            if (!ServiceLocator.Instance.DatabaseService.EnsureDatabase(ServiceLocator.Instance.LoggerService))
            {
                viewModel.StatusMessage = "Error. Exiting application";
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Database Initialization Error";
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "Unable to ensure the existence of the database or create the tables. Exiting.";
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true;
                viewModel.PopupDialogViewModel.MessageBoxViewModel.CloseAction = () =>
                {
                    Application.Current.Shutdown(-1);
                };
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                return;
            }

            viewModel.PopupDialogViewModel.DataViewModel.Visibility = Visibility.Visible;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // the ListView on the deck page is firing this selection changed event alongn with
            // its own when a deck is selected make sure our source is the tab control...
            // wonder why it does this? they are separate selection changed events
            if (e.OriginalSource != tabControl) return;

            if (tabControl.SelectedIndex == 0 && ServiceLocator.Instance.MainWindowViewModel != null)
            {
                if (ServiceLocator.Instance.MainWindowViewModel.DeckCollectionViewModel.IsDeckTabButtonsEnabled)
                    ServiceLocator.Instance.MainWindowViewModel.StatusMessage = "Viewing card collection";
                else
                    ServiceLocator.Instance.MainWindowViewModel.StatusMessage = "Creating deck";
            }
            else if (tabControl.SelectedIndex == 1 && ServiceLocator.Instance.MainWindowViewModel != null)
            {
                ServiceLocator.Instance.MainWindowViewModel.StatusMessage = "Viewing decks";
                ServiceLocator.Instance.MainWindowViewModel.DeckCollectionViewModel.QueryDatabaseForDecks();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel == null) return;

            if (ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.HasChanges)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Save Pending Changes";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "There are unsaved changes in the deck. Would you like to save now?";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.YesNo;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.Help;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true; // prevents closing
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                if (ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (!ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.SaveDeck())
                    {
                        ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Error Saving Changes";
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "The deck could not be saved to the database. Please see log for details.";
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true; // prevents closing
                        ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;
                    }
                }

                ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            /*
             * If the closing event of the deck builder window prevents execution because of a modal dialog then we 
             * want to forcefully kill it by telling the environment to quit.
             */

            Environment.Exit(0);
        }

        private void CardCollection_CardImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // add card to deck
            if (!ServiceLocator.Instance.MainWindowViewModel.DeckCollectionViewModel.IsDeckTabButtonsEnabled) // this means we are creating a deck
            {
                Image image = sender as Image;
                UniqueArtTypeViewModel vm = image.DataContext as UniqueArtTypeViewModel;

                if (vm == null) return;

                /*
                 * we want to see if the card name and the card set are the same, if so...increment count, if not add new card
                 */
                UniqueArtTypeViewModel match = ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.Cards.FirstOrDefault(card => card.Name == vm.Name && card.Set == vm.Set);

                if (match == null)
                {
                    ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.Cards.Add(vm.Clone()); // add new instance with same data
                }
                else
                {
                    match.DeckBuilderDeckCount++;

                    ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.FireCollectionChanged();
                }
            }
        }

        private void CardCollection_CardImage_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;
            UniqueArtTypeViewModel vm = image.DataContext as UniqueArtTypeViewModel;

            if (vm == null) return;

            ServiceLocator.Instance.MainWindowViewModel.BigCardImage = new BitmapImage();
            ServiceLocator.Instance.MainWindowViewModel.BigCardImage.BeginInit();
            ServiceLocator.Instance.MainWindowViewModel.BigCardImage.CacheOption = BitmapCacheOption.None;
            ServiceLocator.Instance.MainWindowViewModel.BigCardImage.DecodePixelWidth = 521;
            ServiceLocator.Instance.MainWindowViewModel.BigCardImage.UriSource = new Uri(vm.ImagePath);
            ServiceLocator.Instance.MainWindowViewModel.BigCardImage.EndInit();

            ServiceLocator.Instance.MainWindowViewModel.BigCardViewVisibility = Visibility.Visible;
        }

        private void InternalDialog_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ServiceLocator.Instance.MainWindowViewModel.BigCardViewVisibility = Visibility.Collapsed;

            // encourage the carbage collector
            GC.Collect();
        }

        #endregion
    }
}
