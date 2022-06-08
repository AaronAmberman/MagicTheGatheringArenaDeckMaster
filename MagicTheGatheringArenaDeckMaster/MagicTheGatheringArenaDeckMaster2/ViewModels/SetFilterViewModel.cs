using MagicTheGatheringArena.Core.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MagicTheGatheringArenaDeckMaster2.ViewModels
{
    internal class SetFilterViewModel : ViewModelBase
    {
        #region Fields

        private ICommand cancelCommand;
        private ICommand okCommand;
        private MessageBoxResult result;
        private ICommand showSetFilterCommand;
        private Visibility visibility = Visibility.Collapsed;

        #endregion

        #region Properties

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

        public ICommand ShowSetFilterCommand => showSetFilterCommand ??= new RelayCommand(ShowSetFilter);

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

        private void ShowSetFilter()
        {
            Visibility = Visibility.Visible;
        }

        #endregion
    }
}
