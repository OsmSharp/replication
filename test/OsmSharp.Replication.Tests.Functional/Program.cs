using System;
using System.Threading.Tasks;
using OsmSharp.Logging;
using Serilog;

namespace OsmSharp.Replication.Test.Functional
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {            
            // enable logging.
            Logger.LogAction = (origin, level, message, parameters) =>
            {
                var formattedMessage = $"{origin} - {message}";
                switch (level)
                {
                    case "critical":
                        Log.Fatal(formattedMessage);
                        break;
                    case "error":
                        Log.Error(formattedMessage);
                        break;
                    case "warning":
                        Log.Warning(formattedMessage);
                        break; 
                    case "verbose":
                        Log.Verbose(formattedMessage);
                        break; 
                    case "information":
                        Log.Information(formattedMessage);
                        break; 
                    default:
                        Log.Debug(formattedMessage);
                        break;
                }
            };

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger();

            var thePast = DateTime.Now.AddHours(-2).AddDays(-5);
            var catchupEnumerator = new CatchupReplicationDiffEnumerator(thePast);

            while (await catchupEnumerator.MoveNext())
            {
                var current = catchupEnumerator.State;

                Log.Information($"State: {current}");
            }
        }
    }
}
