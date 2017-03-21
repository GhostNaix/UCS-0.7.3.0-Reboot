using System;
using System.Configuration;
using System.Reflection;
using UCS.Helpers;

namespace UCS.Core.Settings
{
    internal class Constants
    {
        public static string Version                 = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string Build                   = "37";
        
        public static readonly bool UseCacheServer   = Utils.ParseConfigBoolean("CacheServer");

        public const int CleanInterval               = 5000;
        public static int MaxOnlinePlayers           = Utils.ParseConfigInt("MaxOnlinePlayers");

        internal const int SendBuffer                = 2048;
        internal const int ReceiveBuffer             = 2048;
        public static int LicensePlanID              = 3;
    }
}
