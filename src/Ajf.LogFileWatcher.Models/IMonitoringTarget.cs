using System;

namespace Ajf.LogFileWatcher.Models
{
    public interface IMonitoringTarget
    {
        TimeSpan MaxAge { get; set; }
        string Path { get; set; }
    }
}