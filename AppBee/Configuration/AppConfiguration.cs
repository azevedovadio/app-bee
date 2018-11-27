using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AppBee.Configuration
{
    public class AppConfiguration
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType?.FullName);

        private class ConfigurationEntry
        {
            public string Key { get; set; }

            public string Section { get; set; }

            public string Value { get; set; }
        }

        static AppConfiguration()
        {
            Instance = new AppConfiguration();
        }

        private IList<ConfigurationEntry> _entries;

        private AppConfiguration(string iniPath = null)
        {
            var executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name.ToUpper();
            var path = new FileInfo(iniPath ?? executingAssemblyName + ".ini").FullName;
            ParseFile(path);
            LogParameters();
        }

        private void ParseFile(string path)
        {
            _entries = new List<ConfigurationEntry>();

            if (!File.Exists(path))
            {
                Logger.Warn($"Configuration file {path} not found. Using default values.");
                return;
            }

            var section = string.Empty;
            foreach (var line in File.ReadAllLines(path))
            {
                var sectionMatch = Regex.Match(line, @"^\[(\w+)\]\s*$");
                if (sectionMatch.Success)
                {
                    section = sectionMatch.Groups[1].Value;
                    continue;
                }

                var entryMatch = Regex.Match(line, @"^([^;]+?)=([\s\S]+?)$");
                if (entryMatch.Success)
                {
                    _entries.Add(new ConfigurationEntry()
                    {
                        Section = section,
                        Key = entryMatch.Groups[1].Value,
                        Value = entryMatch.Groups[2].Value
                    });
                }
            }
        }

        private void LogParameters()
        {
            foreach (var parameter in _entries)
            {
                Logger.Info($"{parameter.Section} - {parameter.Key} = {parameter.Value}");
            }
        }

        public int MonitorInterval
        {
            get
            {
                var  entry =  _entries.FirstOrDefault(_ => _.Key == "MONITOR_INTERVAL")?.Value;
                return int.TryParse(entry, out var result) ? result : 60;
            }
        }

        public string HostName
        {
            get
            {
                return _entries.FirstOrDefault(_ => _.Key == "HOST_NAME")?.Value;
            }
        }

        public int HostPort
        {
            get
            {
                var entry = _entries.FirstOrDefault(_ => _.Key == "HOST_PORT")?.Value;
                return int.TryParse(entry, out var result) ? result : 0;
            }
        }

        public int IdleInterval
        {
            get
            {
                var entry = _entries.FirstOrDefault(_ => _.Key == "IDLE_INTERVAL")?.Value;
                return int.TryParse(entry, out var result) ? result : 60;
            }
        }

        public bool EnablePrintScreen
        {
            get
            {
                var entry = _entries.FirstOrDefault(_ => _.Key == "ENABLE_PRINTSCREEN")?.Value;
                return bool.TryParse(entry, out var result) ? result : true;
            }
        }

        public static AppConfiguration Instance { get; private set; }
    }
}
