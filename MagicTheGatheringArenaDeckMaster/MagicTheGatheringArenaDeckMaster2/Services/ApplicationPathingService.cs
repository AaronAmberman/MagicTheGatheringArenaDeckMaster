using MagicTheGatheringArena.Core.Services;
using System.IO;

namespace MagicTheGatheringArenaDeckMaster2.Services
{
    internal class ApplicationPathingService : PathingService
    {
        public string CardDataFile => Path.Combine(BaseDataPath, "Card-Data.json");
        public string LogFile => Path.Combine(LogPath, "DeckMaster.log");
    }
}
