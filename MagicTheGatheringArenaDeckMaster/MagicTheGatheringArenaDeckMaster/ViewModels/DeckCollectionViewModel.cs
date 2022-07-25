using MagicTheGatheringArena.Core.Database.Models;
using MagicTheGatheringArena.Core.MVVM;
using MagicTheGatheringArena.Core.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WPF.InternalDialogs;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class DeckCollectionViewModel : ViewModelBase
    {
        #region Fields

        private ICommand addCommand;
        private ICommand copyCommand;
        private ObservableCollection<Deck> decks = new ObservableCollection<Deck>();
        private CollectionView deckCollectionView;
        private Visibility deckBusyVisibility = Visibility.Collapsed;
        private ICommand editCommand;
        private ICommand exportCommand;
        private ICommand importCommand;
        private bool isDeckTabButtonsEnabled = true;
        private ICommand removeCommand;
        private string searchText;
        private Deck selectedDeck;
        private SingleShotTimer timer;

        #endregion

        #region Properties

        public ICommand AddCommand => addCommand ??= new RelayCommand(AddDeck);

        public ICommand CopyCommand => copyCommand ??= new RelayCommand(CopyDeck);

        public ObservableCollection<Deck> Decks
        {
            get => decks;
            set
            {
                decks = value;
                OnPropertyChanged();
            }
        }

        public Visibility DeckBusyVisibility
        {
            get => deckBusyVisibility;
            set
            {
                deckBusyVisibility = value;
                OnPropertyChanged();
            }
        }

        public ICommand EditCommand => editCommand ??= new RelayCommand(EditDeck);

        public ICommand ExportCommand => exportCommand ??= new RelayCommand(ExportDeck);

        public ICommand ImportCommand => importCommand ??= new RelayCommand(ImportDeck);

        public bool IsDeckTabButtonsEnabled
        {
            get => isDeckTabButtonsEnabled;
            set
            {
                isDeckTabButtonsEnabled = value;
                OnPropertyChanged();
            }
        }

        public ICommand RemoveCommand => removeCommand ??= new RelayCommand(RemoveDeck);

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;

                SetupOrRefreshFilter();

                OnPropertyChanged();
            }
        }

        public Deck SelectedDeck 
        { 
            get => selectedDeck; 
            set
            {
                selectedDeck = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        private void AddDeck()
        {
            // clean if needed (we don't do this on close because of the MainWindow OnClose uses the view model...so only new up/clean up when needing to)
            if (ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel != null)
            {
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.ClearCollectionChanged();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewOneColumnViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewOneColumnViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewThreeColumnColumnOneViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewThreeColumnColumnOneViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewThreeColumnColumnTwoViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewThreeColumnColumnTwoViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewThreeColumnColumnThreeViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewThreeColumnColumnThreeViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnOneViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnOneViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnTwoViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnTwoViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnThreeViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnThreeViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnFourViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnFourViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnFiveViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnFiveViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnSixViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnSixViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnSevenViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnSevenViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnEightViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColumnEightViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnOneViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnOneViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnTwoViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnTwoViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnThreeViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnThreeViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnFourViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnFourViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnFiveViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnFiveViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnSixViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnSixViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnSevenViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnSevenViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnEightViewModel.Cards.Clear();
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CardViewEightColumnColorColumnEightViewModel = null;
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel = null;

                // encourage the garbage collector
                GC.Collect();
            }

            ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel = new DeckBuilderViewModel
            {
                Deck = new Deck(),
                CardViewOneColumnViewModel = new CardColumnViewModel(),
                CardViewThreeColumnColumnOneViewModel = new CardColumnViewModel
                {
                    Header = "Creatures"
                },
                CardViewThreeColumnColumnTwoViewModel = new CardColumnViewModel
                {
                    Header = "Non-Creatures"
                },
                CardViewThreeColumnColumnThreeViewModel = new CardColumnViewModel
                {
                    Header = "Land"
                },
                CardViewEightColumnColumnOneViewModel = new CardColumnViewModel
                {
                    Header = "Creature"
                },
                CardViewEightColumnColumnTwoViewModel = new CardColumnViewModel
                {
                    Header = "Planeswalker"
                },
                CardViewEightColumnColumnThreeViewModel = new CardColumnViewModel
                {
                    Header = "Instant"
                },
                CardViewEightColumnColumnFourViewModel = new CardColumnViewModel
                {
                    Header = "Sorcery"
                },
                CardViewEightColumnColumnFiveViewModel = new CardColumnViewModel
                {
                    Header = "Enchantment"
                },
                CardViewEightColumnColumnSixViewModel = new CardColumnViewModel
                {
                    Header = "Artifact"
                },
                CardViewEightColumnColumnSevenViewModel = new CardColumnViewModel
                {
                    Header = "Commander"
                },
                CardViewEightColumnColumnEightViewModel = new CardColumnViewModel
                {
                    Header = "Land"
                },
                CardViewEightColumnColorColumnOneViewModel = new CardColumnViewModel
                {
                    Header = "White"
                },
                CardViewEightColumnColorColumnTwoViewModel = new CardColumnViewModel
                {
                    Header = "Blue"
                },
                CardViewEightColumnColorColumnThreeViewModel = new CardColumnViewModel
                {
                    Header = "Black"
                },
                CardViewEightColumnColorColumnFourViewModel = new CardColumnViewModel
                {
                    Header = "Red"
                },
                CardViewEightColumnColorColumnFiveViewModel = new CardColumnViewModel
                {
                    Header = "Green"
                },
                CardViewEightColumnColorColumnSixViewModel = new CardColumnViewModel
                {
                    Header = "Artifact"
                },
                CardViewEightColumnColorColumnSevenViewModel = new CardColumnViewModel
                {
                    Header = "Multicolored"
                },
                CardViewEightColumnColorColumnEightViewModel = new CardColumnViewModel
                {
                    Header = "Land"
                }
            };
            ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.SetCounts();

            DeckBuilderWindow window = new DeckBuilderWindow();
            window.DataContext = ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel;

            ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CloseAction = window.Close;

            window.Closing += Window_Closing;
            window.Closed += Window_Closed;
            window.Show();

            // disable deck tab so users cannot navigate to it
            ServiceLocator.Instance.MainWindowViewModel.DeckCollectionViewModel.IsDeckTabButtonsEnabled = false;

            // show card collection tab again so users can double click on cards to add them
            ServiceLocator.Instance.MainWindowViewModel.SelectedTabControlIndex = 0;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility == Visibility.Visible)
            {
                // the user hit the red X at the top twice so we'll take that as a cancel
                ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.Clear();
            }
            else
            {
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
                        if (ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.SaveDeck())
                        {
                            ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Deck Saved";
                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "The deck has been saved to the database.";
                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.Information;
                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true; // prevents closing
                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                            // refresh UI, with data from database
                            ServiceLocator.Instance.MainWindowViewModel.DeckCollectionViewModel.QueryDatabaseForDecks();
                        }
                        else
                        {
                            ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Error Saving Deck";
                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "The deck could not be saved to the database. Please see log for details.";
                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true; // prevents closing
                            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;
                        }
                    }

                    ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                    ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.Clear();
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Window window = sender as Window;

            if (window != null)
            {
                window.Closing -= Window_Closing;
                window.Closed -= Window_Closed;
            }

            // enable deck tab so users can navigate to it (regardless of what happens in this deck editor window; cancel or saved)
            ServiceLocator.Instance.MainWindowViewModel.DeckCollectionViewModel.IsDeckTabButtonsEnabled = true;

            ServiceLocator.Instance.MainWindowViewModel.StatusMessage = "Viewing card collection";

            // encourage the garbage collector
            GC.Collect();
        }

        private void CopyDeck()
        {

        }

        private void EditDeck()
        {

        }

        private void ExportDeck()
        {

        }

        private bool FilterDecks(object obj)
        {
            Deck deck = obj as Deck;

            if (deck == null) return false;

            if (deck.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) return true;

            return false;
        }

        private void ImportDeck()
        {

        }

        public void QueryDatabaseForDecks()
        {
            // this means the task is already running
            if (DeckBusyVisibility == Visibility.Visible) return;

            Task.Run(() =>
            {
                DeckBusyVisibility = Visibility.Visible;

                return ServiceLocator.Instance.DatabaseService.GetDecks();
            }).ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    ServiceLocator.Instance.LoggerService.Error($"An error occurred attempting to query the database for deck info.{Environment.NewLine}{task.Exception}");

                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Error Counting Files";
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = $"An error occurred attempting to query the database. Please see log for details.";
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true;
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                    ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                    DeckBusyVisibility = Visibility.Collapsed;

                    return new List<Deck>();
                }
                else
                {
                    List<Deck> decks = task.Result;

                    foreach (Deck deck in decks)
                    {
                        ServiceLocator.Instance.DatabaseService.GetCardsForDeck(deck);
                    }

                    return decks;
                }
            }).ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    ServiceLocator.Instance.LoggerService.Error($"An error occurred attempting to query the database for deck info.{Environment.NewLine}{task.Exception}");

                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Error Counting Files";
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = $"An error occurred attempting to query the database. Please see log for details.";
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true;
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                    ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();
                }

                List<Deck> decks = task.Result;

                ServiceLocator.Instance.MainWindowViewModel.Dispatcher.Invoke(() =>
                {
                    Decks.Clear();
                    Decks.AddRange(decks);
                });

                // we have all of our deck information, close wait UI
                DeckBusyVisibility = Visibility.Collapsed;
            });
        }

        private void RemoveDeck()
        {

        }

        private void SetupOrRefreshFilter()
        {
            if (timer == null)
            {
                timer = new SingleShotTimer
                {
                    Interval = 1500
                };
            }

            timer.Start(() =>
            {
                ServiceLocator.Instance.MainWindowViewModel.Dispatcher.Invoke(() =>
                {
                    if (deckCollectionView == null)
                    {
                        deckCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(Decks);
                        deckCollectionView.Filter = FilterDecks; // this calls the filter for the first time;
                    }
                    else
                    {
                        deckCollectionView.Refresh();
                    }
                });
            });
        }

        #endregion
    }
}
