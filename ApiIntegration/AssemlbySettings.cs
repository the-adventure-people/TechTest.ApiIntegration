using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text;

namespace ApiIntegration
{
    internal class AssemblySettings
    {
        public static bool ApplyDiscount => bool.Parse(GetAppSetting("ApplyDiscount"));
        public static decimal Discount => decimal.Parse(GetAppSetting("Discount"));

        private static string GetAppSetting(string key)
        {
            try
            {
                var asmPath = Assembly.GetExecutingAssembly().Location;
                var config = ConfigurationManager.OpenExeConfiguration(asmPath);
                var setting = config.AppSettings.Settings[key];
                return setting.Value;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error reading configuration setting", e);
            }
        }
    }
}
