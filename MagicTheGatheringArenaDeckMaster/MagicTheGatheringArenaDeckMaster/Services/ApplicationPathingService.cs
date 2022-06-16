using MagicTheGatheringArena.Core.Services;
using System.IO;

namespace MagicTheGatheringArenaDeckMaster.Services
{
    /// <summary>Overloaded pathing service that tells the application where our assets live.</summary>
    internal class ApplicationPathingService : PathingService
    {
        public string CardDataFile => Path.Combine(BaseDataPath, "Card-Data.json");
        public string DatabaseFile => Path.Combine(DatabasePath, "Database.db");
        public string LogFile => Path.Combine(LogPath, "DeckMaster.log");
    }
}
