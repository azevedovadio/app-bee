using System.Diagnostics;
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

    }
}
