using NativeWifi;
using System;
using System.Collections.Generic;
using System.Text;

namespace WifiManager
{
    class Program
    {
        /// <summary>
        /// Converts a 802.11 SSID to a string.
        /// </summary>
        static string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.ASCII.GetString( ssid.SSID, 0, (int) ssid.SSIDLength );
        }

        

        static void Main( string[] args )
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            bool foundinlist = false;
            //Console.ResetColor();
            List<XML_Functions.WifiNetwork> network_list = new List<XML_Functions.WifiNetwork>();
            XML_Functions.Create_XML_WifiNetwork_File();
            XML_Functions.Read_XML_WifiNetwork_File(network_list);
            WlanClient client = new WlanClient();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("***************** Avaible Wireless Networks ***************************");
            Console.ResetColor();
            foreach ( WlanClient.WlanInterface wlanIface in client.Interfaces )
            {
                // Lists all networks with WEP security
                Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList( 0 );
                foreach ( Wlan.WlanAvailableNetwork network in networks )
                {
                   // if ( network.dot11DefaultCipherAlgorithm == Wlan.Dot11CipherAlgorithm.)
                  //  {
                    foreach (XML_Functions.WifiNetwork wifinetwork in network_list)
                    {
                        if (wifinetwork.SSID == GetStringForSSID(network.dot11Ssid))
                        {
                            foundinlist = true;
                        }
                    }
                    if (foundinlist)
                    {
                        Console.Write("[  ");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("OK");
                        Console.ResetColor();
                        Console.Write("  ]");
                        Console.Write("SSID {0}   Siqnal Quality {1}." + System.Environment.NewLine, GetStringForSSID(network.dot11Ssid), network.wlanSignalQuality);

                    }
                    else
                    {
                        Console.Write("[ ");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("FAIL");
                        Console.ResetColor();
                        Console.Write(" ]");
                        Console.Write("SSID {0}   Siqnal Quality {1}."+ System.Environment.NewLine, GetStringForSSID(network.dot11Ssid), network.wlanSignalQuality);
                    }
                    foundinlist = false;
                   // }
                }

                // Retrieves XML configurations of existing profiles.
                // This can assist you in constructing your own XML configuration
                // (that is, it will give you an example to follow).
                foreach ( Wlan.WlanProfileInfo profileInfo in wlanIface.GetProfiles() )
                {
                    string name = profileInfo.profileName; // this is typically the network's SSID
                    string xml = wlanIface.GetProfileXml( profileInfo.profileName );
                }

                // Connects to a known network with WEP security
                string profileName = ""; // this is also the SSID
                string ssid = profileName;
                byte[] ssidBytes = System.Text.Encoding.Default.GetBytes(ssid);
                string ssidHex = BitConverter.ToString(ssidBytes);
                ssidHex = ssidHex.Replace("-", "");
             
                string key = "";
                string profileXml = string.Format("<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><hex>{1}</hex><name>{0}</name></SSID></SSIDConfig><connectionType>ESS</connectionType><connectionMode>manual</connectionMode><MSM><security><authEncryption><authentication>WPA2PSK</authentication><encryption>AES</encryption><useOneX>false</useOneX></authEncryption><sharedKey><keyType>passPhrase</keyType><protected>false</protected><keyMaterial>{2}</keyMaterial></sharedKey></security></MSM></WLANProfile>",ssid,ssidHex,key);
                
              //  wlanIface.SetProfile( Wlan.WlanProfileFlags.AllUser, profileXml, true );
              //  wlanIface.Connect( Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, profileName );
            }
            Console.Read();

            Console.Clear();
            Console.WriteLine("Option 1");
            Console.WriteLine("Option 2");
            Console.WriteLine("Option 3");
            Console.WriteLine();
            Console.Write("input: ");
            var originalpos = Console.CursorTop;

            var k = Console.ReadKey();
            var i = 2;

            while (k.KeyChar != 'q')
            {

                if (k.Key == ConsoleKey.UpArrow)
                {

                    Console.SetCursorPosition(0, Console.CursorTop - i);
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine("Option " + (Console.CursorTop + 1));
                    Console.ResetColor();
                    i++;

                }

                Console.SetCursorPosition(8, originalpos);
                k = Console.ReadKey();
            }

        }
    }
}
