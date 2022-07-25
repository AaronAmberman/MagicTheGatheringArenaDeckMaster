using MagicTheGatheringArena.Core.Database.Models;
using MagicTheGatheringArena.Core.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WPF.InternalDialogs;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class DeckBuilderViewModel : ViewModelBase
    {
        #region Fields

        private ICommand cancelCommand;
        private ObservableCollection<UniqueArtTypeViewModel> cards = new ObservableCollection<UniqueArtTypeViewModel>();
        private CardColumnViewModel cardViewOneColumnViewModel;
        private CardColumnViewModel cardViewThreeColumnColumnOneViewModel;
        private CardColumnViewModel cardViewThreeColumnColumnTwoViewModel;
        private CardColumnViewModel cardViewThreeColumnColumnThreeViewModel;
        private CardColumnViewModel cardViewEightColumnColumnOneViewModel;
        private CardColumnViewModel cardViewEightColumnColumnTwoViewModel;
        private CardColumnViewModel cardViewEightColumnColumnThreeViewModel;
        private CardColumnViewModel cardViewEightColumnColumnFourViewModel;
        private CardColumnViewModel cardViewEightColumnColumnFiveViewModel;
        private CardColumnViewModel cardViewEightColumnColumnSixViewModel;
        private CardColumnViewModel cardViewEightColumnColumnSevenViewModel;
        private CardColumnViewModel cardViewEightColumnColumnEightViewModel;
        private CardColumnViewModel cardViewEightColumnColorColumnOneViewModel;
        private CardColumnViewModel cardViewEightColumnColorColumnTwoViewModel;
        private CardColumnViewModel cardViewEightColumnColorColumnThreeViewModel;
        private CardColumnViewModel cardViewEightColumnColorColumnFourViewModel;
        private CardColumnViewModel cardViewEightColumnColorColumnFiveViewModel;
        private CardColumnViewModel cardViewEightColumnColorColumnSixViewModel;
        private CardColumnViewModel cardViewEightColumnColorColumnSevenViewModel;
        private CardColumnViewModel cardViewEightColumnColorColumnEightViewModel;
        private Deck deck;
        private bool hasChanges;
        private bool isEditing;
        private bool isEightColumnView;
        private bool isEightColorColumnView;
        private bool isOneColumnView = true;
        private bool isThreeColumnView;
        private bool isSavingAlready;
        private ICommand infoCommand;
        private double progressBarMax;
        private ICommand saveCommand;
        private int selectedSetTypeIndex = -1;
        private List<string> setTypes = new List<string>
        {
            "Alchemy",
            "Brawl",
            "Explorer",
            "Historic",
            "Standard",
        };
        private int totalCardCount;
        private double valueOne;
        private double valueTwo;
        private double valueThree;
        private double valueFour;
        private double valueFive;
        private double valueSix;
        private double zoomFactor = 1.0;
        private double zoomMax = 2.0;
        private double zoomMin = 0.5;
        
        #endregion

        #region Properties

        public double AverageManaCost
        {
            get
            {
                double amc = 0.0;
                double totalManaCost = 0.0;
                double totalCardCount = 0.0;

                foreach (var card in Cards)
                {
                    if (!card.Model.type_line.Contains("land", StringComparison.OrdinalIgnoreCase))
                    {
                        totalCardCount += card.DeckBuilderDeckCount;
                        totalManaCost += card.ManaCostTotal * card.DeckBuilderDeckCount;
                    }
                }

                if (totalManaCost > 0.0)
                    amc = totalManaCost / totalCardCount;

                return amc;
            }
        }

        public ICommand CancelCommand => cancelCommand ??= new RelayCommand(Cancel);

        public ObservableCollection<UniqueArtTypeViewModel> Cards
        {
            get => cards;
            set
            {
                cards = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewOneColumnViewModel
        {
            get => cardViewOneColumnViewModel;
            set
            {
                cardViewOneColumnViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewThreeColumnColumnOneViewModel
        {
            get => cardViewThreeColumnColumnOneViewModel;
            set
            {
                cardViewThreeColumnColumnOneViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewThreeColumnColumnTwoViewModel
        {
            get => cardViewThreeColumnColumnTwoViewModel;
            set
            {
                cardViewThreeColumnColumnTwoViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewThreeColumnColumnThreeViewModel
        {
            get => cardViewThreeColumnColumnThreeViewModel;
            set
            {
                cardViewThreeColumnColumnThreeViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColumnOneViewModel
        {
            get => cardViewEightColumnColumnOneViewModel;
            set
            {
                cardViewEightColumnColumnOneViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColumnTwoViewModel
        {
            get => cardViewEightColumnColumnTwoViewModel;
            set
            {
                cardViewEightColumnColumnTwoViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColumnThreeViewModel
        {
            get => cardViewEightColumnColumnThreeViewModel;
            set
            {
                cardViewEightColumnColumnThreeViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColumnFourViewModel
        {
            get => cardViewEightColumnColumnFourViewModel;
            set
            {
                cardViewEightColumnColumnFourViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColumnFiveViewModel
        {
            get => cardViewEightColumnColumnFiveViewModel;
            set
            {
                cardViewEightColumnColumnFiveViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColumnSixViewModel
        {
            get => cardViewEightColumnColumnSixViewModel;
            set
            {
                cardViewEightColumnColumnSixViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColumnSevenViewModel
        {
            get => cardViewEightColumnColumnSevenViewModel;
            set
            {
                cardViewEightColumnColumnSevenViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColumnEightViewModel
        {
            get => cardViewEightColumnColumnEightViewModel;
            set
            {
                cardViewEightColumnColumnEightViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColorColumnOneViewModel
        {
            get => cardViewEightColumnColorColumnOneViewModel;
            set
            {
                cardViewEightColumnColorColumnOneViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColorColumnTwoViewModel
        {
            get => cardViewEightColumnColorColumnTwoViewModel;
            set
            {
                cardViewEightColumnColorColumnTwoViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColorColumnThreeViewModel
        {
            get => cardViewEightColumnColorColumnThreeViewModel;
            set
            {
                cardViewEightColumnColorColumnThreeViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColorColumnFourViewModel
        {
            get => cardViewEightColumnColorColumnFourViewModel;
            set
            {
                cardViewEightColumnColorColumnFourViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColorColumnFiveViewModel
        {
            get => cardViewEightColumnColorColumnFiveViewModel;
            set
            {
                cardViewEightColumnColorColumnFiveViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColorColumnSixViewModel
        {
            get => cardViewEightColumnColorColumnSixViewModel;
            set
            {
                cardViewEightColumnColorColumnSixViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColorColumnSevenViewModel
        {
            get => cardViewEightColumnColorColumnSevenViewModel;
            set
            {
                cardViewEightColumnColorColumnSevenViewModel = value;
                OnPropertyChanged();
            }
        }

        public CardColumnViewModel CardViewEightColumnColorColumnEightViewModel
        {
            get => cardViewEightColumnColorColumnEightViewModel;
            set
            {
                cardViewEightColumnColorColumnEightViewModel = value;
                OnPropertyChanged();
            }
        }

        public Action CloseAction { get; set; }

        public Deck Deck
        {
            get => deck;
            set
            {
                deck = value;
                OnPropertyChanged();
            }
        }

        public bool HasChanges
        {
            get => hasChanges;
            set
            {
                hasChanges = value;
                OnPropertyChanged();
            }
        }

        public ICommand InfoCommand => infoCommand ??= new RelayCommand(ShowInfoWindow);

        public bool IsEditing 
        { 
            get => isEditing; 
            set
            {
                isEditing = value;
                OnPropertyChanged();
            }
        }

        public bool IsEightColumnView
        {
            get => isEightColumnView;
            set
            {
                isEightColumnView = value;
                OnPropertyChanged();
            }
        }

        public bool IsEightColorColumnView
        {
            get => isEightColorColumnView;
            set
            {
                isEightColorColumnView = value;
                OnPropertyChanged();
            }
        }

        public bool IsOneColumnView
        {
            get => isOneColumnView;
            set
            {
                isOneColumnView = value;
                OnPropertyChanged();
            }
        }

        public bool IsThreeColumnView
        {
            get => isThreeColumnView;
            set
            {
                isThreeColumnView = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => Deck.Name;
            set
            {
                Deck.Name = value;

                HasChanges = true;

                OnPropertyChanged();
            }
        }

        public int NumberOfCreatures
        {
            get
            {
                int count = 0;

                foreach (var card in Cards)
                {
                    if (card.Model.type_line.Contains("creature", StringComparison.OrdinalIgnoreCase))
                    {
                        count += card.DeckBuilderDeckCount;
                    }
                }

                return count;
            }
        }

        public int NumberOfNonCreatures
        {
            get
            {
                int count = 0;

                foreach (var card in Cards)
                {
                    if (card.Model.type_line.Contains("creature", StringComparison.OrdinalIgnoreCase) ||
                        (card.Model.type_line.Contains("land", StringComparison.OrdinalIgnoreCase) &&
                        !card.Model.type_line.Equals("artifact land", StringComparison.OrdinalIgnoreCase)))
                        continue;
                    else
                        count += card.DeckBuilderDeckCount;
                }

                return count;
            }
        }

        public int NumberOfLands
        {
            get
            {
                int count = 0;

                foreach (var card in Cards)
                {
                    if (card.Model.type_line.Contains("land", StringComparison.OrdinalIgnoreCase) &&
                        !card.Model.type_line.Equals("artifact land", StringComparison.OrdinalIgnoreCase))
                        count += card.DeckBuilderDeckCount;
                }

                return count;
            }
        }

        public double ProgressBarMax
        {
            get => progressBarMax;
            set
            {
                progressBarMax = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand => saveCommand ??= new RelayCommand(Save);

        public int SelectedSetTypeIndex
        {
            get => selectedSetTypeIndex;
            set
            {
                selectedSetTypeIndex = value;

                HasChanges = true;

                /*
                 * the cards in the card collection view may need to be reset,
                 * for example, if the user chooses Alchemy then we want to load the 
                 * alchemy versions of cards because costs differ
                 */
                // todo

                OnPropertyChanged();
            }
        }

        public List<string> SetTypes => setTypes;

        public int TotalCardCount
        {
            get => totalCardCount;
            set
            {
                totalCardCount = value;
                OnPropertyChanged();
            }
        }

        public double ValueOne
        {
            get => valueOne;
            set
            {
                valueOne = value;
                OnPropertyChanged();
            }
        }

        public double ValueTwo
        {
            get => valueTwo;
            set
            {
                valueTwo = value;
                OnPropertyChanged();
            }
        }

        public double ValueThree
        {
            get => valueThree;
            set
            {
                valueThree = value;
                OnPropertyChanged();
            }
        }

        public double ValueFour
        {
            get => valueFour;
            set
            {
                valueFour = value;
                OnPropertyChanged();
            }
        }

        public double ValueFive
        {
            get => valueFive;
            set
            {
                valueFive = value;
                OnPropertyChanged();
            }
        }

        public double ValueSix
        {
            get => valueSix;
            set
            {
                valueSix = value;
                OnPropertyChanged();
            }
        }

        public double ZoomFactor
        {
            get => zoomFactor;
            set
            {
                if (value < ZoomMin)
                    zoomFactor = ZoomMin;
                else if (value > ZoomMax)
                    zoomFactor = ZoomMax;
                else
                    zoomFactor = value;

                OnPropertyChanged();
            }
        }

        public double ZoomMax
        {
            get => zoomMax;
            set
            {
                zoomMax = value;
                OnPropertyChanged();
            }
        }

        public double ZoomMin
        {
            get => zoomMin;
            set
            {
                zoomMin = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        public DeckBuilderViewModel()
        {
            Cards.CollectionChanged += Cards_CollectionChanged;
        }

        #endregion

        #region Methods

        private void Cancel()
        {
            Action close = CloseAction;

            Clear();

            close();
        }

        private void Cards_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // this view holds all cards in one vertical line (like Arena)
            CardViewOneColumnViewModel?.Cards.Clear();
            CardViewOneColumnViewModel?.Cards.AddRange(Cards.OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());

            // the columns are creatures, non-creautes, lands
            CardViewThreeColumnColumnOneViewModel?.Cards.Clear();
            CardViewThreeColumnColumnTwoViewModel?.Cards.Clear();
            CardViewThreeColumnColumnThreeViewModel?.Cards.Clear();

            CardViewThreeColumnColumnOneViewModel.Cards.AddRange(Cards.Where(x => x.Model.type_line.Contains("creature", StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewThreeColumnColumnThreeViewModel.Cards.AddRange(Cards.Where(x => x.Model.type_line.Contains("land", StringComparison.OrdinalIgnoreCase) && !x.Model.type_line.Equals("artifact land", StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewThreeColumnColumnTwoViewModel.Cards.AddRange(Cards.Except(CardViewThreeColumnColumnOneViewModel.Cards).Except(CardViewThreeColumnColumnThreeViewModel.Cards).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());

            // the eight columns are creature, planeswalker, instant, sorcery, enchantment, artifact, commander, land
            CardViewEightColumnColumnOneViewModel.Cards.Clear();
            CardViewEightColumnColumnTwoViewModel.Cards.Clear();
            CardViewEightColumnColumnThreeViewModel.Cards.Clear();
            CardViewEightColumnColumnFourViewModel.Cards.Clear();
            CardViewEightColumnColumnFiveViewModel.Cards.Clear();
            CardViewEightColumnColumnSixViewModel.Cards.Clear();
            CardViewEightColumnColumnSevenViewModel.Cards.Clear();
            CardViewEightColumnColumnEightViewModel.Cards.Clear();

            CardViewEightColumnColumnOneViewModel.Cards.AddRange(Cards.Where(x => x.Model.type_line.Contains("creature", StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewEightColumnColumnTwoViewModel.Cards.AddRange(Cards.Where(x => x.Model.type_line.Contains("planeswalker", StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewEightColumnColumnThreeViewModel.Cards.AddRange(Cards.Where(x => x.Model.type_line.Contains("instant", StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewEightColumnColumnFourViewModel.Cards.AddRange(Cards.Where(x => x.Model.type_line.Contains("sorcery", StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewEightColumnColumnFiveViewModel.Cards.AddRange(Cards.Where(x => x.Model.type_line.Contains("enchantment", StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewEightColumnColumnSixViewModel.Cards.AddRange(Cards.Where(x => x.Model.type_line.Contains("artifact", StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewEightColumnColumnSevenViewModel.Cards.AddRange(Cards.Where(x => x.Model.type_line.Contains("commander", StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewEightColumnColumnEightViewModel.Cards.AddRange(Cards.Where(x => x.Model.type_line.Contains("land", StringComparison.OrdinalIgnoreCase) && !x.Model.type_line.Equals("artifact land", StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());

            // the eight columns are white, blue, black, red, green, artifact, multicolored, land
            CardViewEightColumnColorColumnOneViewModel.Cards.Clear();
            CardViewEightColumnColorColumnTwoViewModel.Cards.Clear();
            CardViewEightColumnColorColumnThreeViewModel.Cards.Clear();
            CardViewEightColumnColorColumnFourViewModel.Cards.Clear();
            CardViewEightColumnColorColumnFiveViewModel.Cards.Clear();
            CardViewEightColumnColorColumnSixViewModel.Cards.Clear();
            CardViewEightColumnColorColumnSevenViewModel.Cards.Clear();
            CardViewEightColumnColorColumnEightViewModel.Cards.Clear();

            CardViewEightColumnColorColumnOneViewModel.Cards.AddRange(Cards.Where(x => x.ColorScore == 1 && x.Model.colors.Count == 1).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewEightColumnColorColumnTwoViewModel.Cards.AddRange(Cards.Where(x => x.ColorScore == 2 && x.Model.colors.Count == 1).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewEightColumnColorColumnThreeViewModel.Cards.AddRange(Cards.Where(x => x.ColorScore == 3 && x.Model.colors.Count == 1).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewEightColumnColorColumnFourViewModel.Cards.AddRange(Cards.Where(x => x.ColorScore == 4 && x.Model.colors.Count == 1).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewEightColumnColorColumnFiveViewModel.Cards.AddRange(Cards.Where(x => x.ColorScore == 5 && x.Model.colors.Count == 1).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewEightColumnColorColumnSixViewModel.Cards.AddRange(Cards.Where(x => x.ColorScore == 16).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewEightColumnColorColumnSevenViewModel.Cards.AddRange(Cards.Where(x => x.Model.colors.Count > 1).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());
            CardViewEightColumnColorColumnEightViewModel.Cards.AddRange(Cards.Where(x => x.Model.type_line.Contains("land", StringComparison.OrdinalIgnoreCase) && !x.Model.type_line.Equals("artifact land", StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList());

            HasChanges = true;

            SetCounts();
        }

        public void ClearCollectionChanged()
        {
            Cards.CollectionChanged -= Cards_CollectionChanged;
        }

        public void FireCollectionChanged()
        {
            Cards_CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Clear()
        {
            Cards.Clear();

            CloseAction = null;
            Deck = null;
            SelectedSetTypeIndex = -1;

            SetCounts();

            HasChanges = false;
        }

        private List<UniqueArtTypeViewModel> GetCardsForCost(int cost, int searchType)
        {
            List<UniqueArtTypeViewModel> results = new List<UniqueArtTypeViewModel>();

            foreach (UniqueArtTypeViewModel card in Cards)
            {
                if (searchType == 1)
                {
                    if (card.ManaCostTotal >= cost)
                    {
                        results.Add(card);
                    }
                }
                else if (searchType == -1)
                {
                    if (card.ManaCostTotal <= cost)
                    {
                        results.Add(card);
                    }
                }
                else
                {
                    if (card.ManaCostTotal == cost)
                    {
                        results.Add(card);
                    }
                }
            }

            return results;
        }

        private void Save()
        {
            if (isSavingAlready) return;

            isSavingAlready = true;

            ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

            if (string.IsNullOrWhiteSpace(Name))
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Missing Data";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "The deck needs a name. Please enter a name.";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.Information;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                isSavingAlready = false;

                return;
            }

            if (SelectedSetTypeIndex == -1)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Missing Data";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "The deck needs to have a game type specified. Please select one from the drop down.";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.Information;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                isSavingAlready = false;

                return;
            }

            if (!ServiceLocator.Instance.DatabaseService.IsDeckNameUnique(Name, Deck.Id <= 0 ? -1 : Deck.Id))
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Conflicting Name";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "The deck name needs to be unique. Please enter a unique deck name.";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                isSavingAlready = false;

                return;
            }

            ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

            switch (SelectedSetTypeIndex)
            {
                case 0: Deck.GameType = "Alchemy"; break;
                case 1: Deck.GameType = "Brawl"; break;
                case 2: Deck.GameType = "Explorer"; break;
                case 3: Deck.GameType = "Historic"; break;
                case 4: Deck.GameType = "Standard"; break;
            }

            foreach (UniqueArtTypeViewModel card in Cards)
            {
                if (IsEditing)
                {
                    Card match = Deck.Cards.FirstOrDefault(c => c.Name == card.Name && c.SetSymbol == card.Model.set);

                    if (match != null)
                    {
                        match.Count = card.DeckBuilderDeckCount;

                        // if we have a match then just update the count and continue on
                        continue;
                    }
                }

                // if no match then make a new card
                Card newCard = new Card
                {
                    Count = card.DeckBuilderDeckCount,
                    Name = card.Name,
                    SetSymbol = card.Model.set,
                    Color = string.Join("", card.Model.colors),
                    Type = card.Model.type_line
                };

                bool success = int.TryParse(card.Model.collector_number, out int collectorNumber);

                if (success)
                {
                    newCard.CardNumber = collectorNumber;
                }
                else
                {
                    ServiceLocator.Instance.LoggerService.Error($"Unable to process card collector number to set the on the card {card.Name} during saving of the deck {Deck.Name}.");
                }

                Deck.Cards.Add(newCard);
            }

            // if our deck has cards our card collection does not then those cards need to be deleted as the user removed them
            List<Card> cardsToRemove = new List<Card>();

            foreach (Card card in Deck.Cards)
            {
                UniqueArtTypeViewModel match = Cards.FirstOrDefault(c => c.Name == card.Name && c.Model.set == card.SetSymbol);

                if (match == null)
                {
                    cardsToRemove.Add(card);
                }
            }

            foreach (Card cardToRemove in cardsToRemove)
            {
                Deck.Cards.Remove(cardToRemove);
            }

            // save deck to database
            if (!ServiceLocator.Instance.DatabaseService.SaveDeck(Deck))
            {
                ServiceLocator.Instance.MainWindowViewModel.StatusMessage = "Error saving deck";
                ServiceLocator.Instance.MainWindowViewModel.SetStatusMessageOnDelay("Creating deck", 7000);

                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Error Saving Changes";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "There was a problem saving the deck to the database. Please see log for details.";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true; // prevents closing
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                isSavingAlready = false;
            }
            else
            {
                // after successful save, if we have cards to delete then delete them
                if (cardsToRemove.Count > 0)
                {
                    if (!ServiceLocator.Instance.DatabaseService.DeleteCardsFromDeck(cardsToRemove))
                    {
                        ServiceLocator.Instance.LoggerService.Error($"Error deleting cards from deck in database. Deck: {Deck.Name}");
                    }
                }

                HasChanges = false;

                isSavingAlready = false;

                Action close = CloseAction;

                Clear();

                close();

                // refresh UI, with data from database
                ServiceLocator.Instance.MainWindowViewModel.DeckCollectionViewModel.QueryDatabaseForDecks();
            }
        }

        public bool SaveDeck()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }

            if (SelectedSetTypeIndex == -1)
            {
                return false;
            }

            if (!ServiceLocator.Instance.DatabaseService.IsDeckNameUnique(Name, -1))
            {
                return false;
            }

            switch (SelectedSetTypeIndex)
            {
                case 0: Deck.GameType = "Alchemy"; break;
                case 1: Deck.GameType = "Brawl"; break;
                case 2: Deck.GameType = "Explorer"; break;
                case 3: Deck.GameType = "Historic"; break;
                case 4: Deck.GameType = "Standard"; break;
            }

            foreach (UniqueArtTypeViewModel card in Cards)
            {
                if (IsEditing)
                {
                    Card editMatch = Deck.Cards.FirstOrDefault(c => c.Name == card.Name && c.SetSymbol == card.Model.set);

                    if (editMatch != null)
                    {
                        editMatch.Count = card.DeckBuilderDeckCount;

                        // if we have a match then just update the count and continue on
                        continue;
                    }
                }

                Card match = Deck.Cards.FirstOrDefault(c => c.Name == card.Name && c.SetSymbol == c.SetSymbol);

                if (match != null)
                {
                    match.Count++;
                }
                else
                {
                    Card newCard = new Card
                    {
                        Count = 1,
                        Name = card.Name,
                        SetSymbol = card.Model.set
                    };

                    bool success = int.TryParse(card.Model.collector_number, out int collectorNumber);

                    if (success)
                    {
                        newCard.CardNumber = collectorNumber;
                    }
                    else
                    {
                        ServiceLocator.Instance.LoggerService.Error($"Unable to process card collector number to set the on the card {card.Name} during saving of the deck {Deck.Name}.");
                    }

                    Deck.Cards.Add(newCard);
                }
            }

            // if our deck has cards our card collection does not then those cards need to be deleted as the user removed them
            List<Card> cardsToRemove = new List<Card>();

            foreach (Card card in Deck.Cards)
            {
                UniqueArtTypeViewModel match = Cards.FirstOrDefault(c => c.Name == card.Name && c.Model.set == card.SetSymbol);

                if (match == null)
                {
                    cardsToRemove.Add(card);
                }
            }

            foreach (Card cardToRemove in cardsToRemove)
            {
                Deck.Cards.Remove(cardToRemove);
            }

            bool result = ServiceLocator.Instance.DatabaseService.SaveDeck(Deck);

            if (!result) return result;

            // after successful save, if we have cards to delete then delete them
            if (cardsToRemove.Count > 0)
            {
                result = ServiceLocator.Instance.DatabaseService.DeleteCardsFromDeck(cardsToRemove);

                if (!result)
                {
                    ServiceLocator.Instance.LoggerService.Error($"Error deleting cards from deck in database. Deck: {Deck.Name}");
                }
            }

            return result;
        }

        public void SetCounts()
        {
            List<UniqueArtTypeViewModel> ones = GetCardsForCost(1, -1);

            // we need to remove cards from one that are lands
            ones = ones.Where(x => !x.Model.type_line.Contains("land", StringComparison.OrdinalIgnoreCase)).ToList();

            List<UniqueArtTypeViewModel> twos = GetCardsForCost(2, 0);
            List<UniqueArtTypeViewModel> threes = GetCardsForCost(3, 0);
            List<UniqueArtTypeViewModel> fours = GetCardsForCost(4, 0);
            List<UniqueArtTypeViewModel> fives = GetCardsForCost(5, 0);
            List<UniqueArtTypeViewModel> sixes = GetCardsForCost(6, 1);

            ValueOne = ones.Count;
            ValueTwo = twos.Count;
            ValueThree = threes.Count;
            ValueFour = fours.Count;
            ValueFive = fives.Count;
            ValueSix = sixes.Count;

            double highest = ValueOne;

            if (ValueTwo > highest)
                highest = ValueTwo;

            if (ValueThree > highest)
                highest = ValueThree;

            if (ValueFour > highest)
                highest = ValueFour;

            if (ValueFive > highest)
                highest = ValueFive;

            if (ValueSix > highest)
                highest = ValueSix;

            ProgressBarMax = highest + 5;

            OnPropertyChanged("AverageManaCost");
            OnPropertyChanged("NumberOfCreatures");
            OnPropertyChanged("NumberOfNonCreatures");
            OnPropertyChanged("NumberOfLands");

            TotalCardCount = 0;

            foreach (var card in Cards)
            {
                TotalCardCount += card.DeckBuilderDeckCount;
            }
        }

        private void ShowInfoWindow()
        {
            DeckBuilderInfoWindow deckBuilderInfoWindow = new DeckBuilderInfoWindow();
            deckBuilderInfoWindow.ShowDialog();
        }

        #endregion
    }
}
