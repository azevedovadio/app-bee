using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace AppBee.Helpers
{
    public static class ProcessInfoHelper
    {
        static readonly PerformanceCounter cpuCounter;
        
        static ProcessInfoHelper()
        {
            cpuCounter = new PerformanceCounter("Processor", "% User Time", "_Total");
        }

        public static uint GetMaxClockSpeed()
        {
            var searcher = new ManagementObjectSearcher("select MaxClockSpeed from Win32_Processor");

            foreach (var item in searcher.Get())
            {
                var clockSpeed = (uint)item["MaxClockSpeed"];
                return clockSpeed;
            }

            return 0;
        }

        public static float GetCurrentCpuUsage()
        {
            return cpuCounter.NextValue();
        }


        public static IEnumerable<string> GetListOfProcesses()
        {
            return Process.GetProcesses().Where(_ => !string.IsNullOrEmpty(_.MainWindowTitle)).Select(_ => _.MainWindowTitle);
        }
    }
}
