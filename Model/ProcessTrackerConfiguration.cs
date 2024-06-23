using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowTracker.Model
{
    internal class ProcessTrackerConfiguration : IProcessTrackerConfiguration
    {
        public ProcessTrackerConfiguration(string csvPath,
                                           TimeSpan checkInterval,
                                           string[] processNameRegularExpressionPatterns,
                                           SynchronizationContext synchronizationContext)
        {
            CsvPath = csvPath;
            CheckInterval = checkInterval;
            ProcessNameRegularExpressionPatterns = processNameRegularExpressionPatterns;
            SynchronizationContext = synchronizationContext;
        }

        public string CsvPath { get; }
        public TimeSpan CheckInterval { get; }
        public string[] ProcessNameRegularExpressionPatterns { get; }
        public SynchronizationContext SynchronizationContext { get; }

        public static ProcessTrackerConfiguration LoadFromCommandline(SynchronizationContext synchronizationContext)
        {
            string[] arguments = Environment.GetCommandLineArgs();
            if (arguments.Length < 4)
            {
                throw new ArgumentException("Application must be started with at least three command line arguments: csvPath, checkIntervalTimespan, processNameRegularExpressionPatterns...");
            }
            var result = new ProcessTrackerConfiguration(csvPath: arguments[1],
                                                         checkInterval: TimeSpan.Parse(arguments[2]),
                                                         processNameRegularExpressionPatterns: arguments.Skip(3).ToArray(),
                                                         synchronizationContext);
            return result;
        }
    }
}
