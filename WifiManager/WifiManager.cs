using NativeWifi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

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



        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            bool foundinlist = false;
            bool failed = false;
            int indexlist = 0;
            string keypwd = "";
            string profileName = "";
            string adaptername = "";
            //Console.ResetColor();
            List<XML_Functions.WifiNetwork> network_list = new List<XML_Functions.WifiNetwork>();
            List<XML_Functions.SelectNetwork> select_list = new List<XML_Functions.SelectNetwork>();
            XML_Functions.Create_XML_WifiNetwork_File();
            XML_Functions.Read_XML_WifiNetwork_File(network_list);
            WlanClient client = new WlanClient();

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("***************** Avaible Wireless Networks ***************************");
            Console.ResetColor();
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                adaptername = wlanIface.InterfaceDescription;
                // Lists all networks with WEP security
                Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList(0);
                foreach (Wlan.WlanAvailableNetwork network in networks)
                {
                    // if ( network.dot11DefaultCipherAlgorithm == Wlan.Dot11CipherAlgorithm.)
                    //  {

                    for (int i = 0; i <= network_list.Count - 1; i++)
                    {

                        if (network_list[i].SSID == GetStringForSSID(network.dot11Ssid))
                        {
                            foundinlist = true;
                            indexlist = i;
                        }
                    }
                    if (foundinlist)
                    {
                        foundinlist = false;
                        for (int count = 0; count <= select_list.Count - 1; count++)
                        {
                            if (select_list[count].SSID == GetStringForSSID(network.dot11Ssid))
                            {
                                foundinlist = true;
                            }
                          
                        }
                        if (!foundinlist)
                        {
                            select_list.Add(new XML_Functions.SelectNetwork(string.Format("SSID {0}   Siqnal Quality {1}." + System.Environment.NewLine, GetStringForSSID(network.dot11Ssid), network.wlanSignalQuality), true, indexlist, GetStringForSSID(network.dot11Ssid)));
                        }


                    }
                    else
                    {
                        for (int count = 0; count < select_list.Count - 1; count++)
                        {
                            if (select_list[count].SSID == GetStringForSSID(network.dot11Ssid))
                            {
                                foundinlist = true;
                            }
                          
                        }
                        if (!foundinlist)
                        {
                            select_list.Add(new XML_Functions.SelectNetwork(string.Format("SSID {0}   Siqnal Quality {1}." + System.Environment.NewLine, GetStringForSSID(network.dot11Ssid), network.wlanSignalQuality), false, indexlist, GetStringForSSID(network.dot11Ssid)));
                        }

                    }
                    indexlist = 0;
                    foundinlist = false;
                    // }
                }

                // Retrieves XML configurations of existing profiles.
                // This can assist you in constructing your own XML configuration
                // (that is, it will give you an example to follow).
                foreach (Wlan.WlanProfileInfo profileInfo in wlanIface.GetProfiles())
                {
                    string name = profileInfo.profileName; // this is typically the network's SSID
                    string xml = wlanIface.GetProfileXml(profileInfo.profileName);
                }


            }

            var originalpos = Console.CursorTop;


            var j = 0;

            for (int l = 0; l < select_list.Count - 1; l++)
            {
                if (select_list[l].IsInList == true)
                {
                    Console.Write("[  ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("OK");
                    Console.ResetColor();
                    Console.Write("  ]");
                }
                else
                {
                    Console.Write("[ ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("FAIL");
                    Console.ResetColor();
                    Console.Write(" ]");
                }
                if (l == j)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(select_list[l].netmsg);
                    Console.ResetColor();

                }
                else
                {
                    Console.Write(select_list[l].netmsg);
                }


            }

            Console.WriteLine("Press Key Down or Up for Select the Network and then Enter for Choose");
            Console.WriteLine("Press q for exit");
            Console.SetCursorPosition(0, 1);
            var k = Console.ReadKey();

            while (k.Key != ConsoleKey.Enter)
            {

                if (k.Key == ConsoleKey.UpArrow)
                {


                    if (j < 1)
                    {
                        j = select_list.Count - 1;
                    }
                    j--;
                    Console.SetCursorPosition(0, 1);
                    for (int l = 0; l < select_list.Count - 1; l++)
                    {
                        if (select_list[l].IsInList == true)
                        {
                            Console.Write("[  ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("OK");
                            Console.ResetColor();
                            Console.Write("  ]");
                        }
                        else
                        {
                            Console.Write("[ ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("FAIL");
                            Console.ResetColor();
                            Console.Write(" ]");
                        }
                        if (l == j)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write(select_list[l].netmsg);
                            Console.ResetColor();

                        }
                        else
                        {
                            Console.Write(select_list[l].netmsg);
                        }

                    }


                }

                if (k.Key == ConsoleKey.DownArrow)
                {

                    if (j >= select_list.Count - 2)
                    {
                        j = -1;

                    }
                    j++;
                    Console.SetCursorPosition(0, 1);
                    for (int l = 0; l < select_list.Count - 1; l++)
                    {
                        if (select_list[l].IsInList == true)
                        {
                            Console.Write("[  ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("OK");
                            Console.ResetColor();
                            Console.Write("  ]");
                        }
                        else
                        {
                            Console.Write("[ ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("FAIL");
                            Console.ResetColor();
                            Console.Write(" ]");
                        }
                        if (l == j)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write(select_list[l].netmsg);
                            Console.ResetColor();

                        }
                        else
                        {
                            Console.Write(select_list[l].netmsg);
                        }

                    }

                }
                Console.SetCursorPosition(0, select_list.Count + 2);
                k = Console.ReadKey();
            }

            if (k.Key == ConsoleKey.Enter)
            {

                Console.Clear();
                Console.WriteLine(string.Format("Connect to Network {0}", select_list[j].SSID));

                if (select_list[j].IsInList == false)
                {
                    Console.WriteLine("Put Wifi Passwort :");
                    keypwd = Console.ReadLine();
                   
                }
                else
                {
                     keypwd=network_list[j].Key;
                }
                // Connects to a known network with WEP security
                // this is also the SSID
                profileName =  select_list[j].SSID;
                string ssid = profileName;
                byte[] ssidBytes = System.Text.Encoding.Default.GetBytes(ssid);
                string ssidHex = BitConverter.ToString(ssidBytes);
                ssidHex = ssidHex.Replace("-", "");

                string key = keypwd;
                string profileXml = string.Format("<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><hex>{1}</hex><name>{0}</name></SSID></SSIDConfig><connectionType>ESS</connectionType><connectionMode>manual</connectionMode><MSM><security><authEncryption><authentication>WPA2PSK</authentication><encryption>AES</encryption><useOneX>false</useOneX></authEncryption><sharedKey><keyType>passPhrase</keyType><protected>false</protected><keyMaterial>{2}</keyMaterial></sharedKey></security></MSM></WLANProfile>", ssid, ssidHex, key);
                foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
                {
                    try
                    {
                        wlanIface.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);
                        wlanIface.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, profileName);
                    }
                    catch (Exception ex)
                    {
                        failed = true;
                    }
                }
                Thread.Sleep(1000);
                if (failed == false)
                {
                    if (select_list[j].IsInList == true)
                    {
                        if (network_list[select_list[j].listindex].DHCPorSTATIC == "DHCP")
                        {

                            NetworkAdapter.SetIP("192.168.0.11", "255.255.255.0", adaptername, 'd');
                        }
                        else if (network_list[select_list[j].listindex].DHCPorSTATIC == "static")
                        {

                            NetworkAdapter.SetIP(network_list[select_list[j].listindex].StaticIP, "255.255.255.0", adaptername, 's');
                        }

                    }
                    else
                    {
                        string ip_adress = "";
                        bool valid_ip = false;
                        Regex ip = new Regex(@"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)");
                        MatchCollection result;
                        //Program.ping_ip("192.168.1.1");

                        Console.WriteLine("Enter d for DHCP or s for Static IP: ");
                        ConsoleKeyInfo name = Console.ReadKey();

                        if (name.KeyChar.ToString() == "s")
                        {
                            do
                            {
                                Console.WriteLine("Enter IP Adress :");
                                ip_adress = Console.ReadLine();
                                result = ip.Matches(ip_adress);
                                if (result.Count == 1)
                                {
                                    valid_ip = true;
                                    //Console.WriteLine(ip_adress.LastIndexOf("."));
                                    NetworkAdapter.SetIP(ip_adress, "255.255.255.0", "Realtek PCIe GBE Family Controller", name.KeyChar);
                                }
                                else
                                {
                                    valid_ip = false;
                                    Console.WriteLine("No valid IP Adress !");
                                }
                            } while (valid_ip == false);
                        }

                        else if (name.KeyChar.ToString() == "d")
                        {
                            NetworkAdapter.SetIP(ip_adress, "255.255.255.0", "Realtek PCIe GBE Family Controller", name.KeyChar);
                        }
                    }

                }
                else
                {
                    Console.WriteLine("Cannot connect to Wifi -> Password correct?");
                }
                Thread.Sleep(1000);
                Console.ReadKey();
            }
            
        }
      

    }
}
    

