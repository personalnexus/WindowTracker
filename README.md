# WindowTracker

Track how many windows a set of processes have allocated over time. The current values are shown in a grid while a history of values is written to a CSV file.

## Commandline parameters
1. Path to a CSV file that is written/appended with every tracked process's data
2. Timespan for the timer used to check the running processes, e.g. 00:00:10 for a check every ten seconds
3. This and all following parameters are regular expressions used to determine the processes to track

## Further Reading
- Mark Russinovich: [Pushing the Limits of Windows: Handles](https://techcommunity.microsoft.com/t5/windows-blog-archive/pushing-the-limits-of-windows-handles/ba-p/723848)
- Raymond Chen: [Why is the limit of window handles per process 10,000?](https://devblogs.microsoft.com/oldnewthing/20070718-00/?p=25963)
- Raymond Chen: [On the unanswerability of the maximum number of user interface objects a program can create](https://devblogs.microsoft.com/oldnewthing/20060901-11/?p=29883)
- Raymond Chen: [Windows are not cheap objects](https://devblogs.microsoft.com/oldnewthing/20050315-00/?p=36183)