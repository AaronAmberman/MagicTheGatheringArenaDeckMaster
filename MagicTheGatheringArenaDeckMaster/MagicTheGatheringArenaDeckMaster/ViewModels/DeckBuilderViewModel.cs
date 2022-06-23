using MagicTheGatheringArena.Core.Database.Models;
using MagicTheGatheringArena.Core.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class DeckBuilderViewModel : ViewModelBase
    {
        #region Fields

        private double averageManaCost;
        private ICommand cancelCommand;
        private ObservableCollection<UniqueArtTypeViewModel> cards = new ObservableCollection<UniqueArtTypeViewModel>();
        private Deck deck;
        private ICommand infoCommand;
        private double progressBarMax;
        private ICommand saveCommand;
        private int selectedSetTypeIndex;
        private List<string> setTypes = new List<string>
        {
            "Alchemy",
            "Brawl",
            "Explorer",
            "Historic",
            "Standard",
        };
        private double valueOne;
        private double valueTwo;
        private double valueThree;
        private double valueFour;
        private double valueFive;
        private double valueSix;
        private bool hasChanges;

        #endregion

        #region Properties

        public double AverageManaCost
        {
            get => averageManaCost;
            set
            {
                averageManaCost = value;
                OnPropertyChanged();
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

        public Grid DynamicCardView { get; set; }

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
                        count++;
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
                        card.Model.type_line.Contains("land", StringComparison.OrdinalIgnoreCase))
                        continue;
                    else
                        count++;
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
                    if (card.Model.type_line.Contains("land", StringComparison.OrdinalIgnoreCase))
                        count++;
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

                OnPropertyChanged();
            }
        }

        public List<string> SetTypes => setTypes;

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
            HasChanges = true;

            SetCounts();
        }

        public void Clear()
        {
            cards.Clear();

            AverageManaCost = 0.0;
            CloseAction = null;
            Deck = null;
            DynamicCardView = null;
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
            HasChanges = false;

            // todo : save
            switch(SelectedSetTypeIndex)
            {
                case 0: Deck.GameType = "Alchemy"; break;
                case 1: Deck.GameType = "Brawl"; break;
                case 2: Deck.GameType = "Explorer"; break;
                case 3: Deck.GameType = "Historic"; break;
                case 4: Deck.GameType = "Standard"; break;
            }

            foreach (UniqueArtTypeViewModel card in Cards)
            {
                Deck.Cards.Add(new Card
                {
                    CardNumber = -1,
                    Count = 1, // figure out how to manage card count
                    Name = card.Name,
                    SetSymbol = card.Model.set_id                    
                });
            }

            if (!ServiceLocator.Instance.DatabaseService.SaveDeck(Deck))
            {

            }

            Action close = CloseAction;

            Clear();

            close();
        }

        private void SetCounts()
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

            OnPropertyChanged("NumberOfCreatures");
            OnPropertyChanged("NumberOfNonCreatures");
            OnPropertyChanged("NumberOfLands");
        }

        private void ShowInfoWindow()
        {
            DeckBuilderInfoWindow deckBuilderInfoWindow = new DeckBuilderInfoWindow();
            deckBuilderInfoWindow.ShowDialog();
        }

        #endregion
    }
}
