using MagicTheGatheringArena.Core.MVVM;
using System.Windows;
using System.Windows.Input;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class AboutDialogViewModel : ViewModelBase
    {
        #region Fields

        private ICommand aboutCommand;
        private Visibility aboutBoxVisibility = Visibility.Collapsed;
        private string version;

        #endregion

        #region Properties

        public ICommand AboutCommand => aboutCommand ??= new RelayCommand(About);

        public Visibility AboutBoxVisibility
        {
            get => aboutBoxVisibility;
            set
            {
                aboutBoxVisibility = value;
                OnPropertyChanged();
            }
        }

        public string Version 
        { 
            get => version; 
            set
            {
                version = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        private void About()
        {
            AboutBoxVisibility = Visibility.Visible;
        }

        #endregion
    }
}
