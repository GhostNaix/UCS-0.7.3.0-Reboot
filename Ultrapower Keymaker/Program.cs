using System;
using System.Text;
using System.Linq;
using System.IO;
using System.Reflection;

namespace Ultrapower_Keymaker
{
    class Program
    {
        private static string arguments;
        private static string key;
        private static string FilePath;
        private static string Encrypted_Key;
        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private static void Main(string[] args)
        {
            Console.SetWindowSize(120, 52);
            var arr = new[]
            {
                  @" __",
                  @"/o \_____",
                  @"\__/-==`",
                  @"",
                  @"",
                  @"Ultrapower Key maker v0.7.3.2 By Naix"
            };
            Console.WriteLine("\n\n");
            foreach (string line in arr)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine(" ");
            args = args.Select(s => s.ToLowerInvariant()).ToArray();
            if (args.Contains("-h") || args.Contains("--help"))
            {
                Console.WriteLine("\n");
                Console.WriteLine("Ultrapower keymaker v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " Console interface\n");
                Console.WriteLine("Command line options");
                Console.WriteLine("====================");
                Console.WriteLine("");
                Console.WriteLine("Usage: Ultrapower Keymaker [arguments]");
                Console.WriteLine("-h | --help Displays this help menu");
                Console.WriteLine("-g | --generate Generate a License Key (Select from one of the options below from License types. Default: Lite)");
                Console.WriteLine("");
                Console.WriteLine("License Types");
                Console.WriteLine("=================");
                Console.WriteLine("");
                Console.WriteLine("[l/lite] Generates a Lite License the Default Free Licence limits to 350 players not gui");
                Console.WriteLine("[p/pro] Generates a Pro License Limits to 700 players no gui");
                Console.WriteLine("[u/ultra] Generates a Ultra License no limits");
                Environment.Exit(0);
            }
            else if (args.Contains("-g") || args.Contains("--generate"))
            {
                if (args.Contains("l") || args.Contains("lite"))
                {
                    Console.WriteLine("Generating LITE License key");
                    key = "1" + RandomString(31);
                    WriteKeyToFile(key);
                    Console.WriteLine("Key generated : " + key);
                    Console.WriteLine("You can either use the key above to authenticate your license or move Ky01.lic into Ultrapower's directory");
                }
                else if (args.Contains("p") || args.Contains("pro"))
                {
                    Console.WriteLine("Generating PRO License key");
                    key = "2" + RandomString(31);
                    WriteKeyToFile(key);
                    Console.WriteLine("Key generated : " + key);
                    Console.WriteLine("You can either use the key above to authenticate your license or move Ky01.lic into Ultrapower's directory");
                }
                else if (args.Contains("u") || args.Contains("ultra"))
                {
                    Console.WriteLine("Generating ULTRA License key");
                    key = "3" + RandomString(31);
                    WriteKeyToFile(key);
                    Console.WriteLine("Key generated : " + key);
                    Console.WriteLine("You can either use the key above to authenticate your license or move Ky01.lic into Ultrapower's directory");
                }
                else
                {
                    Console.WriteLine("No option Specified \nGenerating LITE License key");
                    key = "1" + RandomString(31);
                    WriteKeyToFile(key);
                    Console.WriteLine("Key generated : " + key);
                    Console.WriteLine("You can either use the key above to authenticate your license or move Ky01.lic into Ultrapower's directory");
                }
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("\n");
                Console.WriteLine("Unrecongnised Option Please select a valid argument listed below.");
                Console.WriteLine("\n");
                Console.WriteLine("Ultrapower keymaker v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " Console interface\n");
                Console.WriteLine("Command line options");
                Console.WriteLine("====================");
                Console.WriteLine("");
                Console.WriteLine("Usage: Ultrapower Keymaker [arguments]");
                Console.WriteLine("-h | --help Displays this help menu");
                Console.WriteLine("-g | --generate Generate a License Key (Select from one of the options below from License types)");
                Console.WriteLine("");
                Console.WriteLine("License Types");
                Console.WriteLine("=================");
                Console.WriteLine("");
                Console.WriteLine("[l/lite] Generates a Lite License the Default Free Licence limits to 350 players not gui");
                Console.WriteLine("[p/pro] Generates a Pro License Limits to 700 players no gui");
                Console.WriteLine("[u/ultra] Generates a Ultra License no limits");
                Environment.Exit(0);
            }
        }

        private static void WriteKeyToFile(string KeyToWrite)
        {
            FilePath = "Ky01.lic";
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            Encrypted_Key = ToHexString(KeyToWrite);
            using (StreamWriter _SW = new StreamWriter(FilePath))
            {
                _SW.Write(Encrypted_Key);
            }
        }

        private static string ToHexString(string str)
        {
            var sb = new StringBuilder();
            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
