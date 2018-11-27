using AppBee.ExtensionMethods;
using AppBee.Monitors;
using AppBee.Persistence;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace AppBee
{
    public class AppBee : ServiceBase
    {
        private static readonly ILog Logger;
        private AppBeeMonitor monitor;
        static AppBee()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            GlobalContext.Properties["assembly"] = assembly.GetName().Name;
            GlobalContext.Properties["title"] = assembly.GetTitle();
            GlobalContext.Properties["version"] = assembly.GetFileVersion();
            GlobalContext.Properties["buildtime"] = assembly.GetBuildTimestamp();

            var assemblyFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Directory.SetCurrentDirectory(assemblyFolder);

            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.xml"));
            Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType?.FullName);
        }

        protected override void OnStart(string[] args)
        {
            Logger.Info("Initializing AppBee");

            var consumers = new List<IAppBeeConsumer>() { new AppBeeSocketConsumer() };
            var waitHandle = new ManualResetEvent(false);
            monitor = new AppBeeMonitor(consumers);

            Logger.Info("AppBee initialized");

        }

        protected override void OnStop()
        {
            if (monitor != null)
            {
                monitor.Dispose();
                monitor = null;
            }

            Logger.Info("AppBee stopped");
        }

        private static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                var service = new AppBee();
                service.OnStart(null);

                new ManualResetEvent(false).WaitOne();
            }
            else
            {
                Run(new AppBee());
            }
        }
    }
}
