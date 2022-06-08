using MagicTheGatheringArena.Core.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MagicTheGatheringArenaDeckMaster2.ViewModels
{
    internal class SettingsViewModel : ViewModelBase
    {
        #region Fields

        private ICommand cancelCommand;
        private ICommand okCommand;
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

        #endregion

        #region Methods

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

        private void ShowSettings()
        {
            Visibility = Visibility.Visible;
        }

        #endregion
    }
}
