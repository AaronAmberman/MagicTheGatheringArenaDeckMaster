using System;
using System.IO;

namespace MagicTheGatheringArena.Core.Services
{
    public class PathingService
    {
        public string BaseDataPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MTGA-DeckMaster");

        public string CardImagePath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MTGA-DeckMaster", "CardImages");
    }
}
