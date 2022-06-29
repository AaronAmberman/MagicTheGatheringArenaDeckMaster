using System.Windows;
using System.Windows.Controls;

namespace MagicTheGatheringArenaDeckMaster.CustomControls
{
    /// <summary>Represents the cost of one mana symbol.</summary>
    public class ManaCostSymbol : Control
    {
        #region Properties

        /// <summary>Gets or sets the image to use for the symbol (if not text).</summary>
        public string SymbolImage
        {
            get { return (string)GetValue(SymbolImageProperty); }
            set { SetValue(SymbolImageProperty, value); }
        }

        public static readonly DependencyProperty SymbolImageProperty =
            DependencyProperty.Register("SymbolImage", typeof(string), typeof(ManaCostSymbol), new PropertyMetadata(null));

        /// <summary>Gets or sets the text to use for the symbol (if not an image).</summary>
        public string SymbolText
        {
            get { return (string)GetValue(SymbolTextProperty); }
            set { SetValue(SymbolTextProperty, value); }
        }

        public static readonly DependencyProperty SymbolTextProperty =
            DependencyProperty.Register("SymbolText", typeof(string), typeof(ManaCostSymbol), new PropertyMetadata(null));

        #endregion

        #region Constructors

        static ManaCostSymbol()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ManaCostSymbol), new FrameworkPropertyMetadata(typeof(ManaCostSymbol)));
        }

        #endregion
    }
}
