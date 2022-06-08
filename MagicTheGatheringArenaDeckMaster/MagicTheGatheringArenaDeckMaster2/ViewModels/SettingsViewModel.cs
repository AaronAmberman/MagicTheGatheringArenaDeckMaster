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
    internal class SettingsViewModel : ViewModelBase
    {
        #region Fields

        private ICommand addAlchemySetCommand;
        private ICommand addHistoricSetCommand;
        private ICommand addStandardSetCommand;
        private ICommand cancelCommand;
        private ICommand okCommand;
        private ICommand removeAlchemySetCommand;
        private ICommand removeHistoricSetCommand;
        private ICommand removeStandardSetCommand;
        private MessageBoxResult result;
        private ICommand showSettingsCommand;
        private Visibility visibility = Visibility.Collapsed;

        private ObservableCollection<string> arenaStandardOnlySetNames = new ObservableCollection<string>
        {
            // add to a database later

            "Adventures in the Forgotten Realms",
            "Innistrad: Crimson Vow",
            "Innistrad: Midnight Hunt",
            "Kaldheim",
            "Strixhaven: School of Mages",
            "Zendikar Rising",
            "Kamigawa: Neon Dynasty",
            "Streets of New Capenna"
        };

        private ObservableCollection<string> arenaAlchemyOnlySetNames = new ObservableCollection<string>
        {
            // add to a database later

            "Alchemy: Innistrad",
            "Alchemy: Kamigawa",
            "Alchemy: New Capenna",
            "Rebalanced",
            "Arena Base Set",
            "Adventures in the Forgotten Realms",
            "Innistrad: Crimson Vow",
            "Innistrad: Midnight Hunt",
            "Kaldheim",
            "Strixhaven: School of Mages",
            "Zendikar Rising",
            "Kamigawa: Neon Dynasty",
            "Streets of New Capenna"
        };

        private ObservableCollection<string> arenaHistoricOnlySetNames = new ObservableCollection<string>
        {
            // add to a database later

            "Core 2021",
            "Ikoria Lair of Behemoths",
            "Theros Beyond Death",
            "Theros of Eldraine",
            "Core Set 2020",
            "War of the Spark",
            "Ravnica Allegiance",
            "Guilds of Ravnica",
            "Core Set 2019",
            "Dominaria",
            "Rivals of Ixalan",
            "Amonkhet Remastered",
            "Kaladesh Remastered",
            "Historic Anthology",
            "Historic Anthology2",
            "Historic Anthology3",
            "Historic Anthology4",
            "Historic Anthology5",
            "Jumpstart",
            "Jumpstart: Historic Horizons",
            "Mystical Archive"
        };

        #endregion

        #region Properties

        public ObservableCollection<string> ArenaStandardOnlySetNames
        {
            get => arenaStandardOnlySetNames;
            set
            {
                arenaStandardOnlySetNames = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> ArenaAlchemyOnlySetNames
        {
            get => arenaAlchemyOnlySetNames;
            set
            {
                arenaAlchemyOnlySetNames = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> ArenaHistoricOnlySetNames
        {
            get => arenaHistoricOnlySetNames;
            set
            {
                arenaHistoricOnlySetNames = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddAlchemySetCommand => addAlchemySetCommand ??= new RelayCommand(AddAlchemySet);

        public ICommand AddHistoricSetCommand => addHistoricSetCommand ??= new RelayCommand(AddHistoricSet);

        public ICommand AddStandardSetCommand => addStandardSetCommand ??= new RelayCommand(AddStandardSet);

        public ICommand CancelCommand => cancelCommand ??= new RelayCommand(Cancel);

        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok);

        public ICommand RemoveAlchemySetCommand => removeAlchemySetCommand ??= new RelayCommand(RemoveAlchemySet);

        public ICommand RemoveHistoricSetCommand => removeHistoricSetCommand ??= new RelayCommand(RemoveHistoricSet);

        public ICommand RemoveStandardSetCommand => removeStandardSetCommand ??= new RelayCommand(RemoveStandardSet);

        public MessageBoxResult Result
        {
            get => result;
            set
            {
                result = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowSettingsCommand => showSettingsCommand ??= new RelayCommand(ShowSettings);

        public Visibility Visibility
        {
            get => visibility;
            set
            {
                visibility = value;
                OnPropertyChanged();
            }
        }

        // set on load of PopupDialogUserControl
        public ListBox AlchemyListBox { get; set; }
        public ListBox HistoricListBox { get; set; }
        public ListBox StandardListBox { get; set; }

        #endregion

        #region Methods

        private void AddAlchemySet()
        {
            if (ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.AllSetNames == null)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.AllSetNames =
                    new ObservableCollection<string>(ServiceLocator.Instance.MainWindowViewModel.SetNames.ToList());

                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.AllSetNamesCollectionView =
                    (CollectionView)CollectionViewSource.GetDefaultView(ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.AllSetNames);
            }

            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.SettingSetNames =
                new ObservableCollection<string>(ArenaAlchemyOnlySetNames.ToList());

            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.Title = "Add Set To Settings (Alchemy)";
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.Visibility = Visibility.Visible;
        }

        private void AddHistoricSet()
        {
            if (ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.AllSetNames == null)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.AllSetNames =
                    new ObservableCollection<string>(ServiceLocator.Instance.MainWindowViewModel.SetNames.ToList());

                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.AllSetNamesCollectionView =
                    (CollectionView)CollectionViewSource.GetDefaultView(ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.AllSetNames);
            }

            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.SettingSetNames =
                new ObservableCollection<string>(ArenaHistoricOnlySetNames.ToList());

            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.Title = "Add Set To Settings (Historic)";
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.Visibility = Visibility.Visible;
        }

        private void AddStandardSet()
        {
            if (ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.AllSetNames == null)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.AllSetNames =
                    new ObservableCollection<string>(ServiceLocator.Instance.MainWindowViewModel.SetNames.ToList());

                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.AllSetNamesCollectionView =
                    (CollectionView)CollectionViewSource.GetDefaultView(ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.AllSetNames);
            }

            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.SettingSetNames =
                new ObservableCollection<string>(ArenaStandardOnlySetNames.ToList());

            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.Title = "Add Set To Settings (Standard)";
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.Visibility = Visibility.Visible;
        }

        private void Cancel()
        {
            Result = MessageBoxResult.Cancel;

            Visibility = Visibility.Collapsed;
        }

        private void Ok()
        {
            Result = MessageBoxResult.OK;

            Visibility = Visibility.Collapsed;
        }

        private void RemoveAlchemySet()
        {
            if (AlchemyListBox.SelectedItems.Count == 0)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Missing Selection";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "Please select one or more items from the alchemy list to remove.";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.Information;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true; // set blocking
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();
            }

            List<string> selectedItems = AlchemyListBox.SelectedItems.Cast<string>().ToList();

            foreach (string setName in selectedItems)
            {
                foreach (string sn in ArenaAlchemyOnlySetNames)
                {
                    if (sn == setName)
                    {
                        ArenaAlchemyOnlySetNames.Remove(sn);

                        // we found our match, iterate the next outer loop
                        break;
                    }
                }
            }

            // todo : persist changes to data source
        }

        private void RemoveHistoricSet()
        {
            if (HistoricListBox.SelectedItems.Count == 0)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Missing Selection";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "Please select one or more items from the historic list to remove.";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.Information;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true; // set blocking
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();
            }

            List<string> selectedItems = HistoricListBox.SelectedItems.Cast<string>().ToList();

            foreach (string setName in HistoricListBox.SelectedItems)
            {
                foreach (string sn in ArenaHistoricOnlySetNames)
                {
                    if (sn == setName)
                    {
                        ArenaHistoricOnlySetNames.Remove(sn);

                        // we found our match, iterate the next outer loop
                        break;
                    }
                }
            }

            // todo : persist changes to data source
        }

        private void RemoveStandardSet()
        {
            if (StandardListBox.SelectedItems.Count == 0)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Missing Selection";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "Please select one or more items from the standard list to remove.";
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.Information;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true; // set blocking
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                ServiceLocator.Instance.MainWindowViewModel.ClearOutMessageBoxDialog();
            }

            List<string> selectedItems = StandardListBox.SelectedItems.Cast<string>().ToList();

            foreach (string setName in StandardListBox.SelectedItems)
            {
                foreach (string sn in ArenaStandardOnlySetNames)
                {
                    if (sn == setName)
                    {
                        ArenaStandardOnlySetNames.Remove(sn);

                        // we found our match, iterate the next outer loop
                        break;
                    }

                }
            }

            // todo : persist changes to data source
        }

        private void ShowSettings()
        {
            Visibility = Visibility.Visible;
        }

        #endregion
    }
}
