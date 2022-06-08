using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MagicTheGatheringArenaDeckMaster2
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // do something on start up?
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            try
            {
                ServiceLocator.Instance.LoggerService.Error($"An unhandled exception occurred. Details:{Environment.NewLine}{e.Exception}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred ensuring the log file exists or an error occurred trying to write to the log file.{Environment.NewLine}{ex}");
            }

            // we don't know what happened, tell the user and carry on
            MessageBox.Show("An unhandled exception occurred in the application. We have logged it. Please see log for further details.",
                "Unhandled Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
