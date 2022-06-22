using MagicTheGatheringArena.Core.MVVM;
using MagicTheGatheringArena.Core.Types;
using MagicTheGatheringArenaDeckMaster.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;

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
            DeckBuilderWindow window = new DeckBuilderWindow();
            window.DataContext = ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel;
            window.Closed += Window_Closed;
            window.Show();

            // disable deck tab so users cannot navigate to it
            ServiceLocator.Instance.MainWindowViewModel.IsDeckTabEnabled = false;

            // show card collection tab again so users can double click on cards to add them
            ServiceLocator.Instance.MainWindowViewModel.SelectedTabControlIndex = 0;
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
