using MagicTheGatheringArena.Core;
using MagicTheGatheringArena.Core.MVVM;
using MagicTheGatheringArena.Core.Scryfall.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF.InternalDialogs;

namespace MagicTheGatheringArenaDeckMaster.ViewModels
{
    internal class InternalDialogUserControlViewModel : ViewModelBase
    {
        #region Fields

        private ICommand? aboutCommand;
        private Visibility aboutBoxVisibility = Visibility.Collapsed;
        private ICommand? browseCommand;
        private int cardProgressValue;
        private Visibility cardProgressVisibility = Visibility.Collapsed;
        private string fileLocation = string.Empty;
        private bool hasDataPreviously;
        private MessageBoxButton messageBoxButton = MessageBoxButton.OK;
        private MessageBoxInternalDialogImage messageBoxImage = MessageBoxInternalDialogImage.Information;
        private bool messageBoxIsModal;
        private string? messageBoxMessage;
        private MessageBoxResult messageBoxResult;
        private string? messageBoxTitle;
        private Visibility messageBoxVisibility = Visibility.Collapsed;
        private Visibility needDataVisibility = Visibility.Collapsed;
        private ICommand? okDataCommand;
        private Visibility progressDialogVisbility = Visibility.Collapsed;
        private string? progressMessage;
        private string? progressTitle;
        private ICommand? useDataCommand;

        #endregion

        #region Properties

        public ICommand? AboutCommand => aboutCommand ??= new RelayCommand(About);

        public Visibility AboutBoxVisibility
        {
            get => aboutBoxVisibility;
            set
            {
                aboutBoxVisibility = value;
                OnPropertyChanged();
            }
        }

        public ICommand? BrowseCommand => browseCommand ??= new RelayCommand(BrowseForFile);

        public int CardProgressValue
        {
            get => cardProgressValue;
            set
            {
                cardProgressValue = value;
                OnPropertyChanged();
            }
        }

        public Visibility CardProgressVisibility
        {
            get => cardProgressVisibility;
            set
            {
                cardProgressVisibility = value;
                OnPropertyChanged();
            }
        }

        public string FileLocation
        {
            get => fileLocation;
            set
            {
                fileLocation = value;
                OnPropertyChanged();
            }
        }

        public bool HasDataPreviously
        {
            get => hasDataPreviously;
            set
            {
                hasDataPreviously = value;
                OnPropertyChanged();
            }
        }

        public MessageBoxButton MessageBoxButton
        {
            get => messageBoxButton;
            set
            {
                messageBoxButton = value;
                OnPropertyChanged();
            }
        }

        public MessageBoxInternalDialogImage MessageBoxImage
        {
            get => messageBoxImage;
            set
            {
                messageBoxImage = value;
                OnPropertyChanged();
            }
        }

        public bool MessageBoxIsModal
        {
            get => messageBoxIsModal;
            set
            {
                messageBoxIsModal = value;
                OnPropertyChanged();
            }
        }

        public string? MessageBoxMessage
        {
            get => messageBoxMessage;
            set
            {
                messageBoxMessage = value;
                OnPropertyChanged();
            }
        }

        public MessageBoxResult MessageBoxResult
        {
            get => messageBoxResult;
            set
            {
                messageBoxResult = value;
                OnPropertyChanged();
            }
        }

        public string? MessageBoxTitle
        {
            get => messageBoxTitle;
            set
            {
                messageBoxTitle = value;
                OnPropertyChanged();
            }
        }

        public Visibility MessageBoxVisibility
        {
            get => messageBoxVisibility;
            set
            {
                messageBoxVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility NeedDataVisibility
        {
            get => needDataVisibility;
            set
            {
                needDataVisibility = value;
                OnPropertyChanged();
            }
        }

        public ICommand? OkDataCommand => okDataCommand ??= new RelayCommand(OkData, CanOkdata);

        public MainWindowViewModel Parent { get; set; }

        public Visibility ProgressDialogVisbility
        {
            get => progressDialogVisbility;
            set
            {
                progressDialogVisbility = value;
                OnPropertyChanged();
            }
        }

        public string? ProgressMessage
        {
            get => progressMessage;
            set
            {
                progressMessage = value;
                OnPropertyChanged();
            }
        }

        public string? ProgressTitle
        {
            get => progressTitle;
            set
            {
                progressTitle = value;
                OnPropertyChanged();
            }
        }

        public ICommand? UseDataCommand => useDataCommand ??= new RelayCommand(UseData);

        #endregion

        #region Constructors

        public InternalDialogUserControlViewModel(MainWindowViewModel parent)
        {
            Parent = parent;
        }

        #endregion

        #region Methods

        private void About()
        {
            AboutBoxVisibility = Visibility.Visible;
        }

        private void BrowseForFile()
        {
            try
            {
                OpenFileDialog ofd = new()
                {
                    AddExtension = true,
                    CheckFileExists = true,
                    Filter = "JSON files (*.json)|*.json",
                    Multiselect = false,
                    Title = "Select File for Processing"
                };

                bool? result = ofd.ShowDialog();

                if (result.HasValue)
                {
                    if (result.Value)
                    {
                        string selectedFile = ofd.FileName;

                        FileLocation = selectedFile;

                        // even if there was previous data but the user picked a file still, then clear our flag
                        HasDataPreviously = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.Instance.LoggerService.Error($"An error occurred attempting to load in file from file picker.{Environment.NewLine}{ex}");

                MessageBoxTitle = "Browse Error";
                MessageBoxMessage = $"An error occurred attempting to reference the file. {ex.Message}{Environment.NewLine}See the log for more details.";
                MessageBoxVisibility = Visibility.Visible;
            }
        }

        private bool CanOkdata()
        {
            return !string.IsNullOrWhiteSpace(FileLocation);
        }

        private void OkData()
        {
            Parent.ProcessFileData();
        }

        private void UseData()
        {
            Parent.ProcessFileData();
        }

        #endregion
    }
}
