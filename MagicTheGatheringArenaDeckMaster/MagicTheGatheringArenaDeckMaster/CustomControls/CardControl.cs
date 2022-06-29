using MagicTheGatheringArenaDeckMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPF.CustomControls;

namespace MagicTheGatheringArenaDeckMaster.CustomControls
{
    /// <summary>Represents a card in a deck in the deck builder.</summary>
    public class CardControl : Control
    {
        #region Fields

        private Border border;
        private StackPanel manaImagePanel;
        private RoundableButton addRemoveButton;

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

        #region Events

        public event EventHandler<UniqueArtTypeViewModel> AddCard;
        public event EventHandler<UniqueArtTypeViewModel> RemoveCard;

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

        #endregion

        #region Methods

        private void CardControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UniqueArtTypeViewModel vm = e.NewValue as UniqueArtTypeViewModel;

            if (vm == null) return;

            SetColorSchemeBasedOnCard(vm);
        }

        public override void OnApplyTemplate()
        {
            // this fires after DataContextChanged

            base.OnApplyTemplate();

            border = GetTemplateChild("border") as Border;
            addRemoveButton = GetTemplateChild("addRemoveButton") as RoundableButton;
            manaImagePanel = GetTemplateChild("ManaImagePanel") as StackPanel;

            UniqueArtTypeViewModel vm = DataContext as UniqueArtTypeViewModel;

            if (vm == null) return;

            string[] valueGroup = vm.Model.mana_cost.Split("}", StringSplitOptions.RemoveEmptyEntries);

            List<ManaCostSymbol> cost = new List<ManaCostSymbol>();

            for (int i = 0; i < valueGroup.Length; i++)
            {
                string value = valueGroup[i];
                string temp = value.Replace("{", "");

                cost.Add(SetSymbol(temp));
            }

            foreach (ManaCostSymbol symbol in cost)
            {
                manaImagePanel.Children.Add(symbol);
            }

            border.MouseLeftButtonDown += Border_MouseLeftButtonDown;
            addRemoveButton.Click += AddRemoveButton_Click;
            addRemoveButton.MouseRightButtonDown += AddRemoveButton_MouseRightButtonDown;
        }

        private void AddRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            AddCard?.Invoke(this, DataContext as UniqueArtTypeViewModel);
        }

        private void AddRemoveButton_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // remove card (-1 on card if more than one, if one then remove card completely)
            RemoveCard?.Invoke(this, DataContext as UniqueArtTypeViewModel);
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RemoveCard?.Invoke(this, DataContext as UniqueArtTypeViewModel);
        }

        private ManaCostSymbol SetSymbol(string value)
        {
            if (value.Contains("W/U"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolImage = "pack://application:,,,/Images/white-blue.png";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (value.Contains("W/B"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolImage = "pack://application:,,,/Images/white-black.png";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (value.Contains("U/B"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolImage = "pack://application:,,,/Images/blue-black.png";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (value.Contains("U/R"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolImage = "pack://application:,,,/Images/blue-red.png";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (value.Contains("B/R"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolImage = "pack://application:,,,/Images/black-red.png";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (value.Contains("B/G"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolImage = "pack://application:,,,/Images/black-green.png";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (value.Contains("R/W"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolImage = "pack://application:,,,/Images/red-white.png";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (value.Contains("R/G"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolImage = "pack://application:,,,/Images/red-green.png";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (value.Contains("G/W"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolImage = "pack://application:,,,/Images/green-white.png";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (value.Contains("G/B"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolImage = "pack://application:,,,/Images/green-blue.png";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (value.Contains("W"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolImage = "pack://application:,,,/Images/white.png";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (value.Contains("U"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolImage = "pack://application:,,,/Images/blue.png";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (value.Contains("B"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolImage = "pack://application:,,,/Images/black.png";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (value.Contains("R"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolImage = "pack://application:,,,/Images/red.png";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (value.Contains("G"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolImage = "pack://application:,,,/Images/green.png";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (value.Contains("X"))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolText = "X";
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else if (int.TryParse(value, out int convRes))
            {
                ManaCostSymbol symbol = new ManaCostSymbol();
                symbol.SymbolText = convRes.ToString(System.Globalization.CultureInfo.CurrentCulture);
                symbol.Margin = new Thickness(2, 0, 0, 0);

                return symbol;
            }
            else return null;
        }

        private void SetColorSchemeBasedOnCard(UniqueArtTypeViewModel model)
        {
            if (model.NumberOfColors == 0)
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF949494"));
                InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCBCBCB"));
            }
            else if (model.NumberOfColors == 1)
            {
                if (model.Model.colors[0] == "C") // colorless
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFAAAAAA"));
                    InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCCCCCC"));
                }
                else if (model.Model.colors[0] == "G") // green
                {
                    Background = Brushes.Green;
                    InnerBackground = Brushes.LightGreen;
                }
                else if (model.Model.colors[0] == "U") // blue
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0880C3"));
                    InnerBackground = Brushes.LightSkyBlue;
                }
                else if (model.Model.colors[0] == "R") // red
                {
                    Background = Brushes.Red;
                    InnerBackground = Brushes.Salmon;
                }
                else if (model.Model.colors[0] == "B") // black
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF444444"));
                    InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF888888"));
                }
                else if (model.Model.colors[0] == "W") // white
                {
                    Background = Brushes.White;
                    InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
                }
            }
            else if (model.NumberOfColors == 2)
            {
                Color colorOne;
                Color colorTwo;

                if (model.Model.colors[0] == "W")
                {
                    colorOne = Brushes.White.Color;
                }
                else if (model.Model.colors[0] == "U")
                {
                    colorOne = (Color)ColorConverter.ConvertFromString("#FF0880C3");
                }
                else if (model.Model.colors[0] == "B")
                {
                    colorOne = (Color)ColorConverter.ConvertFromString("#FF444444");
                }
                else if (model.Model.colors[0] == "R")
                {
                    colorOne = Brushes.Red.Color;
                }
                else if (model.Model.colors[0] == "G")
                {
                    colorOne = Brushes.Green.Color;
                }

                if (model.Model.colors[1] == "W")
                {
                    colorTwo = Brushes.White.Color;
                }
                else if (model.Model.colors[1] == "U")
                {
                    colorTwo = (Color)ColorConverter.ConvertFromString("#FF0880C3");
                }
                else if (model.Model.colors[1] == "B")
                {
                    colorTwo = (Color)ColorConverter.ConvertFromString("#FF444444");
                }
                else if (model.Model.colors[1] == "R")
                {
                    colorTwo = Brushes.Red.Color;
                }
                else if (model.Model.colors[1] == "G")
                {
                    colorTwo = Brushes.Green.Color;
                }

                Background = new LinearGradientBrush(colorOne, colorTwo, new Point(0, 0), new Point(1, 1));
                InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD6BE73"));
            }
            else if (model.NumberOfColors >= 3 && model.NumberOfColors <= 5)
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB3A26D"));
                InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD6BE73"));
            }
            else if (model.NumberOfColors == 6) // Artifacts
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF949494"));
                InnerBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCBCBCB"));
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
