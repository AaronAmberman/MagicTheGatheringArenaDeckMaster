using MagicTheGatheringArena.Core.Database;
using MagicTheGatheringArena.Core.Services;
using MagicTheGatheringArenaDeckMaster.Services;
using MagicTheGatheringArenaDeckMaster.ViewModels;

namespace MagicTheGatheringArenaDeckMaster
{
    /// <summary>ServiceLocator to hold runtime references.</summary>
    internal class ServiceLocator
    {
        #region Fields

        private static ServiceLocator instance = new ServiceLocator();
        private static readonly object @lock = new object();

        #endregion

        #region Properties

        public static ServiceLocator Instance
        {
            get
            {
                lock (@lock)
                {
                    return instance;
                }
            }
        }

        public MainWindowViewModel MainWindowViewModel { get; set; }

        public SQLiteDatabase DatabaseService { get; set; }
        public LoggerService LoggerService { get; set; }
        public ApplicationPathingService PathingService { get; set; }
        public ScryfallService ScryfallService { get; set; }

        #endregion

        #region Constructors

        private ServiceLocator()
        {
            DatabaseService = new SQLiteDatabase();
            LoggerService = new LoggerService();
            PathingService = new ApplicationPathingService();
            ScryfallService = new ScryfallService(LoggerService);
        }

        #endregion
    }
}
