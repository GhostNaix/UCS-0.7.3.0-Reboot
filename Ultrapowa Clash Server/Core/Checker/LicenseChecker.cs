using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;
using static UCS.Core.Logger;
using System.Net.Sockets;
using UCS.Core.Settings;

namespace UCS.Core.Checker
{
    internal class LicenseChecker
    {
        private string Key;
        private int responseData;
        public LicenseChecker()
        {
            try
            {
                Program._Stopwatch.Stop();

                back:
                Key = GetKey();
                if (Key.Length == 32)
                {
                    CheckIfKeyIsSaved(Key);
                    if (Key.StartsWith("1"))
                    {
                        responseData = 1;
                    }
                    else if (Key.StartsWith("2"))
                    {
                        responseData = 2;
                    }
                    else if (Key.StartsWith("3"))
                    {
                        responseData = 3;
                    }
                    else
                    {
                        responseData = 69;
                    }
                    if (responseData > 0 && responseData < 4)
                    {
                        Constants.LicensePlanID = responseData;
                        Program.UpdateTitle();

                        switch (responseData)
                        {

                            case 1:
                                {
                                    Say("UCS is running on Plan (Lite).");
                                    break;
                                }

                            case 2:
                                {
                                    Say("UCS is running on Plan (Pro).");
                                    break;
                                }

                            case 3:
                                {
                                    Say("UCS is running on Plan (Ultra).");
                                    break;
                                }
                        }
                    }
                    else
                    {
                        Say();
                        Say("This Key is not valid.");
                        Say("UCS will be closed now...");
                        Thread.Sleep(4000);
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Say("You entered a wrong Key! Please try again.");
                    goto back;
                }

                Program._Stopwatch.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Say("UCS will be closed now...");
                Thread.Sleep(4000);
                Environment.Exit(0);
            }
        }

        private static void CheckIfKeyIsSaved(string _Key)
        {
            string _FilePath = "Ky01.lic";
            if (!File.Exists(_FilePath))
            {
                if (_Key.Length == 32)
                {
                    using (StreamWriter _SW = new StreamWriter(_FilePath))
                    {
                        _SW.Write(ToHexString(_Key));
                    }
                }
            }
        }

        private static string GetKey()
        {
            back:
            string _FilePath = "Ky01.lic";
            if (File.Exists(_FilePath))
            {
                string Data = FromHexString(File.ReadAllText(_FilePath));
                if (Data.Length == 32)
                {
                    return Data;
                }
                else
                {
                    File.Delete(_FilePath);
                    goto back;
                }
            }
            else
            {
                Say("Enter now your License Key:");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("[Enter Your License key ] >  ");
                Console.ResetColor();
                string Key = Console.ReadLine();
                return Key;
            }
        }

        private static string ToHexString(string str)
        {
            var sb    = new StringBuilder();
            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }

        private static string FromHexString(string String)
        {
            var bytes = new byte[String.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(String.Substring(i * 2, 2), 16);
            }
            return Encoding.Unicode.GetString(bytes);
        }

    }
}
