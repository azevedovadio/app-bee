using AppBee.Helpers;
using AppBee.Persistence;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using AppBee.Configuration;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AppBee.Monitors
{
    public class AppBeeMonitor : IDisposable
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType?.FullName);

        private Thread _workerThread;

        private IList<IAppBeeConsumer> services;

        public AppBeeMonitor(IList<IAppBeeConsumer> services)
        {
            this.services = services;

            _workerThread = new Thread(DoWork);
            _workerThread.Start();

        }

        private void DoWork()
        {
            var sendOnline = new List<Func<string>>()
            {
                () => "IDLE",
                () => { return NetworkInfoHelper.GetLocalIPAddress(); },
                () => { return Environment.MachineName; },
                () => { return MemoryInfoHelper.GetTotalPhysicMemory().ToString(); },
                () => { return ProcessInfoHelper.GetMaxClockSpeed().ToString(); },
                () => { return TimeSpan.FromMilliseconds(UserInfoHelper.GetIdleTime()).ToString(); }
            };

            var sendWork = new List<Func<string>>()
            {
                () => "WORK",
                () => { return NetworkInfoHelper.GetLocalIPAddress(); },
                () => { return Environment.MachineName; },
                () => { return UserInfoHelper.GetCurrentUserName(); },
                () => { return MemoryInfoHelper.GetUsagePercentage().ToString() + "%"; },
                () => { return ProcessInfoHelper.GetCurrentCpuUsage().ToString() + "%"; },
            };

            var sendPrintScreen = new List<Func<string>>()
            {
                () => "PRINTSCREEN",
                () =>
                {
                    ScreenCapture sc = new ScreenCapture();
                    Image img = sc.CaptureScreen();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        img.Save(ms, ImageFormat.Png);
                        byte[] imageBytes = ms.ToArray();
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            };

            while (true)
            {
                try
                {
                    var idle = UserInfoHelper.GetIdleTime();

                    Logger.Info("IDLE: " + TimeSpan.FromMilliseconds(idle));

                    foreach (var service in services)
                    {
                        if (idle >= AppConfiguration.Instance.IdleInterval * 1000)
                        {
                            service.SendMessage(GetMessage(sendOnline));
                        }
                        else
                        {
                            service.SendMessage(GetMessage(sendWork));

                            if (AppConfiguration.Instance.EnablePrintScreen)
                            {
                                service.SendMessage(GetMessage(sendPrintScreen));
                            }
                        }
                        Console.WriteLine();
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("Error monitoring services... ");
                    Logger.Error(e.Message);
                    Logger.Error(e.StackTrace);
                }

                Thread.Sleep(AppConfiguration.Instance.MonitorInterval * 1000);
            }

        }

        private string GetMessage(List<Func<string>> funcs)
        {
            return string.Join(",", funcs.Select(_ => _()).ToArray());
        }

        public void Dispose()
        {
            _workerThread.Abort();
        }


    }
}
