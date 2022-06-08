using System;
using System.Diagnostics;
using System.IO;

namespace MagicTheGatheringArena.Core.Services
{
    public class PathingService
    {
        public string BaseDataPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MTGA-DeckMaster");

        public string CardImagePath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MTGA-DeckMaster", "CardImages");

        public string LogPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MTGA-DeckMaster", "Logs");

        public bool EnsureDirectories(LoggerService loggerService)
        {
            try
            {
                // check to make sure our data folder exist and if not create them
                if (!Directory.Exists(BaseDataPath))
                {
                    Directory.CreateDirectory(BaseDataPath);

                    loggerService.Info($"Created {BaseDataPath}");
                }

                if (!Directory.Exists(CardImagePath))
                {
                    Directory.CreateDirectory(CardImagePath);

                    loggerService.Info($"Created {CardImagePath}");
                }

                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);

                    loggerService.Info($"Created {LogPath}");
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred attempting to create the directories {BaseDataPath} or {CardImagePath} or {LogPath}.{Environment.NewLine}{ex}");
                loggerService.Error($"An exception occurred attempting to create the directories {BaseDataPath} or {CardImagePath} or {LogPath}.{Environment.NewLine}{ex}");

                return false;
            }
        }
    }
}
