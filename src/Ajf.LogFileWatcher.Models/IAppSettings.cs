using System.Collections;
using System.Collections.Generic;

namespace LogFileWatcher.Models
{
    public interface IAppSettings
    {
        string RunAsUserName { get; }
        int TickSleep { get; }
        string RunAsPassword { get; }
        string Description { get; }
        string DisplayName { get; }
        string ServiceName { get; }
        IEnumerable<IMonitoringTarget> MonitoringTargets { get; set; }
    }
}