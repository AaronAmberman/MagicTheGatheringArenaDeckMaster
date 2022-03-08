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

        public event EventHandler<int> ImageProcessed;

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

        public void DownloadArtworkFiles(UniqueArtType uniqueDataType, bool small, bool normal, bool large, bool png, bool artCrop, bool borderCrop, string setPath)
        {
            try
            {
                // nothing to download
                if (uniqueDataType.image_uris == null) return;

                if (small)
                {
                    int index = uniqueDataType.image_uris.small.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1;
                    int index2 = uniqueDataType.image_uris.small.LastIndexOf("?", StringComparison.OrdinalIgnoreCase);
                    int count = index2 - index;

                    //string name = uniqueDataType.image_uris.small.Substring(index, count).Replace(".jpg", "");
                    string name = uniqueDataType.name_field.ReplaceBadWindowsCharacters();
                    string fullName = name + "-small.jpg";
                    string fullPath = Path.Combine(setPath, name);
                    string absolutePathToFile = Path.Combine(fullPath, fullName);

                    // only download the image if we don't have it
                    if (!File.Exists(absolutePathToFile))
                    {
                        HttpClient client = new HttpClient();

                        HttpResponseMessage response = client.GetAsync(uniqueDataType.image_uris.small, HttpCompletionOption.ResponseHeadersRead).Result;
                        response.EnsureSuccessStatusCode();

                        byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result;

                        // make sure our directory exists
                        if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);

                        File.WriteAllBytes(absolutePathToFile, imageBytes);
                    }

                    ImageProcessed?.Invoke(this, 1); // we processed one image so just send 1 along
                }

                if (normal)
                {
                    int index = uniqueDataType.image_uris.normal.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1;
                    int index2 = uniqueDataType.image_uris.normal.LastIndexOf("?", StringComparison.OrdinalIgnoreCase);
                    int count = index2 - index;

                    //string name = uniqueDataType.image_uris.normal.Substring(index, count).Replace(".jpg", "");
                    string name = uniqueDataType.name_field.ReplaceBadWindowsCharacters();
                    string fullName = name + "-normal.jpg";
                    string fullPath = Path.Combine(setPath, name);
                    string absolutePathToFile = Path.Combine(fullPath, fullName);

                    // only download the image if we don't have it
                    if (!File.Exists(absolutePathToFile))
                    {
                        HttpClient client = new HttpClient();

                        HttpResponseMessage response = client.GetAsync(uniqueDataType.image_uris.normal, HttpCompletionOption.ResponseHeadersRead).Result;
                        response.EnsureSuccessStatusCode();

                        byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result;

                        // make sure our directory exists
                        if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);

                        File.WriteAllBytes(absolutePathToFile, imageBytes);
                    }

                    ImageProcessed?.Invoke(this, 1); // we processed one image so just send 1 along
                }

                if (large)
                {
                    int index = uniqueDataType.image_uris.large.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1;
                    int index2 = uniqueDataType.image_uris.large.LastIndexOf("?", StringComparison.OrdinalIgnoreCase);
                    int count = index2 - index;

                    string name = uniqueDataType.name_field.ReplaceBadWindowsCharacters();
                    string fullName = name + "-large.jpg";
                    string fullPath = Path.Combine(setPath, name);
                    string absolutePathToFile = Path.Combine(fullPath, fullName);

                    // only download the image if we don't have it
                    if (!File.Exists(absolutePathToFile))
                    {
                        HttpClient client = new HttpClient();

                        HttpResponseMessage response = client.GetAsync(uniqueDataType.image_uris.large, HttpCompletionOption.ResponseHeadersRead).Result;
                        response.EnsureSuccessStatusCode();

                        byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result;

                        // make sure our directory exists
                        if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);

                        File.WriteAllBytes(absolutePathToFile, imageBytes);
                    }

                    ImageProcessed?.Invoke(this, 1); // we processed one image so just send 1 along
                }

                if (png)
                {
                    int index = uniqueDataType.image_uris.png.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1;
                    int index2 = uniqueDataType.image_uris.png.LastIndexOf("?", StringComparison.OrdinalIgnoreCase);
                    int count = index2 - index;

                    string name = uniqueDataType.name_field.ReplaceBadWindowsCharacters();
                    string fullName = name + "-PNG.png";
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

                    ImageProcessed?.Invoke(this, 1); // we processed one image so just send 1 along
                }

                if (artCrop)
                {
                    int index = uniqueDataType.image_uris.art_crop.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1;
                    int index2 = uniqueDataType.image_uris.art_crop.LastIndexOf("?", StringComparison.OrdinalIgnoreCase);
                    int count = index2 - index;

                    string name = uniqueDataType.name_field.ReplaceBadWindowsCharacters();
                    string fullName = name + "-artCrop.jpg";
                    string fullPath = Path.Combine(setPath, name);
                    string absolutePathToFile = Path.Combine(fullPath, fullName);

                    // only download the image if we don't have it
                    if (!File.Exists(absolutePathToFile))
                    {
                        HttpClient client = new HttpClient();

                        HttpResponseMessage response = client.GetAsync(uniqueDataType.image_uris.art_crop, HttpCompletionOption.ResponseHeadersRead).Result;
                        response.EnsureSuccessStatusCode();

                        byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result;

                        // make sure our directory exists
                        if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);

                        File.WriteAllBytes(absolutePathToFile, imageBytes);
                    }

                    ImageProcessed?.Invoke(this, 1); // we processed one image so just send 1 along
                }

                if (borderCrop)
                {
                    int index = uniqueDataType.image_uris.border_crop.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1;
                    int index2 = uniqueDataType.image_uris.border_crop.LastIndexOf("?", StringComparison.OrdinalIgnoreCase);
                    int count = index2 - index;

                    string name = uniqueDataType.name_field.ReplaceBadWindowsCharacters();
                    string fullName = name + "-borderCrop.jpg";
                    string fullPath = Path.Combine(setPath, name);
                    string absolutePathToFile = Path.Combine(fullPath, fullName);

                    // only download the image if we don't have it
                    if (!File.Exists(absolutePathToFile))
                    {
                        HttpClient client = new HttpClient();

                        HttpResponseMessage response = client.GetAsync(uniqueDataType.image_uris.border_crop, HttpCompletionOption.ResponseHeadersRead).Result;
                        response.EnsureSuccessStatusCode();

                        byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result;

                        // make sure our directory exists
                        if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);

                        File.WriteAllBytes(absolutePathToFile, imageBytes);
                    }

                    ImageProcessed?.Invoke(this, 1); // we processed one image so just send 1 along
                }
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred attempting to download the unique artwork file for {uniqueDataType.name_field}.{Environment.NewLine}{ex}");
            }
        }
    }
}
