using MagicTheGatheringArena.Core.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private ObservableCollection<string> allSetNames;
        private CollectionView allSetNamesCollectionView;
        private ICommand cancelCommand;
        private ICommand okCommand;
        private MessageBoxResult result;
        private ICommand removeSetsCommand;
        private ObservableCollection<string> settingSetNames;
        private ICommand showAddSetToSettingsCommand;
        private Visibility visibility = Visibility.Collapsed;
        private string title;
        private string setSearchText;
        private ListBox allSetsListBox;
        private ListBox settingsSetsListBox;

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
            // reset the collection of set names for the setting if the user cancel (this throws away any changes they may have made)
            if (Title.Contains("Alchemy", StringComparison.OrdinalIgnoreCase))
            {
                SettingSetNames.Clear();

                foreach (string set in ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaAlchemyOnlySetNames)
                {
                    SettingSetNames.Add(set);
                }
            }
            else if (Title.Contains("Historic", StringComparison.OrdinalIgnoreCase))
            {
                SettingSetNames.Clear();

                foreach (string set in ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaHistoricOnlySetNames)
                {
                    SettingSetNames.Add(set);
                }
            }
            else if (Title.Contains("Standard", StringComparison.OrdinalIgnoreCase))
            {
                SettingSetNames.Clear();

                foreach (string set in ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaStandardOnlySetNames)
                {
                    SettingSetNames.Add(set);
                }
            }

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
            if (Title.Contains("Alchemy", StringComparison.OrdinalIgnoreCase))
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaAlchemyOnlySetNames.Clear();

                foreach (string set in SettingSetNames)
                {
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaAlchemyOnlySetNames.Add(set);
                }
            }
            else if (Title.Contains("Historic", StringComparison.OrdinalIgnoreCase))
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaHistoricOnlySetNames.Clear();

                foreach (string set in SettingSetNames)
                {
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaHistoricOnlySetNames.Add(set);
                }
            }
            else if (Title.Contains("Standard", StringComparison.OrdinalIgnoreCase))
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaStandardOnlySetNames.Clear();

                foreach (string set in SettingSetNames)
                {
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaStandardOnlySetNames.Add(set);
                }
            }

            // todo : persist changes to data source

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
