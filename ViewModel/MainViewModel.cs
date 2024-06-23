using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowTracker.Model;

namespace WindowTracker.ViewModel
{
    internal class MainViewModel
    {
        public MainViewModel(IProcessTrackerConfiguration configuration)
        {
            _processTracker = new ProcessTracker(configuration);
            RegularExpressions = string.Join("\t", configuration.ProcessNameRegularExpressionPatterns);
        }

        private ProcessTracker _processTracker;

        public string RegularExpressions { get; }

        public ObservableCollection<TrackedProcess> Processes => _processTracker.Processes;
    }
}
