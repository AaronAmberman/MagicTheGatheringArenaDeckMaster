using MagicTheGatheringArenaDeckMaster.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using WPF.InternalDialogs;

namespace MagicTheGatheringArenaDeckMaster
{
    public partial class MainWindow : Window
    {
        MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Instance.LoggerService.LogFile = Path.Combine(ServiceLocator.Instance.PathingService.BaseDataPath, "Logs", "DeckMaster.log");
            ServiceLocator.Instance.LoggerService.LogRollSize = 10240; // 10 MB

            viewModel = new MainWindowViewModel
            {
                Dispatcher = Dispatcher,
                CardCollectionViewModel = new CardCollectionViewModel
                {
                    CardListBox = cardListBox
                },
                PopupDialogViewModel = new PopupDialogViewModel
                {
                    AddSetToSettingsViewModel = new AddSetToSettingsViewModel(),
                    AboutDialogViewModel = new AboutDialogViewModel(),
                    CardDownloadViewModel = new CardDownloadViewModel(),
                    DataViewModel = new DataViewModel(),
                    MessageBoxViewModel = new MessageBoxViewModel(),
                    ProgressViewModel = new ProgressViewModel(),
                    SetFilterViewModel = new SetFilterViewModel(),
                    SettingsViewModel = new SettingsViewModel(),
                }
            };

            ServiceLocator.Instance.MainWindowViewModel = viewModel;

            try
            {
                viewModel.PopupDialogViewModel.AboutDialogViewModel.Version = Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred attempting to get the version information.{Environment.NewLine}{ex}");
                ServiceLocator.Instance.LoggerService.Error($"An error occurred attempting to get the version information.{Environment.NewLine}{ex}");

                viewModel.PopupDialogViewModel.AboutDialogViewModel.Version = "Unable to retrieve version information.";
            }

            try
            {
                // if we have a previous card data file, enable offline mode
                if (File.Exists(ServiceLocator.Instance.PathingService.CardDataFile))
                {
                    viewModel.PopupDialogViewModel.DataViewModel.OfflineModeVisibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred attempting to determine if an existing card data file existed.{Environment.NewLine}{ex}");
                ServiceLocator.Instance.LoggerService.Error($"An error occurred attempting to determine if an existing card data file existed.{Environment.NewLine}{ex}");
            }

            DataContext = viewModel;

            if (!ServiceLocator.Instance.PathingService.EnsureDirectories(ServiceLocator.Instance.LoggerService))
            {
                viewModel.StatusMessage = "Error. Exiting application";
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Needed Directories Missing";
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "Could not create the needed directories for the application to run. Exiting.";
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true;
                viewModel.PopupDialogViewModel.MessageBoxViewModel.CloseAction = () => 
                {
                    Application.Current.Shutdown(-1);
                };
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                return;
            }

            ServiceLocator.Instance.DatabaseService.ConnectionString = $"Data Source={ServiceLocator.Instance.PathingService.DatabaseFile}";

            if (!ServiceLocator.Instance.DatabaseService.EnsureDatabase(ServiceLocator.Instance.LoggerService))
            {
                viewModel.StatusMessage = "Error. Exiting application";
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxTitle = "Database Initialization Error";
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxMessage = "Unable to ensure the existence of the database or create the tables. Exiting.";
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxButton = MessageBoxButton.OK;
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxImage = MessageBoxInternalDialogImage.CriticalError;
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxIsModal = true;
                viewModel.PopupDialogViewModel.MessageBoxViewModel.CloseAction = () =>
                {
                    Application.Current.Shutdown(-1);
                };
                viewModel.PopupDialogViewModel.MessageBoxViewModel.MessageBoxVisibility = Visibility.Visible;

                return;
            }

            viewModel.PopupDialogViewModel.DataViewModel.Visibility = Visibility.Visible;
        }
    }
}
