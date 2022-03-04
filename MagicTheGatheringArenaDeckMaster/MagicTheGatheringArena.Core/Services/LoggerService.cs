using NLog;
using System;

namespace MagicTheGatheringArena.Core.Services
{
    public class LoggerService
    {
        private ILogger logger;

        public LoggerService()
        {
            logger = LogManager.GetCurrentClassLogger();            
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            logger.Error(exception, message);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Warning(string message)
        {
            logger.Warn(message);
        }
    }
}
