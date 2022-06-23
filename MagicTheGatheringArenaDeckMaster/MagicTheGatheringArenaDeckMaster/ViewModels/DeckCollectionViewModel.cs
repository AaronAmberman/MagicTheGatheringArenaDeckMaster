using MagicTheGatheringArena.Core.Database.Models;
using MagicTheGatheringArena.Core.MVVM;
using MagicTheGatheringArena.Core.Types;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        private ICommand editCommand;
        private ICommand exportCommand;
        private ICommand importCommand;
        private ICommand removeCommand;
        private string searchText;
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

        public ICommand EditCommand => editCommand ??= new RelayCommand(EditDeck);

        public ICommand ExportCommand => exportCommand ??= new RelayCommand(ExportDeck);

        public ICommand ImportCommand => importCommand ??= new RelayCommand(ImportDeck);

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

        #endregion

        #region Methods

        private void AddDeck()
        {
            ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.Clear();
            ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.Deck = new Deck();

            DeckBuilderWindow window = new DeckBuilderWindow();
            window.DataContext = ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel;

            ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.CloseAction = window.Close;

            window.Closing += Window_Closing;
            window.Closed += Window_Closed;
            window.Show();

            // disable deck tab so users cannot navigate to it
            ServiceLocator.Instance.MainWindowViewModel.IsDeckTabEnabled = false;

            // show card collection tab again so users can double click on cards to add them
            ServiceLocator.Instance.MainWindowViewModel.SelectedTabControlIndex = 0;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility == Visibility.Visible)
            {
                // the user hit the red X at the top twice so we'll take that as a cancel
                ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.HasChanges = false;
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
                        Debug.WriteLine("Save changes to database");
                    }

                    ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.HasChanges = false;
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // enable deck tab so users can navigate to it (regardless of what happens in this deck editor window; cancel or saved)
            ServiceLocator.Instance.MainWindowViewModel.IsDeckTabEnabled = true;

            ServiceLocator.Instance.MainWindowViewModel.StatusMessage = "Viewing card collection";
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
