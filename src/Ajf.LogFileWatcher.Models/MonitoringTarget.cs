using System;

namespace Ajf.LogFileWatcher.Models
{
    public class MonitoringTarget : IMonitoringTarget
    {
        public string Path { get; set; }
        public TimeSpan MaxAge { get; set; }
    }
}