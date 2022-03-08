using MagicTheGatheringArena.Core.MVVM;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class SetNameViewModel : ViewModelBase
    {
        #region Fields

        private string? name;
        private bool isChecked;
        private MainWindowViewModel parent;

        #endregion

        #region Properties

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public bool IsChecked
        {
            get => isChecked;
            set
            {
                isChecked = value;

                Parent.CardCollectionView?.Refresh();
                Parent.CardImageCollectionView?.Refresh();
                Parent.VerifyStateOfSmallCheckBoxAndSet();
                Parent.VerifyStateOfNormalCheckBoxAndSet();
                Parent.VerifyStateOfLargeCheckBoxAndSet();
                Parent.VerifyStateOfPngCheckBoxAndSet();
                Parent.VerifyStateOfArtCropCheckBoxAndSet();
                Parent.VerifyStateOfBorderCropCheckBoxAndSet();

                OnPropertyChanged();
            }
        }

        public MainWindowViewModel Parent
        {
            get => parent;
            set
            {
                parent = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public SetNameViewModel(MainWindowViewModel parent)
        {
            this.parent = parent;
        }
    }
}
