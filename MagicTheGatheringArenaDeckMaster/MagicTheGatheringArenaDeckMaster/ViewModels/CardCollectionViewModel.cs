using MagicTheGatheringArena.Core.MVVM;
using MagicTheGatheringArena.Core.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPF.InternalDialogs;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class CardCollectionViewModel : ViewModelBase
    {
        #region Fields

        private ICommand advancedFilterCommand;
        private ObservableCollection<UniqueArtTypeViewModel> cards = new ObservableCollection<UniqueArtTypeViewModel>();
        private CollectionView cardCollectionView;
        private bool isBlackChecked;
        private bool isBlueChecked;
        private bool isColorlessChecked;
        private bool isGreenChecked;
        private bool isLandChecked;
        private bool isMulticolorChecked;
        private bool isRedChecked;
        private bool isWhiteChecked;
        private string searchText;
        private SingleShotTimer timer;

        #endregion

        #region Properties

        public ICommand AdvancedFilterCommand => advancedFilterCommand ??= new RelayCommand(ShowAdvancedFilterDialog);

        public ObservableCollection<UniqueArtTypeViewModel> Cards
        {
            get => cards;
            set
            {
                cards = value;
                OnPropertyChanged();
            }
        }

        public bool IsBlackChecked
        {
            get => isBlackChecked;
            set
            {
                isBlackChecked = value;

                SetupOrRefreshFilter();

                OnPropertyChanged();
            }
        }

        public bool IsBlueChecked
        {
            get => isBlueChecked;
            set
            {
                isBlueChecked = value;

                SetupOrRefreshFilter();

                OnPropertyChanged();
            }
        }

        public bool IsColorlessChecked
        {
            get => isColorlessChecked;
            set
            {
                isColorlessChecked = value;

                SetupOrRefreshFilter();

                OnPropertyChanged();
            }
        }

        public bool IsGreenChecked
        {
            get => isGreenChecked;
            set
            {
                isGreenChecked = value;

                SetupOrRefreshFilter();

                OnPropertyChanged();
            }
        }

        public bool IsLandChecked
        {
            get => isLandChecked;
            set
            {
                isLandChecked = value;

                SetupOrRefreshFilter();

                OnPropertyChanged();
            }
        }

        public bool IsMulticolorChecked
        {
            get => isMulticolorChecked;
            set
            {
                isMulticolorChecked = value;

                SetupOrRefreshFilter();

                OnPropertyChanged();
            }
        }

        public bool IsRedChecked
        {
            get => isRedChecked;
            set
            {
                isRedChecked = value;

                SetupOrRefreshFilter();

                OnPropertyChanged();
            }
        }

        public bool IsWhiteChecked
        {
            get => isWhiteChecked;
            set
            {
                isWhiteChecked = value;

                SetupOrRefreshFilter();

                OnPropertyChanged();
            }
        }

        public string SearchText 
        { 
            get => searchText; 
            set
            {
                searchText = value;

                SetupOrRefreshFilter();

                OnPropertyChanged();
            }
        }

        public ListBox CardListBox { get; set; }

        #endregion

        #region Methods

        private bool FilterCards(object obj)
        {
            if (obj is not UniqueArtTypeViewModel uavm) return false;
            if (uavm == null) return false;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                if (!IsBlackChecked && !IsBlueChecked && !IsColorlessChecked && !IsGreenChecked && 
                    !IsLandChecked && !IsMulticolorChecked && !IsRedChecked && !IsWhiteChecked)
                {
                    return true;
                }
                else
                {
                    if (IsMulticolorChecked && uavm.Model.colors.Count >= 2 && GetNumberOfSelectedColors() > 1)
                    {
                        List<string> colors = GetSelectedColors();
                        List<bool> matchedColors = GetMatchedColors(uavm, colors);

                        if (matchedColors.Count == colors.Count && matchedColors.All(val => val)) return true;
                        else return false;
                    }
                    else if (IsMulticolorChecked && uavm.Model.colors.Count >= 2 && !IsBlackChecked && !IsBlueChecked && !IsColorlessChecked &&
                        !IsGreenChecked && !IsLandChecked && !IsRedChecked && !IsWhiteChecked)
                    {
                        return true;
                    }
                    else if (IsColorlessChecked && !uavm.Model.mana_cost.Contains("{B}") && !uavm.Model.mana_cost.Contains("{U}") &&
                        !uavm.Model.mana_cost.Contains("{G}") && !uavm.Model.mana_cost.Contains("{R}") && !uavm.Model.mana_cost.Contains("{W}") &&
                        uavm.Model.type_line.Contains("Artifact"))
                    {
                        return true;
                    }
                    else if (IsBlackChecked && uavm.Model.mana_cost.Contains("{B}") && !IsMulticolorChecked)
                    {
                        return true;
                    }
                    else if (IsBlueChecked && uavm.Model.mana_cost.Contains("{U}") && !IsMulticolorChecked)
                    {
                        return true;
                    }
                    else if (IsGreenChecked && uavm.Model.mana_cost.Contains("{G}") && !IsMulticolorChecked)
                    {
                        return true;
                    }
                    else if (IsLandChecked && uavm.Model.type_line.Contains("Land"))
                    {
                        return true;
                    }
                    else if (IsRedChecked && uavm.Model.mana_cost.Contains("{R}") && !IsMulticolorChecked)
                    {
                        return true;
                    }
                    else if (IsWhiteChecked && uavm.Model.mana_cost.Contains("{W}") && !IsMulticolorChecked)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                bool textMatch = false;

                if (uavm.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) textMatch = true;
                else if (!string.IsNullOrWhiteSpace(uavm.Model.oracle_text) && uavm.Model.oracle_text.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) textMatch = true;
                else if (!string.IsNullOrWhiteSpace(uavm.Model.flavor_text) && uavm.Model.flavor_text.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) textMatch = true;
                else if (!string.IsNullOrWhiteSpace(uavm.Model.type_line) && uavm.Model.type_line.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) textMatch = true;
                else if (!string.IsNullOrWhiteSpace(uavm.Model.rarity) && uavm.Model.rarity.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) textMatch = true;

                if (!IsBlackChecked && !IsBlueChecked && !IsColorlessChecked && !IsGreenChecked &&
                    !IsLandChecked && !IsMulticolorChecked && !IsRedChecked && !IsWhiteChecked && textMatch)
                {
                    return true;
                }
                else
                {
                    if (IsMulticolorChecked && uavm.Model.colors.Count >= 2 && GetNumberOfSelectedColors() > 1)
                    {
                        List<string> colors = GetSelectedColors();
                        List<bool> matchedColors = GetMatchedColors(uavm, colors);

                        if (matchedColors.Count == colors.Count && matchedColors.All(val => val) && textMatch) return true;
                        else return false;
                    }
                    else if (IsMulticolorChecked && uavm.Model.colors.Count >= 2 && !IsBlackChecked && !IsBlueChecked && !IsColorlessChecked &&
                        !IsGreenChecked && !IsLandChecked && !IsRedChecked && !IsWhiteChecked && textMatch)
                    {
                        return true;
                    }
                    else if (IsColorlessChecked && !uavm.Model.mana_cost.Contains("{B}") && !uavm.Model.mana_cost.Contains("{U}") &&
                        !uavm.Model.mana_cost.Contains("{G}") && !uavm.Model.mana_cost.Contains("{R}") && !uavm.Model.mana_cost.Contains("{W}") &&
                        uavm.Model.type_line.Contains("Artifact") && textMatch)
                    {
                        return true;
                    }
                    else if (IsBlackChecked && uavm.Model.mana_cost.Contains("{B}") && !IsMulticolorChecked && textMatch)
                    {
                        return true;
                    }
                    else if (IsBlueChecked && uavm.Model.mana_cost.Contains("{U}") && !IsMulticolorChecked && textMatch)
                    {
                        return true;
                    }
                    else if (IsGreenChecked && uavm.Model.mana_cost.Contains("{G}") && !IsMulticolorChecked && textMatch)
                    {
                        return true;
                    }
                    else if (IsLandChecked && uavm.Model.type_line.Contains("Land") && textMatch)
                    {
                        return true;
                    }
                    else if (IsRedChecked && uavm.Model.mana_cost.Contains("{R}") && !IsMulticolorChecked && textMatch)
                    {
                        return true;
                    }
                    else if (IsWhiteChecked && uavm.Model.mana_cost.Contains("{W}") && !IsMulticolorChecked && textMatch)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private List<bool> GetMatchedColors(UniqueArtTypeViewModel uavm, List<string> colors)
        {
            List<string> colorsOnCard = uavm.Model.mana_cost.Split("{", StringSplitOptions.RemoveEmptyEntries).ToList();
            List<bool> matchedColors = new List<bool>();

            foreach (string color in colorsOnCard)
            {
                string temp = color.Replace("}", "");

                foreach (string color2 in colors)
                {
                    if (temp == color2)
                    {
                        matchedColors.Add(true);

                        break; // onto the next color
                    }
                }
            }

            return matchedColors;
        }

        private int GetNumberOfSelectedColors()
        {
            int count = 0;

            if (IsBlackChecked) count++;
            if (IsBlueChecked) count++;
            if (IsGreenChecked) count++;
            if (IsRedChecked) count++;
            if (IsWhiteChecked) count++;

            return count;
        }

        private List<string> GetSelectedColors()
        {
            List<string> colors = new List<string>();

            if (IsBlackChecked) colors.Add("B");
            if (IsBlueChecked) colors.Add("U");
            if (IsGreenChecked) colors.Add("G");
            if (IsRedChecked) colors.Add("R");
            if (IsWhiteChecked) colors.Add("W");

            return colors;
        }

        private void SetupOrRefreshFilter()
        {
            if (timer == null)
            {
                timer = new SingleShotTimer
                {
                    Interval = 1500
                };
            }

            timer.Start(() =>
            {
                ServiceLocator.Instance.MainWindowViewModel.Dispatcher.Invoke(() =>
                {
                    if (cardCollectionView == null)
                    {
                        cardCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(Cards);
                        cardCollectionView.Filter = FilterCards; // this calls the filter for the first time;
                    }
                    else
                    {
                        cardCollectionView.Refresh();
                    }
                });
            });
        }

        private void ShowAdvancedFilterDialog()
        {
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "Under construction.";
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Under Construction";
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true;
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

            ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();
        }

        #endregion
    }
}
