﻿using MagicTheGatheringArena.Core.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class CardColumnViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<UniqueArtTypeViewModel> cards = new ObservableCollection<UniqueArtTypeViewModel>();
        private object header;
        private SolidColorBrush lineBrush;

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

        public object Header
        {
            get => header;
            set 
            { 
                header = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush LineBrush
        {
            get => lineBrush;
            set 
            { 
                lineBrush = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        #endregion

        #region Methods

        #endregion
    }
}
