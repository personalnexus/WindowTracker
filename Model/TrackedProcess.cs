using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace WindowTracker.Model
{
    internal class TrackedProcess : INotifyPropertyChanged
    {
        public TrackedProcess(Process process)
        {
            Name = process.ProcessName;
            Id = process.Id;
            IsRunning = true;
            LastSeen = DateTime.Now;
            _process = process;
        }

        public string Name { get; }
        public int Id { get; }

        public bool IsRunning { get; private set; }
        public DateTime LastSeen { get; private set; }

        public int AddedWindowCount { get; private set; }
        public int RemovedWindowCount { get; private set; }
        public int CurrentWindowCount => _windowHandles.Count;

        private HashSet<IntPtr> _windowHandles = new HashSet<IntPtr>();
        private readonly Process _process;

        public void Update(IReadOnlyCollection<IntPtr>? currentHandles)
        {
            if (IsRunning)
            {
                if (_process.HasExited)
                {
                    IsRunning = false;
                }
                else
                {
                    currentHandles = currentHandles ?? NoHandles;
                    LastSeen = DateTime.Now;
                    AddedWindowCount += currentHandles.Except(_windowHandles).Count();
                    RemovedWindowCount += _windowHandles.Except(currentHandles).Count();
                    _windowHandles = new HashSet<IntPtr>(currentHandles);
                }
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        public event PropertyChangedEventHandler? PropertyChanged;

        private ReadOnlyCollection<IntPtr> NoHandles = new ReadOnlyCollection<IntPtr>(Array.Empty<IntPtr>());

        public static string CsvHeader = "Name,Id,CurrentWindowCount,AddedWindowCount,RemovedWindowCount,IsRunning,LastSeen";
        public string ToCsv() => $"{Name},{Id},{CurrentWindowCount},{AddedWindowCount},{RemovedWindowCount},{IsRunning},{LastSeen:yyyy-MM-ddTHH:mm:ss}";
    }
}
