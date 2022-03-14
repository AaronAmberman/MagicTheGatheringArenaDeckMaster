using System.Windows;
using System.Windows.Controls;

namespace MagicTheGatheringArenaDeckMaster.UserControls
{
    public partial class CardDataUserControl : UserControl
    {
        public CardDataUserControl()
        {
            InitializeComponent();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox? cb = e.OriginalSource as CheckBox;

            if (cb == null) return;

            // we want the user to not be able to click the header check box into a null/partial check state
            // with mouse clicks they can only set it to checked or unchecked not partial
            // we do this here so it only happens when the user clicks the mouse not when we set the state programmatically
            // this is true for any column in the list view that has a check box in the header
            if (!cb.IsChecked.HasValue)
                cb.IsChecked = false;
        }
    }
}
