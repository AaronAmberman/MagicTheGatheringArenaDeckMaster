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

        public ScryfallService(LoggerService loggerService)
        {
            logger = loggerService;
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

        public bool DownloadArtworkFile(UniqueArtType uniqueArtType, string setPath)
        {
            try
            {
                // nothing to download
                if (uniqueArtType.image_uris == null) return false;

                string name = uniqueArtType.name_field.ReplaceBadWindowsCharacters();
                string fullName = name + ".png";
                string absolutePathToFile = Path.Combine(setPath, fullName);

                // only download the image if we don't have it
                if (!File.Exists(absolutePathToFile))
                {
                    HttpClient client = new HttpClient();
                    client.Timeout = new TimeSpan(0, 0, 30);

                    HttpResponseMessage response = client.GetAsync(uniqueArtType.image_uris.png, HttpCompletionOption.ResponseHeadersRead).Result;
                    response.EnsureSuccessStatusCode();

                    byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result;

                    // make sure our directory exists
                    if (!Directory.Exists(setPath)) Directory.CreateDirectory(setPath);

                    File.WriteAllBytes(absolutePathToFile, imageBytes);

                    return true;
                }
                else return true; // we already have it
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred attempting to download the unique artwork file for {uniqueArtType.name_field}.{Environment.NewLine}{ex}");

                return false;
            }
        }
    }
}
