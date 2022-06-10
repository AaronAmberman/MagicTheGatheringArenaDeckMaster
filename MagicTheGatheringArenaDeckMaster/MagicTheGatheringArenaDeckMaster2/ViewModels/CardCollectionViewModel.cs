using MagicTheGatheringArena.Core.MVVM;
using System.Collections.ObjectModel;

namespace MagicTheGatheringArenaDeckMaster2.ViewModels
{
    internal class CardCollectionViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<UniqueArtTypeViewModel> cards = new ObservableCollection<UniqueArtTypeViewModel>();

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

        #endregion

        #region Methods



        #endregion
    }
}
