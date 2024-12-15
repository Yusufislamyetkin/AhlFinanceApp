using AhlApp.Shared.Logging;
using Serilog;

namespace AhlApp.Infrastructure.Logging.Concrete
{
    public class SerilogLoggerService : ILoggerService
    {
        public void LogInfo(string message)
        {
            Log.Information(message);
        }

        public void LogWarning(string message)
        {
            Log.Warning(message);
        }

        public void LogError(string message, Exception ex)
        {
            Log.Error(ex, message);
        }
    }
}
