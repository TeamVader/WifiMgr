using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace WifiManager
{
    class XML_Functions
    {
        
        public static string xml_name = AppDomain.CurrentDomain.BaseDirectory + @"networksettings.xml";
        public static void Create_XML_WifiNetwork_File()
        {

            List<WifiNetwork> WifiNetworklist = new List<WifiNetwork>();
           
            WifiNetworklist.Add(new WifiNetwork("aerne", "test", "DHCP", "0.0.0.0"));
            WifiNetworklist.Add(new WifiNetwork("aerne", "test", "DHCP", "0.0.0.0"));
            WifiNetworklist.Add(new WifiNetwork("aerne", "test", "DHCP", "0.0.0.0"));



            try
            {

                if (!File.Exists(xml_name))
                {

                    XmlWriterSettings settings = new XmlWriterSettings();

                    // settings.Encoding = Encoding.GetEncoding("UTF-8");
                    settings.Indent = true;
                    settings.IndentChars = "\t";
                    // settings.Indent = true;
                    // settings.NewLineHandling = NewLineHandling.Replace;
                    // settings.IndentChars = " ";
                    // settings.NewLineOnAttributes = true;
                    //  settings.OmitXmlDeclaration = true;



                    using (XmlWriter writer = XmlWriter.Create(xml_name, settings))//
                    {
                        writer.WriteStartDocument();

                        writer.WriteStartElement("WifiNetwork_List");

                        foreach (WifiNetwork WifiNetwork in WifiNetworklist)
                        {
                            writer.WriteStartElement("WifiNetwork");

                            writer.WriteElementString("SSID", WifiNetwork.SSID);
                            writer.WriteElementString("Key", WifiNetwork.Key);
                            writer.WriteElementString("DHCPorSTATIC", WifiNetwork.DHCPorSTATIC);
                            writer.WriteElementString("StaticIP", WifiNetwork.StaticIP);

                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                        writer.WriteEndDocument();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }


        /// <summary>
        /// Read the WifiNetwork file 
        /// </summary>
        /// <param name="WifiNetwork_list"></param>
        public static void Read_XML_WifiNetwork_File(List<WifiNetwork> WifiNetwork_list)
        {

            try
            {
               // Console.WriteLine(xml_name);
                if (File.Exists(xml_name))
                {

                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(xml_name);

                    foreach (XmlNode WifiNetwork in xdoc.SelectNodes("/WifiNetwork_List/*"))
                    {
                        if (WifiNetwork != null)
                        {
                            WifiNetwork_list.Add(new WifiNetwork(WifiNetwork["SSID"].InnerText, WifiNetwork["Key"].InnerText, WifiNetwork["DHCPorSTATIC"].InnerText, WifiNetwork["StaticIP"].InnerText));
                           // Console.WriteLine(WifiNetwork["SSID"].InnerText + WifiNetwork["Key"].InnerText + WifiNetwork["DHCPorSTATIC"].InnerText + WifiNetwork["StaticIP"].InnerText);
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static void Change_XML_WifiNetwork_File(WifiNetwork WifiNetwork)
        {

            try
            {
                // Console.WriteLine(xml_name);
                if (File.Exists(xml_name))
                {

                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(xml_name);

                    XmlElement node = xdoc.CreateElement("WifiNetwork");
                    XmlElement SSID = xdoc.CreateElement("SSID");
                    SSID.InnerText = WifiNetwork.SSID;
                    XmlElement Key = xdoc.CreateElement("Key");
                    Key.InnerText = WifiNetwork.Key;
                    XmlElement DHCPorSTATIC = xdoc.CreateElement("DHCPorSTATIC");
                    DHCPorSTATIC.InnerText = WifiNetwork.DHCPorSTATIC;
                    XmlElement StaticIP = xdoc.CreateElement("StaticIP");
                    StaticIP.InnerText = WifiNetwork.StaticIP;
                    node.AppendChild(SSID);
                    node.AppendChild(Key);
                    node.AppendChild(DHCPorSTATIC);
                    node.AppendChild(StaticIP);
                    xdoc.DocumentElement.AppendChild(node);

                    xdoc.Save(xml_name);


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }


        public class SelectNetwork
        {
            string _netmsg;
            bool _IsInList;
            int _listindex;
            string _SSID;

            public SelectNetwork(string Netmsg, bool IsinList, int ListIndex,string ssid)
            {
                this._netmsg = Netmsg;
                this._IsInList = IsinList;
                this._listindex = ListIndex;
                this._SSID = ssid;
            }
            public string netmsg { get { return _netmsg; } }
            public bool IsInList { get { return _IsInList; } }
            public int listindex { get { return _listindex; } }
            public string SSID { get { return _SSID; } }
        }

        public class WifiNetwork
        {
            string _ssid;
            string _key;
            string _DCHPorStatic;
            string _staticIP;


            public WifiNetwork(string ssid, string key, string DCHPorStatic, string staticIP)
            {
                this._ssid = ssid;
                this._key = key;
                this._DCHPorStatic = DCHPorStatic;
                this._staticIP = staticIP;

            }

            public string SSID { get { return _ssid; } }
            public string Key { get { return _key; } }
            public string DHCPorSTATIC { get { return _DCHPorStatic; } }
            public string StaticIP { get { return _staticIP; } }

        }

    }
}
