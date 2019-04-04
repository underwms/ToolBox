using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace SeqAndSerilogPoC
{
    public static class AppHelper
    {
        // Fields ---------------------------------------------------------------------------------
        private const string _argumentLogging = "-l";
        private const string _configKeyEnvironment = "Environment";
        private const string _configKeyLogKey = "LogKey";
        private const string _configKeyLogUrl = "LogUrl";

        // Properties -----------------------------------------------------------------------------
        public static Dictionary<Arguments, string> ArgumentFlags = new Dictionary<Arguments, string>()
        {
            [Arguments.Logging] = _argumentLogging
        };
        public static Dictionary<ConfigKeys, string> ConfigurationValues = new Dictionary<ConfigKeys, string>()
        {
            [ConfigKeys.Environment] = ConfigurationManager.AppSettings[_configKeyEnvironment] ?? string.Empty,
            [ConfigKeys.LogKey] = ConfigurationManager.AppSettings[_configKeyLogKey] ?? string.Empty,
            [ConfigKeys.LogUrl] = ConfigurationManager.AppSettings[_configKeyLogUrl] ?? string.Empty,
        };
        public static string ApplicationUser => Environment.UserName;
        public static string AppName => Assembly.GetEntryAssembly().GetName().Name;
        public static string AppVersion => Assembly.GetEntryAssembly().GetName().Version.ToString();

        // Public Functions -----------------------------------------------------------------------
        public static string RandomString()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var randomString = new string(chars.Select(c => chars[random.Next(chars.Length)]).Take(10).ToArray());

            return randomString;
        }
    }

    public enum Arguments
    {
        Logging
    }

    public enum ConfigKeys
    {
        Environment,
        LogKey,
        LogUrl
    }
}
