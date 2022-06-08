using MagicTheGatheringArena.Core.MVVM;
using MagicTheGatheringArena.Core.Scryfall.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicTheGatheringArenaDeckMaster2.ViewModels
{
    internal class UniqueArtTypeViewModel : ViewModelBase
    {
        #region Fields

        private UniqueArtType model;
        private string imagePath;

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

        public string ImagePath
        {
            get => imagePath;
            set
            {
                imagePath = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        public UniqueArtTypeViewModel(UniqueArtType uat)
        {
            model = uat;
            imagePath = string.Empty;
        }

        #endregion
    }
}
