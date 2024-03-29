﻿using MagicTheGatheringArenaDeckMaster.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MagicTheGatheringArenaDeckMaster.UserControls
{
    /// <summary>Represents a column of cards.</summary>
    public partial class CardColumnUserControl : UserControl
    {
        #region Properties

        /// <summary>Gets or sets the zoom factor for the card column. Default is 1.0.</summary>
        public double ZoomFactor
        {
            get { return (double)GetValue(ZoomFactorProperty); }
            set { SetValue(ZoomFactorProperty, value); }
        }

        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register("ZoomFactor", typeof(double), typeof(CardColumnUserControl), new PropertyMetadata(1.0));

        #endregion

        #region Constructors

        public CardColumnUserControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void CardControl_AddCard(object sender, UniqueArtTypeViewModel card)
        {
            // because of where add is fired from we only ever have to worry about increasing the count of the card
            card.DeckBuilderDeckCount++;

            ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.FireCollectionChanged();
        }

        private void CardControl_RemoveCard(object sender, UniqueArtTypeViewModel card)
        {
            // we either need to decrement the count or we need to remove the card if there is only one
            if (card.DeckBuilderDeckCount > 1)
            {
                card.DeckBuilderDeckCount--;
            }
            else
            {
                // we are going from 1 to 0 here so completely remove the card
                ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.Cards.Remove(card);
            }

            ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.FireCollectionChanged();
        }

        #endregion
    }
}
