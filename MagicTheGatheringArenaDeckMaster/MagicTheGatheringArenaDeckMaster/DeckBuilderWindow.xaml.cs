using System.Windows;

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ServiceLocator.Instance.MainWindowViewModel.DeckBuilderViewModel.DynamicCardView = dynamicCardView;
        }

        #endregion
    }
}
