using MagicTheGatheringArenaDeckMaster.ViewModels;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace MagicTheGatheringArenaDeckMaster
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? viewModel;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new MainWindowViewModel
            {
                Dispatcher = Dispatcher,
                StatusMessage = "Waiting on data input..."
            };

            InternalDialogUserControlViewModel idVm = new InternalDialogUserControlViewModel(viewModel)
            {
                NeedDataVisibility = Visibility.Visible
            };

            viewModel.InternalDialogViewModel = idVm;

            DataContext = viewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (viewModel == null) return;
            if (viewModel.InternalDialogViewModel == null) return;

            try
            {
                if (Directory.Exists(ServiceLocator.Instance.PathingService.BaseDataPath))
                {
                    string[] files = Directory.GetFiles(ServiceLocator.Instance.PathingService.BaseDataPath, "*.json");

                    if (files.Length == 1)
                    {
                        viewModel.InternalDialogViewModel.FileLocation = files[0]; // there will only ever be one
                        viewModel.InternalDialogViewModel.HasDataPreviously = true;
                    }
                    else if (files.Length > 1)
                    {
                        viewModel.InternalDialogViewModel.FileLocation = files[0]; // there should only ever be one
                        viewModel.InternalDialogViewModel.HasDataPreviously = true;

                        // delete the rest because they aren't supposed to there, we don't want our application getting confused
                        for (int i = 1; i < files.Length; i++)
                        {
                            File.Delete(files[i]);
                        }
                    }
                }

                string? val = Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString();

                viewModel.Version = Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString();
            }
            catch (Exception ex)
            {
                ServiceLocator.Instance.LoggerService.Error($"An error occurred attempting to read the backed up file.{Environment.NewLine}{ex}");
            }
        }
    }
}
