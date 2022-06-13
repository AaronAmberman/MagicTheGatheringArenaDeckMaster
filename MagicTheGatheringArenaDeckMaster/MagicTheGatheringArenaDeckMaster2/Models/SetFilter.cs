using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MagicTheGatheringArenaDeckMaster.Models
{
    /// <summary>Represents a runtime set filter.</summary>
    internal class SetFilter
    {
        public bool AllImagesExistInSet
        {
            get
            {
                try
                {
                    if (Exists)
                    {
                        int setCount = ServiceLocator.Instance.MainWindowViewModel.Cards[Name].Count;

                        List<string> imagesOnDisk = Directory.GetFiles(Path.Combine(ServiceLocator.Instance.PathingService.CardImagePath, Name)).ToList();

                        if (setCount == imagesOnDisk.Count) return true;
                        else return false;
                    }
                    else return false;
                }
                catch (Exception ex)
                {
                    ServiceLocator.Instance.LoggerService.Error($"An error occurred attempting to check to see if the set card image directory existed.{Environment.NewLine}{ex}");

                    return false;
                }
            }
        }

        public bool Exists
        {
            get
            {
                try
                {
                    if (Directory.Exists(Path.Combine(ServiceLocator.Instance.PathingService.CardImagePath, Name)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    ServiceLocator.Instance.LoggerService.Error($"An error occurred attempting to check to see if the set card image directory existed.{Environment.NewLine}{ex}");

                    return false;
                }
            }
        }

        public string Name { get; set; }
    }
}
