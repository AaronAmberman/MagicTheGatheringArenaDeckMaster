using MagicTheGatheringArenaDeckMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagicTheGatheringArenaDeckMaster.CustomControls
{
    /// <summary>Represents a card in a deck in the deck builder.</summary>
    public class CardControl : Control
    {
        #region Fields

        #endregion

        #region Properties

        public string CardName
        {
            get { return (string)GetValue(CardNameProperty); }
            set { SetValue(CardNameProperty, value); }
        }

        public static readonly DependencyProperty CardNameProperty =
            DependencyProperty.Register("CardName", typeof(string), typeof(CardControl), new PropertyMetadata(string.Empty));

        public int CardCount
        {
            get { return (int)GetValue(CardCountProperty); }
            set { SetValue(CardCountProperty, value); }
        }

        public static readonly DependencyProperty CardCountProperty =
            DependencyProperty.Register("CardCount", typeof(int), typeof(CardControl), new PropertyMetadata(1));

        public SolidColorBrush InnerBackground
        {
            get { return (SolidColorBrush)GetValue(InnerBackgroundProperty); }
            set { SetValue(InnerBackgroundProperty, value); }
        }

        public static readonly DependencyProperty InnerBackgroundProperty =
            DependencyProperty.Register("InnerBackground", typeof(SolidColorBrush), typeof(CardControl), new PropertyMetadata(Brushes.Transparent));

        #endregion

        #region Constructors

        static CardControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CardControl), new FrameworkPropertyMetadata(typeof(CardControl)));
        }

        public CardControl()
        {
            DataContextChanged += CardControl_DataContextChanged;
        }

        private void CardControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UniqueArtTypeViewModel vm = e.NewValue as UniqueArtTypeViewModel;

            if (vm == null) return;

            SetColorSchemeBasedOnCard(vm);
        }

        #endregion

        #region Methods

        private void SetColorSchemeBasedOnCard(UniqueArtTypeViewModel model)
        {
            if (model.NumberOfColors == 0)
            {
                model.ToString();
            }
            else if (model.NumberOfColors == 1)
            {
                // mono colored cards
            }
            else if (model.NumberOfColors == 2)
            {
                // two colored cards
            }
            else if (model.NumberOfColors >= 3 && model.NumberOfColors <= 5)
            {
                // three or more colored cards
            }
            else if (model.NumberOfColors == 6) // Artifacts
            {
                if (model.Model.type_line.Equals("artifact land", StringComparison.OrdinalIgnoreCase)) // artifact land
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF949494"));
                    InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCBCBCB"));
                }
            }
            else if (model.NumberOfColors >= 7) // Lands
            {
                int startIndex = model.Model.oracle_text.IndexOf("{t}: add", StringComparison.OrdinalIgnoreCase);

                if (startIndex == -1) // lands that don't have the tap character
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFAAAAAA"));
                    InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCCCCC"));

                    return;
                }

                int endIndex = model.Model.oracle_text.IndexOf(".", startIndex + 1, StringComparison.OrdinalIgnoreCase);

                string addText = model.Model.oracle_text.Substring(startIndex, endIndex - startIndex);

                if (addText.Contains("any color", StringComparison.OrdinalIgnoreCase)) // lands that add mana of any color
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFAAAAAA"));
                    InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCCCCC"));

                    return;
                }

                if (addText.Contains("chosen color", StringComparison.OrdinalIgnoreCase)) // lands that add mana of a chosen color
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFAAAAAA"));
                    InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCCCCC"));

                    return;
                }

                if (!string.IsNullOrWhiteSpace(addText))
                    addText = addText.Replace("{T}: Add ", "").Replace(",", "").Replace("or", "").Replace(" ", "").Replace("}", "");

                string[] exploded = addText.Split("{", StringSplitOptions.RemoveEmptyEntries);

                if (exploded.Length == 1)
                {
                    if (exploded[0] == "C") // colorless
                    {
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFAAAAAA"));
                        InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCCCCC"));
                    }
                    else if (exploded[0] == "G") // green
                    {
                        Background = Brushes.Green;
                        InnerBackground = Brushes.LightGreen;
                    }
                    else if (exploded[0] == "U") // blue
                    {
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0880C3"));
                        InnerBackground = Brushes.LightSkyBlue;
                    }
                    else if (exploded[0] == "R") // red
                    {
                        Background = Brushes.Red;
                        InnerBackground = Brushes.Salmon;
                    }
                    else if (exploded[0] == "B") // black
                    {
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF444444"));
                        InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF888888"));
                    }
                    else if (exploded[0] == "W") // white
                    {
                        Background = Brushes.White;
                        InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
                    }
                }
                else if (exploded.Length == 2)
                {
                    Color colorOne;
                    Color colorTwo;

                    if (exploded[0] == "W")
                    {
                        colorOne = Brushes.White.Color;
                    }
                    else if (exploded[0] == "U")
                    {
                        colorOne = (Color)ColorConverter.ConvertFromString("#FF0880C3");
                    }
                    else if (exploded[0] == "B")
                    {
                        colorOne = (Color)ColorConverter.ConvertFromString("#FF444444");
                    }
                    else if (exploded[0] == "R")
                    {
                        colorOne = Brushes.Red.Color;
                    }
                    else if (exploded[0] == "G")
                    {
                        colorOne = Brushes.Green.Color;
                    }

                    if (exploded[1] == "W")
                    {
                        colorTwo = Brushes.White.Color;
                    }
                    else if (exploded[1] == "U")
                    {
                        colorTwo = (Color)ColorConverter.ConvertFromString("#FF0880C3");
                    }
                    else if (exploded[1] == "B")
                    {
                        colorTwo = (Color)ColorConverter.ConvertFromString("#FF444444");
                    }
                    else if (exploded[1] == "R")
                    {
                        colorTwo = Brushes.Red.Color;
                    }
                    else if (exploded[1] == "G")
                    {
                        colorTwo = Brushes.Green.Color;
                    }

                    Background = new LinearGradientBrush(colorOne, colorTwo, new Point(0, 0), new Point(1, 1));
                    InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD6BE73"));
                }
                else if (exploded.Length == 3)
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB3A26D"));
                    InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD6BE73"));
                }
                else // every other land that did not fit the above categories
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFAAAAAA"));
                    InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCCCCC"));
                }
            }
        }

        #endregion
    }
}
