using MagicTheGatheringArenaDeckMaster.ViewModels;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace MagicTheGatheringArenaDeckMaster
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? viewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = new MainWindowViewModel
            {
                Dispatcher = Dispatcher,
                NeedDataVisibility = Visibility.Visible,
                StatusMessage = "Waiting on data input..."
            };

            try
            {
                if (Directory.Exists(ServiceLocator.Instance.PathingService.BaseDataPath))
                {
                    string[] files = Directory.GetFiles(ServiceLocator.Instance.PathingService.BaseDataPath, "*.json");

                    if (files.Length == 1)
                    {
                        viewModel.FileLocation = files[0]; // there will only ever be one
                        viewModel.HasDataPreviously = true;
                    }
                    else if (files.Length > 1)
                    {
                        viewModel.FileLocation = files[0]; // there should only ever be one
                        viewModel.HasDataPreviously = true;

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

            DataContext = viewModel;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox? cb = e.OriginalSource as CheckBox;

            if (cb == null) return;

            // we want the user to not be able to click the header check box into a null/partial check state
            // with mouse clicks they can only set it to checked or unchecked not partial
            // we do this here so it only happens when the user clicks the mouse not when we set the state programmatically
            // this is true for any column in the list view that has a check box in the header
            if (!cb.IsChecked.HasValue)
                cb.IsChecked = false;
        }
    }
}
