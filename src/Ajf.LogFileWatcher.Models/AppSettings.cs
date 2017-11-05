using System.Collections.Generic;

namespace Ajf.LogFileWatcher.Models
{
    public class AppSettings:IAppSettings
    {
        public string RunAsUserName { get; private set; }
        public int TickSleep { get; set; }
        public string RunAsPassword { get; private set; }
        public string Description { get; private set; }
        public string DisplayName { get; set; }
        public string ServiceName { get; set; }
        public IEnumerable<IMonitoringTarget> MonitoringTargets { get; set; }

    }
}