using MagicTheGatheringArena.Core.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPF.InternalDialogs;

namespace MagicTheGatheringArenaDeckMaster2.ViewModels
{
    internal class AddSetToSettingsViewModel : ViewModelBase
    {
        #region Fields

        private ICommand addSetsCommand;
        private ListBox allSetsListBox;
        private ObservableCollection<string> allSetNames;
        private CollectionView allSetNamesCollectionView;
        private ICommand cancelCommand;
        private ICommand okCommand;
        private MessageBoxResult result;
        private ICommand removeSetsCommand;
        private string setSearchText;
        private ListBox settingsSetsListBox;
        private ObservableCollection<string> settingSetNames;
        private ICommand showAddSetToSettingsCommand;
        private string title;
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

        public CollectionView AllSetNamesCollectionView
        {
            get => allSetNamesCollectionView;
            set
            {
                allSetNamesCollectionView = value;

                if (allSetNamesCollectionView != null)
                {
                    allSetNamesCollectionView.Filter = FilterSetNames;
                }

                OnPropertyChanged();
            }
        }

        public ICommand CancelCommand => cancelCommand ??= new RelayCommand(Cancel);

        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok);

        public int Mode { get; set; }

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

        public ICommand ShowAddSetToSettingsCommand => showAddSetToSettingsCommand ??= new RelayCommand(ShowAddSetToSettings);

        public string SetSearchText
        {
            get => setSearchText;
            set
            {
                setSearchText = value;

                AllSetNamesCollectionView?.Refresh();

                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> SettingSetNames
        {
            get => settingSetNames;
            set
            {
                settingSetNames = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
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

        public ListBox SettingsSetsListBox 
        { 
            get => settingsSetsListBox; 
            set
            { 
                settingsSetsListBox = value;
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
            foreach (string item in sets)
            {
                // no need for duplicate set names
                if (!SettingSetNames.Contains(item))
                {
                    SettingSetNames.Add(item);
                }
            }
        }

        private void Cancel()
        {
            // reset the collection of set names for the setting if the user cancels (this throws away any changes they may have made)
            if (Mode == 0)
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.AlchemySetsFromAdding.Clear();
            else if (Mode == 1)
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.HistoricSetsFromAdding.Clear();
            else if (Mode == 3)
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.StandardSetsFromAdding.Clear();

            Result = MessageBoxResult.Cancel;

            Visibility = Visibility.Collapsed;
        }

        private bool FilterSetNames(object obj)
        {
            if (string.IsNullOrWhiteSpace(SetSearchText)) 
                return true;
            else
            {
                string value = obj?.ToString();

                if (string.IsNullOrWhiteSpace(value)) return false;
                else
                {
                    if (value.Contains(SetSearchText, StringComparison.OrdinalIgnoreCase)) return true;
                    else return false;
                }
            }
        }

        private void Ok()
        {
            if (Mode == 0)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.AlchemySetsFromAdding.Clear();
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaAlchemyOnlySetNames.Clear();

                foreach (string set in SettingSetNames)
                {
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.AlchemySetsFromAdding.Add(set);
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaAlchemyOnlySetNames.Add(set);
                }
            }
            else if (Mode == 1)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.HistoricSetsFromAdding.Clear();
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaHistoricOnlySetNames.Clear();

                foreach (string set in SettingSetNames)
                {
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.HistoricSetsFromAdding.Add(set);
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaHistoricOnlySetNames.Add(set);
                }
            }
            else if (Mode == 2)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.StandardSetsFromAdding.Clear();
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaStandardOnlySetNames.Clear();

                foreach (string set in SettingSetNames)
                {
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.StandardSetsFromAdding.Add(set);
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaStandardOnlySetNames.Add(set);
                }
            }

            Result = MessageBoxResult.OK;

            Visibility = Visibility.Collapsed;
        }

        private void RemoveSets()
        {
            if (SettingsSetsListBox.SelectedItems.Count == 0)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Missing Selection";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "Please select one or more items from the settings list on the right to remove.";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.Information;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true; // set blocking
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();

                return;
            }

            List<string> itemsToRemove = SettingsSetsListBox.SelectedItems.Cast<string>().ToList();

            RemoveSets(itemsToRemove);
        }

        public void RemoveSets(List<string> sets)
        {
            foreach (string set in sets)
            {
                SettingSetNames.Remove(set);
            }
        }

        private void ShowAddSetToSettings()
        {
            Visibility = Visibility.Visible;
        }

        #endregion
    }
}
