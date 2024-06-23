# WindowTracker

Track how many windows a set of processes have allocated over time. The current values are shown in a grid while a history of values is written to a CSV file.

## Commandline parameters
1. Path to a CSV file that is written/appended with every tracked process's data
2. Timespan for the timer used to check the running processes, e.g. 00:00:10 for a check every ten seconds
3. This and all following parameters are regular expressions used to determine the processes to track