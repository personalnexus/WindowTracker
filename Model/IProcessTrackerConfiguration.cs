namespace WindowTracker.Model
{
    internal interface IProcessTrackerConfiguration
    {
        string CsvPath { get; }
        TimeSpan CheckInterval { get; }
        string[] ProcessNameRegularExpressionPatterns { get; }
        SynchronizationContext SynchronizationContext { get; }
    }
}