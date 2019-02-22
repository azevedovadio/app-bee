using AppBee.ExtensionMethods;
using AppBee.Monitors;
using AppBee.Persistence;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

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

        public static ContextMenu menu;
        public static MenuItem mnuExit;
        public static NotifyIcon notificationIcon;

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

                Thread notifyThread = new Thread(() =>
                {
                    menu = new ContextMenu();
                    mnuExit = new MenuItem("Exit");
                    menu.MenuItems.Add(0, mnuExit);

                    Stream IconResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AppBee.bee.ico");
                    var test = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                    notificationIcon = new NotifyIcon()
                    {
                        Icon = new Icon(IconResourceStream),
                        ContextMenu = menu,
                        Text = "Main"
                    };

                    mnuExit.Click += new EventHandler((object sender, EventArgs e) =>
                    {
                        notificationIcon.Dispose();
                        service.Stop();
                        Environment.Exit(1);
                    });

                    notificationIcon.Visible = true;
                    Application.Run();
                });

                notifyThread.Start();

                new ManualResetEvent(false).WaitOne();
            }
            else
            {
                Run(new AppBee());
            }
        }
    }
}
