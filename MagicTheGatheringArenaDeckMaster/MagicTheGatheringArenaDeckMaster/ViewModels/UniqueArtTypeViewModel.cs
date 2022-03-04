using MagicTheGatheringArena.Core.MVVM;
using MagicTheGatheringArena.Core.Scryfall.Data;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class UniqueArtTypeViewModel : ViewModelBase
    {
        #region Fields

        private UniqueArtType model;
        private bool downloadSmallPicture;
        private bool downloadNormalPicture;
        private bool downloadLargePicture;
        private bool downloadPngPicture;
        private bool downloadArtCropPicture;
        private bool downloadBorderCropPicture;
        private bool isInternallySettingValue = false;
        private MainWindowViewModel parent;

        #endregion

        #region Properties

        public UniqueArtType Model
        {
            get => model;
            set
            {
                model = value;
                OnPropertyChanged();
            }
        }

        public string Name => model.name_field;
        public string Set => model.set_name;

        public bool DownloadSmallPicture
        {
            get => downloadSmallPicture;
            set
            {
                // only increment or decrement our count if the value changes, true to true or false to false should not trigger a increase or decrease
                if (downloadSmallPicture != value)
                {
                    if (value) Parent.SelectedImageCount++;
                    else Parent.SelectedImageCount--;
                }

                downloadSmallPicture = value;

                if (!IsInternallySettingValue)
                    Parent.VerifyStateOfSmallCheckBoxAndSet();

                OnPropertyChanged();
            }
        }

        public bool DownloadNormalPicture
        {
            get => downloadNormalPicture;
            set
            {
                // only increment or decrement our count if the value changes, true to true or false to false should not trigger a increase or decrease
                if (downloadNormalPicture != value)
                {
                    if (value) Parent.SelectedImageCount++;
                    else Parent.SelectedImageCount--;
                }

                downloadNormalPicture = value;

                if (!IsInternallySettingValue)
                    Parent.VerifyStateOfNormalCheckBoxAndSet();

                OnPropertyChanged();
            }
        }

        public bool DownloadLargePicture
        {
            get => downloadLargePicture;
            set
            {
                // only increment or decrement our count if the value changes, true to true or false to false should not trigger a increase or decrease
                if (downloadLargePicture != value)
                {
                    if (value) Parent.SelectedImageCount++;
                    else Parent.SelectedImageCount--;
                }

                downloadLargePicture = value;

                if (!IsInternallySettingValue)
                    Parent.VerifyStateOfLargeCheckBoxAndSet();

                OnPropertyChanged();
            }
        }

        public bool DownloadPngPicture
        {
            get => downloadPngPicture;
            set
            {
                // only increment or decrement our count if the value changes, true to true or false to false should not trigger a increase or decrease
                if (downloadPngPicture != value)
                {
                    if (value) Parent.SelectedImageCount++;
                    else Parent.SelectedImageCount--;
                }

                downloadPngPicture = value;

                if (!IsInternallySettingValue)
                    Parent.VerifyStateOfPngCheckBoxAndSet();

                OnPropertyChanged();
            }
        }

        public bool DownloadArtCropPicture
        {
            get => downloadArtCropPicture;
            set
            {
                // only increment or decrement our count if the value changes, true to true or false to false should not trigger a increase or decrease
                if (downloadArtCropPicture != value)
                {
                    if (value) Parent.SelectedImageCount++;
                    else Parent.SelectedImageCount--;
                }

                downloadArtCropPicture = value;

                if (!IsInternallySettingValue)
                    Parent.VerifyStateOfArtCropCheckBoxAndSet();

                OnPropertyChanged();
            }
        }

        public bool DownloadBorderCropPicture
        {
            get => downloadBorderCropPicture;
            set
            {
                // only increment or decrement our count if the value changes, true to true or false to false should not trigger a increase or decrease
                if (downloadBorderCropPicture != value)
                {
                    if (value) Parent.SelectedImageCount++;
                    else Parent.SelectedImageCount--;
                }

                downloadBorderCropPicture = value;

                if (!IsInternallySettingValue)
                    Parent.VerifyStateOfBorderCropCheckBoxAndSet();

                OnPropertyChanged();
            }
        }

        public bool IsInternallySettingValue
        {
            get => isInternallySettingValue;
            set
            {
                isInternallySettingValue = value;
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

        #region Constructors

        public UniqueArtTypeViewModel(UniqueArtType uat, MainWindowViewModel parent)
        {
            model = uat;
            this.parent = parent;
        }

        #endregion
    }
}
