using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace MagicTheGatheringArenaDeckMaster
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                // check to make sure our data folder exist and if not create them
                if (!Directory.Exists(ServiceLocator.Instance.PathingService.BaseDataPath))
                {
                    Directory.CreateDirectory(ServiceLocator.Instance.PathingService.BaseDataPath);
                }

                if (!Directory.Exists(ServiceLocator.Instance.PathingService.CardImagePath))
                {
                    Directory.CreateDirectory(ServiceLocator.Instance.PathingService.CardImagePath);
                }
            }
            catch (Exception ex)
            {
                ServiceLocator.Instance.LoggerService.Error($"An exception occurred attempting to create the directories needed for our application to run.{Environment.NewLine}{ex}");
                
                MessageBox.Show($"An error occurred attempting to create the directories we need for our application. {ex.Message}. Exiting.", 
                    "Error Creating Directories", MessageBoxButton.OK, MessageBoxImage.Error);

                Current.Shutdown(-1);
            }
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            ServiceLocator.Instance.LoggerService.Error($"An unhandled exception occurred. Details:{Environment.NewLine}{e.Exception}");

            // we don't know what happened, tell the user and carry on
            MessageBox.Show("An unhandled exception occurred in the application. We have logged it. Please see log for further details.", 
                "Unhandled Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
