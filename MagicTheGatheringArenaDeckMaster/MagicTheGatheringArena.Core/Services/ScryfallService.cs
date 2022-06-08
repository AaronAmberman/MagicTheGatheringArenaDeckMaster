using MagicTheGatheringArena.Core.Scryfall.Data;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace MagicTheGatheringArena.Core.Services
{
    public class ScryfallService
    {
        private readonly LoggerService logger;
        private readonly PathingService pathing;

        public event EventHandler<string> ImageProcessed;

        public ScryfallService(LoggerService loggerService, PathingService pathingService)
        {
            logger = loggerService;
            pathing = pathingService;
        }

        public BulkDataType GetUniqueArtworkUri()
        {
            try
            {
                HttpClient client = new HttpClient();
                
                HttpResponseMessage response = client.GetAsync("https://api.scryfall.com/bulk-data").Result;
                response.EnsureSuccessStatusCode();

                string data = response.Content.ReadAsStringAsync().Result;

                BulkDataTypeWrapper results = JsonConvert.DeserializeObject<BulkDataTypeWrapper>(data);

                BulkDataType uniqueArtwork = results.data.FirstOrDefault(bdi => bdi.name == "Unique Artwork");

                response.Dispose();
                client.Dispose();

                return uniqueArtwork;
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred attempting to get the unique artwork URI.{Environment.NewLine}{ex}");

                return null;
            }
        }

        public void DownloadUniqueArtworkFile(BulkDataType bulkDataType, string fullFilePath)
        {
            try
            {
                HttpClient client = new HttpClient();

                HttpResponseMessage response = client.GetAsync(bulkDataType.download_uri, HttpCompletionOption.ResponseHeadersRead).Result;
                response.EnsureSuccessStatusCode();

                Stream readStream = response.Content.ReadAsStreamAsync().Result;
                FileStream writeStream = File.Open(fullFilePath, FileMode.Create);

                readStream.CopyTo(writeStream);

                writeStream.Close();
                writeStream.Dispose();
                readStream.Dispose();
                response.Dispose();
                client.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred attempting to download the unique artwork file.{Environment.NewLine}{ex}");
            }
        }

        public void DownloadArtworkFile(UniqueArtType uniqueDataType, string setPath)
        {
            try
            {
                // nothing to download
                if (uniqueDataType.image_uris == null) return;

                string name = uniqueDataType.name_field.ReplaceBadWindowsCharacters();
                string fullName = name + ".png";
                string fullPath = Path.Combine(setPath, name);
                string absolutePathToFile = Path.Combine(fullPath, fullName);

                // only download the image if we don't have it
                if (!File.Exists(absolutePathToFile))
                {
                    HttpClient client = new HttpClient();

                    HttpResponseMessage response = client.GetAsync(uniqueDataType.image_uris.png, HttpCompletionOption.ResponseHeadersRead).Result;
                    response.EnsureSuccessStatusCode();

                    byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result;

                    // make sure our directory exists
                    if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);

                    File.WriteAllBytes(absolutePathToFile, imageBytes);
                }

                ImageProcessed?.Invoke(this, absolutePathToFile);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred attempting to download the unique artwork file for {uniqueDataType.name_field}.{Environment.NewLine}{ex}");
            }
        }
    }
}
