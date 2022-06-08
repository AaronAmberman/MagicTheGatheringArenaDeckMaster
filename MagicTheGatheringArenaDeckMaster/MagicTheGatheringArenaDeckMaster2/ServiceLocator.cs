using MagicTheGatheringArena.Core.Services;
using MagicTheGatheringArenaDeckMaster2.Services;
using MagicTheGatheringArenaDeckMaster2.ViewModels;

namespace MagicTheGatheringArenaDeckMaster2
{
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

        public LoggerService LoggerService { get; set; }
        public ApplicationPathingService PathingService { get; set; }
        public ScryfallService ScryfallService { get; set; }

        #endregion

        #region Constructors

        private ServiceLocator()
        {
            LoggerService = new LoggerService();
            PathingService = new ApplicationPathingService();
            ScryfallService = new ScryfallService(LoggerService, PathingService);
        }

        #endregion
    }
}
