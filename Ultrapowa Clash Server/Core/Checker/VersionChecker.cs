using System;
using System.Diagnostics;
using System.IO.Compression;
using Ionic.Zip;
using Ionic.Zlib;
using System.IO;
using UCS.Core.Threading;
using static UCS.Core.Logger;
using System.Net;
using System.Threading;
using System.Reflection;
using UCS.Core.Settings;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace UCS.Core.Web
{
    internal class VersionChecker
    {
        public static void DownloadUpdater()
        {
            WebClient client = new WebClient();
            client.DownloadFile("https://ucs-up.000webhostapp.com/UCS_Updater.dat", @"Tools/Updater.exe");
            Thread.Sleep(1000);
            Process.Start(@"Tools/Updater.exe");
            Environment.Exit(0);
        }

        public static string GetVersionString()
        {
            try
            {
                string Version = "0.7.3.2";
                return (Version);
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        public static string LatestCoCVersion()
        {
            try
            {
                JObject obj = JObject.Parse(new WebClient().DownloadString("http://carreto.pt/tools/android-store-version/?package=com.supercell.clashofclans"));
                string Version = (string)obj["version"];

                return Version;
            }
            catch (Exception)
            {
                return "Couldn't get last CoC Version.";
            }
        }
    }
}
