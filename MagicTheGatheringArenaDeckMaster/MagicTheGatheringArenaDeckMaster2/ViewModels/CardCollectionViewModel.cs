using MagicTheGatheringArena.Core.MVVM;
using System.Collections.ObjectModel;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class CardCollectionViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<UniqueArtTypeViewModel> cards = new ObservableCollection<UniqueArtTypeViewModel>();
        private bool isBlackChecked;
        private bool isBlueChecked;
        private bool isColorlessChecked;
        private bool isGreenChecked;
        private bool isLandChecked;
        private bool isMulticolorChecked;
        private bool isRedChecked;
        private bool isWhiteChecked;

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

        public bool IsBlackChecked
        { 
            get => isBlackChecked; 
            set 
            {
                isBlackChecked = value;
                OnPropertyChanged();
            }
        }

        public bool IsBlueChecked
        {
            get => isBlueChecked;
            set
            {
                isBlueChecked = value;
                OnPropertyChanged();
            }
        }

        public bool IsColorlessChecked
        {
            get => isColorlessChecked;
            set
            {
                isColorlessChecked = value;
                OnPropertyChanged();
            }
        }

        public bool IsGreenChecked
        {
            get => isGreenChecked;
            set
            {
                isGreenChecked = value;
                OnPropertyChanged();
            }
        }

        public bool IsLandChecked
        {
            get => isLandChecked;
            set
            {
                isLandChecked = value;
                OnPropertyChanged();
            }
        }

        public bool IsMulticolorChecked
        {
            get => isMulticolorChecked;
            set
            {
                isMulticolorChecked = value;
                OnPropertyChanged();
            }
        }

        public bool IsRedChecked
        {
            get => isRedChecked;
            set
            {
                isRedChecked = value;
                OnPropertyChanged();
            }
        }

        public bool IsWhiteChecked
        {
            get => isWhiteChecked;
            set
            {
                isWhiteChecked = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods



        #endregion
    }
}
