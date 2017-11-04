using System;
using System.Collections.Generic;
using System.Configuration;
using LogFileWatcher.Models;

namespace LogFileWatcher.Service
{
    public class AppSettingsProvider
    {
        public IAppSettings GetAppSettings()
        {
            return new AppSettings
            {
                DisplayName = ConfigurationManager.AppSettings["DisplayName"],
                ServiceName = ConfigurationManager.AppSettings["ServiceName"],
                TickSleep = Convert.ToInt32(ConfigurationManager.AppSettings["TickSleep"]),
                MonitoringTargets = GetMonitoringTargets(ConfigurationManager.AppSettings["MonitoringTargets"])
            };
        }

        public IEnumerable<IMonitoringTarget> GetMonitoringTargets(string monitoringTargets)
        {
            var targetsAsStrings = monitoringTargets.Split('|');

            var result = new List<IMonitoringTarget>();
            foreach (var targetsAsString in targetsAsStrings)
            {
                var targetProperties = targetsAsString.Split(';');
                result.Add(new MonitoringTarget()
                {
                    Path = targetProperties[0],
                    MaxAge = new TimeSpan(0, Convert.ToInt32(targetProperties[1]), Convert.ToInt32(targetProperties[2]))
                });
            }

            return result;
        }
    }
}