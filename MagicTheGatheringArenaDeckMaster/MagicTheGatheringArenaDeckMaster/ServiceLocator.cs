using MagicTheGatheringArena.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicTheGatheringArenaDeckMaster
{
    public class ServiceLocator
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

        public LoggerService LoggerService { get; set; }
        public PathingService PathingService { get; set; }
        public ScryfallService ScryfallService { get; set; }

        #endregion

        #region Constructors

        private ServiceLocator()
        {
            LoggerService = new LoggerService();
            PathingService = new PathingService();
            ScryfallService = new ScryfallService(LoggerService);
        }

        #endregion
    }
}
