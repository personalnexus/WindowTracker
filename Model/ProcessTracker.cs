using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WindowTracker.Model
{
    internal class ProcessTracker : IDisposable
    {
        public ProcessTracker(IProcessTrackerConfiguration configuration)
        {
            _csvPath = configuration.CsvPath;
            Processes = new ObservableCollection<TrackedProcess>();
            _processesById = new Dictionary<int, TrackedProcess>();
            _processNameRegularExpressions = configuration.ProcessNameRegularExpressionPatterns
                .Select(x => new Regex(x, RegexOptions.Compiled))
                .ToArray();
            _checkLock = new object();
            _checkSynchronizationContext = configuration.SynchronizationContext;
            _checkTimer = new Timer(Check, null, TimeSpan.FromMilliseconds(1), configuration.CheckInterval);
        }

        public void Dispose() => _checkTimer.Dispose();

        private readonly string _csvPath;
        private readonly Regex[] _processNameRegularExpressions;
        private readonly SynchronizationContext _checkSynchronizationContext;
        private readonly object _checkLock;
        private readonly Timer _checkTimer;
        private readonly Dictionary<int, TrackedProcess> _processesById;

        public ObservableCollection<TrackedProcess> Processes { get; }

        private void Check(object? _)
        {
            if (Monitor.TryEnter(_checkLock))
            {
                try
                {
                    _checkSynchronizationContext.Send(_ =>
                    {
                        // Identify any new processes matching our regex
                        foreach (Process process in Process.GetProcesses())
                        {
                            if (Processes.FirstOrDefault(x => x.Id == process.Id) == null &&
                                _processNameRegularExpressions.Any(x => x.IsMatch(process.ProcessName)))
                            {
                                Processes.Add(new TrackedProcess(process));
                            }
                        }

                        // Group handles by process
                        var handlesByProcessId = new Dictionary<int, List<IntPtr>>();
                        NativeMethods.EnumWindows((hWnd, _) =>
                        {
                            NativeMethods.GetWindowThreadProcessId(hWnd, out int processId);
                            if (!handlesByProcessId.TryGetValue(processId, out List<IntPtr>? handles))
                            {
                                handles = new List<IntPtr>();
                                handlesByProcessId.Add(processId, handles);
                            }
                            handles.Add(hWnd);
                            NativeMethods.EnumChildWindows(hWnd, (childHWnd, _) =>
                            {
                                handles.Add(childHWnd);
                                return true;
                            },
                            IntPtr.Zero);
                            return true;
                        },
                        IntPtr.Zero);

                        // Compare current handles against previous
                        List<string> csv = new List<string>();
                        foreach (TrackedProcess process in Processes)
                        {
                            process.Update(handlesByProcessId.GetValueOrDefault(process.Id));
                            csv.Add(process.ToCsv());
                        }

                        // Export to CSV
                        File.AppendAllLines(_csvPath, csv);
                    },
                    null);
                }
                finally
                {
                    Monitor.Exit(_checkLock);
                }
            }
        }
    }
}
