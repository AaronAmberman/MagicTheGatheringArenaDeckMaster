using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MagicTheGatheringArenaDeckMaster.UserControls
{
    public partial class CardsUserControl : UserControl
    {
        private double scale = 1.0;

        public CardsUserControl()
        {
            InitializeComponent();
        }

        private void CardImageListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            if (e.Delta < 0 && scale > 0.5)
            {
                scale -= 0.1;

                CardImageListView.LayoutTransform = new ScaleTransform(scale, scale);                
            }
            else if (e.Delta > 0 && scale < 2.0)
            {
                scale += 0.1;

                CardImageListView.LayoutTransform = new ScaleTransform(scale, scale);
            }
        }
    }
}
