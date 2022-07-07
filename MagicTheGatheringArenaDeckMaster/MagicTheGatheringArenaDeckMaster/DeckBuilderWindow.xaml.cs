using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MagicTheGatheringArenaDeckMaster
{
    public partial class DeckBuilderWindow : Window
    {
        #region Constructors

        public DeckBuilderWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void resetZoom_Click(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.ZoomFactor = 1.0;
        }

        #endregion
    }
}
