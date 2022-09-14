using MagicTheGatheringArena.Core.MVVM;
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

        private ObservableCollection<string> arenaStandardOnlySetNamesOrig = new ObservableCollection<string>
        {
            // these should be whatever is in the database (or this list if the database is null)
            "Dominaria",
            "Innistrad: Crimson Vow",
            "Innistrad: Midnight Hunt",
            "Kamigawa: Neon Dynasty",
            "Streets of New Capenna"
        };

        private ObservableCollection<string> arenaAlchemyOnlySetNamesOrig = new ObservableCollection<string>
        {
            // these should be whatever is in the database (or this list if the database is null)
            "Rebalanced",
            "Alchemy Horizons: Baldur's Gate",
            "Alchemy: Kamigawa",
            "Alchemy: New Capenna",
            "Alchemy: Innistrad",
            "Rebalanced",
            "Arena Base Set",
        };

        private ObservableCollection<string> arenaHistoricOnlySetNamesOrig = new ObservableCollection<string>
        {
            // these should be whatever is in the database (or this list if the database is null)
            "Adventures in the Forgotten Realms",
            "Historic Anthology",
            "Historic Anthology2",
            "Historic Anthology3",
            "Historic Anthology4",
            "Historic Anthology5",
            "Historic Anthology6",
            "Amonkhet Remastered",
            "Dominaria",
            "Explorer Anthology 1",
            "Throne of Eldraine",
            "Guilds of Ravnica",
            "Ikoria Lair of Behemoths",
            "Jumpstart: Historic Horizons",
            "Jumpstart",
            "Kaldheim",
            "Kaladesh Remastered",
            "Core Set 2019",
            "Core Set 2020",
            "Core Set 2021",
            "Rivals of Ixalan",
            "Ravnica Allegiance",
            "Mystical Archive",
            "Strixhaven",
            "Theros Beyond Death",
            "War of the Spark",
            "Ixanlan",
            "Zendikar Rising"
        };

        private ObservableCollection<string> arenaStandardOnlySetNames;
        private ObservableCollection<string> arenaAlchemyOnlySetNames;
        private ObservableCollection<string> arenaHistoricOnlySetNames;

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

        public List<string> AlchemySetsFromAdding { get; set; } = new List<string>();
        public List<string> HistoricSetsFromAdding { get; set; } = new List<string>();
        public List<string> StandardSetsFromAdding { get; set; } = new List<string>();

        #endregion

        #region Constructors

        public SettingsViewModel()
        {
            arenaStandardOnlySetNames = new ObservableCollection<string>(arenaStandardOnlySetNamesOrig);
            arenaAlchemyOnlySetNames = new ObservableCollection<string>(arenaAlchemyOnlySetNamesOrig);
            arenaHistoricOnlySetNames = new ObservableCollection<string>(arenaHistoricOnlySetNamesOrig);
        }

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
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.Mode = 0;
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
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.Mode = 1;
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
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.Mode = 2;
            ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.AddSetToSettingsViewModel.Visibility = Visibility.Visible;
        }

        private void Cancel()
        {
            // clear changes to set collections
            AlchemySetsFromAdding.Clear();
            HistoricSetsFromAdding.Clear();
            StandardSetsFromAdding.Clear();

            ArenaStandardOnlySetNames.Clear();
            ArenaStandardOnlySetNames.AddRange(arenaStandardOnlySetNamesOrig.ToList());

            ArenaAlchemyOnlySetNames.Clear();
            ArenaAlchemyOnlySetNames.AddRange(arenaAlchemyOnlySetNamesOrig.ToList());

            ArenaHistoricOnlySetNames.Clear();
            ArenaHistoricOnlySetNames.AddRange(arenaHistoricOnlySetNamesOrig.ToList());

            Result = MessageBoxResult.Cancel;

            Visibility = Visibility.Collapsed;
        }

        private void Ok()
        {
            if (AlchemySetsFromAdding.Count > 0)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaAlchemyOnlySetNames.Clear();

                foreach (string set in AlchemySetsFromAdding)
                {
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaAlchemyOnlySetNames.Add(set);
                }
            }

            if (HistoricSetsFromAdding.Count > 0)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaHistoricOnlySetNames.Clear();

                foreach (string set in HistoricSetsFromAdding)
                {
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaHistoricOnlySetNames.Add(set);
                }
            }

            if (StandardSetsFromAdding.Count > 0)
            {
                ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaStandardOnlySetNames.Clear();

                foreach (string set in StandardSetsFromAdding)
                {
                    ServiceLocator.Instance.MainWindowViewModel.PopupDialogViewModel.SettingsViewModel.ArenaStandardOnlySetNames.Add(set);
                }
            }

            // todo : persist changes to data source

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
