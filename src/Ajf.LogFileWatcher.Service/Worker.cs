using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using LogFileWatcher.Models;
using Serilog;

namespace LogFileWatcher.Service
{
    public class Worker
    {
        private IAppSettings _appSettings;
        private BackgroundWorker _backgroundWorker;
        public bool WorkDone { get; set; }

        public void Start()
        {
            try
            {
                _appSettings = new AppSettings();

                _backgroundWorker = new BackgroundWorker
                {
                    WorkerSupportsCancellation = true
                };
                _backgroundWorker.DoWork += _backgroundWorker_DoWork;
                _backgroundWorker.RunWorkerCompleted += _backgroundWorker_RunWorkerCompleted;
                _backgroundWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "During Start", new object[0]);
                throw;
            }
        }

        private void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            WorkDone = true;
        }

        private void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Log.Logger.Information("Doing work");

            try
            {
                DoWorkInternal(sender);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "During do work", new object[0]);
                throw;
            }
        }

        private void DoWorkInternal(object sender)
        {
            WorkDone = false;

            _appSettings = new AppSettingsProvider()
                .GetAppSettings();

            while (true)
            {
                var backgroundWorker = sender as BackgroundWorker;
                if (backgroundWorker == null || backgroundWorker.CancellationPending)
                {
                    Log.Logger.Information("backgroundworker.CancellationPending: {@backgroundWorker}",
                        backgroundWorker);
                    return;
                }

                Log.Logger.Information("Alive");

                foreach (var monitoringTarget in _appSettings.MonitoringTargets)
                {
                    Log.Verbose("Checking " + monitoringTarget.Path);

                    if (!File.Exists(monitoringTarget.Path))
                    {
                        Log.Logger.Warning($"Monitoring target not found:{monitoringTarget.Path}");
                        continue;
                    }

                    var lastWriteTime = File.GetLastWriteTime(monitoringTarget.Path);
                    var timePassed = DateTime.Now.Subtract(lastWriteTime);

                    if (monitoringTarget.MaxAge < timePassed)
                    {
                        Log.Logger.Warning(
                            $"Log file not written for {timePassed}. Max age set to {monitoringTarget.MaxAge}. Path:{monitoringTarget.Path}");
                    }
                }

                Thread.Sleep(_appSettings.TickSleep);

            }
        }

        public void Stop()
        {
            if (_backgroundWorker != null)
            {
                _backgroundWorker.CancelAsync();

                while (!WorkDone)
                    Thread.Sleep(500);
            }
        }
    }
}