using MagicTheGatheringArena.Core.MVVM;
using MagicTheGatheringArenaDeckMaster.Models;
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
    internal class SetFilterViewModel : ViewModelBase
    {
        #region Fields

        private ICommand addSetsCommand;
        private ListBox allSetsListBox;
        private ObservableCollection<string> allSetNames;
        private List<string> allSetNamesPool;
        private ICommand cancelCommand;
        private ListView filterSetsListBox;
        private ObservableCollection<SetFilter> filterSetNames;
        private bool isAllChecked;
        private bool isAlchemyChecked;
        private bool isHistoricChecked;
        private bool isStandardChecked;
        private ICommand okCommand;
        private MessageBoxResult result;
        private ICommand removeSetsCommand;
        private string setSearchText;
        private ICommand showSetFilterCommand;
        private Visibility visibility = Visibility.Collapsed;

        #endregion

        #region Properties

        public ICommand AddSetsCommand => addSetsCommand ??= new RelayCommand(AddSets);

        public ObservableCollection<string> AllSetNames
        {
            get => allSetNames;
            set
            {
                allSetNames = value;
                OnPropertyChanged();
            }
        }

        public ICommand CancelCommand => cancelCommand ??= new RelayCommand(Cancel);

        public ObservableCollection<SetFilter> FilterSetNames
        {
            get => filterSetNames;
            set
            {
                filterSetNames = value;
                OnPropertyChanged();
            }
        }

        public bool IsAllChecked
        {
            get => isAllChecked;
            set
            {
                isAllChecked = value;

                if (value)
                {
                    IsAlchemyChecked = false;
                    IsHistoricChecked = false;
                    IsStandardChecked = false;

                    ManualFilter();
                }

                OnPropertyChanged();
            }
        }

        public bool IsAlchemyChecked
        {
            get => isAlchemyChecked;
            set
            {
                isAlchemyChecked = value;

                if (value)
                {
                    IsAllChecked = false;
                    IsHistoricChecked = false;
                    IsStandardChecked = false;

                    ManualFilter();
                }

                OnPropertyChanged();
            }
        }

        public bool IsHistoricChecked
        {
            get => isHistoricChecked;
            set
            {
                isHistoricChecked = value;

                if (value)
                {
                    IsAlchemyChecked = false;
                    IsAllChecked = false;
                    IsStandardChecked = false;

                    ManualFilter();
                }

                OnPropertyChanged();
            }
        }

        public bool IsStandardChecked
        {
            get => isStandardChecked;
            set
            {
                isStandardChecked = value;

                if (value)
                {
                    IsAlchemyChecked = false;
                    IsAllChecked = false;
                    IsHistoricChecked = false;

                    ManualFilter();
                }

                OnPropertyChanged();
            }
        }

        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok);

        public MessageBoxResult Result
        {
            get => result;
            set
            {
                result = value;
                OnPropertyChanged();
            }
        }

        public ICommand RemoveSetsCommand => removeSetsCommand ??= new RelayCommand(RemoveSets);

        public ICommand ShowSetFilterCommand => showSetFilterCommand ??= new RelayCommand(ShowSetFilter);

        public string SetSearchText
        {
            get => setSearchText;
            set
            {
                setSearchText = value;

                ManualFilter();

                OnPropertyChanged();
            }
        }

        public Visibility Visibility
        {
            get => visibility;
            set
            {
                visibility = value;
                OnPropertyChanged();
            }
        }

        public ListBox AllSetsListBox
        {
            get => allSetsListBox;
            set
            {
                allSetsListBox = value;

                OnPropertyChanged();
            }
        }

        public ListView FilterSetsListBox
        {
            get => filterSetsListBox;
            set
            {
                filterSetsListBox = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        private void AddSets()
        {
            if (AllSetsListBox.SelectedItems.Count == 0)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Missing Selection";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "Please select one or more items from the all sets list on the left to add.";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.Information;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true; // set blocking
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                return;
            }

            List<string> itemsToAdd = AllSetsListBox.SelectedItems.Cast<string>().ToList();

            AddSets(itemsToAdd);
        }

        public void AddSets(List<string> sets)
        {
            foreach (string set in sets)
            {
                // no need for duplicate set names
                if (!FilterSetNames.Any(item => item.Name == set))
                {
                    FilterSetNames.Add(new SetFilter { Name = set });
                }
            }
        }

        private void Cancel()
        {
            // revert to previous filter set if cancel was clicked
            FilterSetNames.Clear();
            FilterSetNames.AddRange(ServiceLocator.Instance.MainWindowViewModel.FilterSetNames.ToList());

            Result = MessageBoxResult.Cancel;

            Visibility = Visibility.Collapsed;
        }

        private void ManualFilter()
        {
            /*
             * Standard filtering methodology did not work. Even though the filter was returning false items were still showing up in the list. Not sure
             * why this was but suspicion is that it had something do to with the "default" view CollectionViewSource.GetDefaultView was returning. It 
             * worked in one spot (adding sets to settings) but not in another (here in set filters). So went with a manual approach instead.
             */

            if (IsAllChecked)
            {
                if (!string.IsNullOrWhiteSpace(SetSearchText))
                {
                    AllSetNames?.Clear();
                    AllSetNames?.AddRange(allSetNamesPool.Where(x => x.Contains(SetSearchText, StringComparison.OrdinalIgnoreCase)).ToList());
                }
                else
                {
                    AllSetNames?.Clear();
                    AllSetNames?.AddRange(allSetNamesPool.ToList());
                }
            }
            else if (IsAlchemyChecked)
            {
                if (!string.IsNullOrWhiteSpace(SetSearchText))
                {
                    AllSetNames?.Clear();
                    AllSetNames?.AddRange(allSetNamesPool.Where(item => ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaAlchemyOnlySetNames.Any(item2 => item == item2) &&
                        item.Contains(SetSearchText, StringComparison.OrdinalIgnoreCase)));
                }
                else
                {
                    AllSetNames?.Clear();
                    AllSetNames?.AddRange(allSetNamesPool.Where(item => ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaAlchemyOnlySetNames.Any(item2 => item == item2)));
                }
            }
            else if (IsHistoricChecked)
            {
                if (!string.IsNullOrWhiteSpace(SetSearchText))
                {
                    AllSetNames?.Clear();
                    AllSetNames?.AddRange(allSetNamesPool.Where(item => ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaHistoricOnlySetNames.Any(item2 => item == item2) &&
                        item.Contains(SetSearchText, StringComparison.OrdinalIgnoreCase)));
                }
                else
                {
                    AllSetNames?.Clear();
                    AllSetNames?.AddRange(allSetNamesPool.Where(item => ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaHistoricOnlySetNames.Any(item2 => item == item2)));
                }
            }
            else if (isStandardChecked)
            {
                if (!string.IsNullOrWhiteSpace(SetSearchText))
                {
                    AllSetNames?.Clear();
                    AllSetNames?.AddRange(allSetNamesPool.Where(item => ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaStandardOnlySetNames.Any(item2 => item == item2) &&
                        item.Contains(SetSearchText, StringComparison.OrdinalIgnoreCase)));
                }
                else
                {
                    AllSetNames?.Clear();
                    AllSetNames?.AddRange(allSetNamesPool.Where(item => ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaStandardOnlySetNames.Any(item2 => item == item2)));
                }
            }
        }

        private void Ok()
        {
            ServiceLocator.Instance.MainWindowViewModel.FilterSetNames.Clear();
            ServiceLocator.Instance.MainWindowViewModel.FilterSetNames.AddRange(FilterSetNames);

            if (FilterSetNames.Count > 0)
            {
                ServiceLocator.Instance.MainWindowViewModel.SetFilterMessageVisibility = Visibility.Collapsed;

                // see what sets have card images downloaded and which ones do not
                List<SetFilter> setsNotExisting = FilterSetNames.Where(sf => !sf.Exists || (sf.Exists && !sf.AllImagesExistInSet)).ToList();

                // we need to show a custom progress window that shows how many card have been downloaded of X
                if (setsNotExisting.Count == 0)
                {
                    List<SetFilter> setsExisting = FilterSetNames.Where(sf => sf.Exists && sf.AllImagesExistInSet).ToList();

                    foreach (SetFilter setFilter in setsExisting)
                    {
                        ServiceLocator.Instance.MainWindowViewModel.CardCollectionViewModel.Cards.AddRange(ServiceLocator.Instance.MainWindowViewModel.Cards[setFilter.Name]);
                    }

                    Visibility = Visibility.Collapsed;

                    return;
                }

                // show custom progress bar dialog
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.CardDownloadViewModel.SetFilters = setsNotExisting;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.CardDownloadViewModel.Visibility = Visibility.Visible;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.CardDownloadViewModel.BeingDownloading();
            }
            else
            {
                ServiceLocator.Instance.MainWindowViewModel.SetFilterMessageVisibility = Visibility.Visible;
            }

            Result = MessageBoxResult.OK;

            Visibility = Visibility.Collapsed;
        }

        private void RemoveSets()
        {
            if (FilterSetsListBox.SelectedItems.Count == 0)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Missing Selection";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "Please select one or more items from the set filter list on the right to remove.";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.Information;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true; // set blocking
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                return;
            }

            List<SetFilter> itemsToRemove = FilterSetsListBox.SelectedItems.Cast<SetFilter>().ToList();

            RemoveSets(itemsToRemove);
        }

        public void RemoveSets(List<SetFilter> sets)
        {
            foreach (SetFilter set in sets)
            {
                SetFilter match = FilterSetNames.FirstOrDefault(item => item.Name == set.Name);

                if (match != null)
                    FilterSetNames.Remove(match);
            }
        }

        public void ShowSetFilter()
        {
            if (AllSetNames == null)
            {
                IsAllChecked = true;
                AllSetNames = new ObservableCollection<string>(ServiceLocator.Instance.MainWindowViewModel.SetNames.ToList());
                allSetNamesPool = ServiceLocator.Instance.MainWindowViewModel.SetNames.ToList();
                //AllSetNamesCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(AllSetNames);
            }

            FilterSetNames = new ObservableCollection<SetFilter>(ServiceLocator.Instance.MainWindowViewModel.FilterSetNames.ToList());

            Visibility = Visibility.Visible;
        }

        #endregion
    }
}
