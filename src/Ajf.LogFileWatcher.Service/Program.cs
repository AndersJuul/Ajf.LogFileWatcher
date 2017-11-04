using System;
using LogFileWatcher.Models;
using Serilog;
using Topshelf;

namespace LogFileWatcher.Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();

            Log.Logger.Verbose("Logger configured");

            var appSettings = new AppSettingsProvider()
                .GetAppSettings();

            try
            {
                HostFactory.Run(x => //1
                {
                    try
                    {
                        x.Service<Worker>(s => //2
                        {
                            try
                            {
                                s.ConstructUsing(name => new Worker()); //3
                                s.WhenStarted(tc =>
                                {
                                    Log.Logger.Information("Starting service.");
                                    tc.Start();
                                    Log.Logger.Information("Service started.");
                                }); //4
                                s.WhenStopped(tc =>
                                {
                                    Log.Logger.Information("Stopping service.");
                                    tc.Stop();
                                    Log.Logger.Information("Service stopped.");
                                }); //5
                                s.WhenPaused(tc =>
                                {
                                    Log.Logger.Information("Pausing service.");
                                    tc.Stop();
                                    Log.Logger.Information("Service paused.");
                                }); //5

                                s.WhenContinued(tc =>
                                {
                                    Log.Logger.Information("Continuing service.");
                                    tc.Start();
                                    Log.Logger.Information("Service continued.");
                                }); //5
                                s.WhenSessionChanged((w, sca) =>
                                {
                                    Log.Logger.Information("Session changed: " + w);
                                    Log.Logger.Information("Session changed: " + sca);
                                });



                            }
                            catch (Exception ex)
                            {
                                Log.Logger.Error(ex, "Fra program");
                                throw;
                            }
                        });
                        x.RunAsLocalService(); //6

                        x.SetDescription(appSettings.Description); //7
                        x.SetDisplayName(appSettings.DisplayName); //8
                        x.SetServiceName(appSettings.ServiceName); //9
                        x.OnException(e =>
                        {
                            Log.Logger.Error(e,"Exception");
                        });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }); //10        }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Yderste");
                throw;
            }
        }
    }
}