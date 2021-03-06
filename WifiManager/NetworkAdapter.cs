﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WifiManager
{
    class NetworkAdapter
    {
        /// <summary>
        /// Set's a new IP Address and it's Submask of the local machine
        /// </summary>
        /// <param name="ip_address">The IP Address</param>
        /// <param name="subnet_mask">The Submask IP Address</param>
        /// <remarks>Requires a reference to the System.Management namespace</remarks>
        public static void SetIP(string ipAddress, string subnetMask, string nicname, string type)
        {
            using (var networkConfigMng = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                using (var networkConfigs = networkConfigMng.GetInstances())
                {
                    foreach (var managementObject in networkConfigs.Cast<ManagementObject>().Where(managementObject => (bool)managementObject["IPEnabled"]))
                    {

                        
                        string name = managementObject["Caption"].ToString();
                        if (name.Contains(nicname))
                        {
                            Console.WriteLine(managementObject["Caption"].ToString());

                            if (type=="s")
                            {
                                Console.WriteLine("Static IP Enabled");
                                using (var newIP = managementObject.GetMethodParameters("EnableStatic"))
                                {
                                    // Set new IP address and subnet if needed
                                    if ((!String.IsNullOrEmpty(ipAddress)) || (!String.IsNullOrEmpty(subnetMask)))
                                    {
                                        if (!String.IsNullOrEmpty(ipAddress))
                                        {
                                            newIP["IPAddress"] = new[] { ipAddress };
                                        }

                                        if (!String.IsNullOrEmpty(subnetMask))
                                        {
                                            newIP["SubnetMask"] = new[] { subnetMask };
                                        }

                                        managementObject.InvokeMethod("EnableStatic", new object[] {new string[] { ipAddress}, new string[] {  subnetMask} });
                                        Console.WriteLine("Set New IP");
                                    }


                                    // Set mew gateway if needed
                                    /**
                                    if (!String.IsNullOrEmpty(gateway))
                                    {
                                        using (var newGateway = managementObject.GetMethodParameters("SetGateways"))
                                        {
                                            newGateway["DefaultIPGateway"] = new[] { newGateway };
                                            newGateway["GatewayCostMetric"] = new[] { 1 };
                                            managementObject.InvokeMethod("SetGateways", newGateway, null);
                                        }
                                    }*/
                                }

                            }
                            //  }
                            if (type=="d")
                            {
                                var ndns = managementObject.GetMethodParameters("SetDNSServerSearchOrder");
                                ndns["DNSServerSearchOrder"] = null;
                                var enableDhcp = managementObject.InvokeMethod("EnableDHCP", null, null);
                                var setDns = managementObject.InvokeMethod("SetDNSServerSearchOrder", ndns, null);
                            }
                            Console.WriteLine("This App is powered by Stark Industries");

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set's a new Gateway address of the local machine
        /// </summary>
        /// <param name="gateway">The Gateway IP Address</param>
        /// <remarks>Requires a reference to the System.Management namespace</remarks>
        public void setGateway(string gateway)
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    try
                    {
                        ManagementBaseObject setGateway;
                        ManagementBaseObject newGateway =
                            objMO.GetMethodParameters("SetGateways");

                        newGateway["DefaultIPGateway"] = new string[] { gateway };
                        newGateway["GatewayCostMetric"] = new int[] { 1 };

                        setGateway = objMO.InvokeMethod("SetGateways", newGateway, null);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
        /// <summary>
        /// Set's the DNS Server of the local machine
        /// </summary>
        /// <param name="NIC">NIC address</param>
        /// <param name="DNS">DNS server address</param>
        /// <remarks>Requires a reference to the System.Management namespace</remarks>
        public void setDNS(string NIC, string DNS)
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    // if you are using the System.Net.NetworkInformation.NetworkInterface you'll need to change this line to if (objMO["Caption"].ToString().Contains(NIC)) and pass in the Description property instead of the name 
                    if (objMO["Caption"].Equals(NIC))
                    {
                        try
                        {
                            ManagementBaseObject newDNS =
                                objMO.GetMethodParameters("SetDNSServerSearchOrder");
                            newDNS["DNSServerSearchOrder"] = DNS.Split(',');
                            ManagementBaseObject setDNS =
                                objMO.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Set's WINS of the local machine
        /// </summary>
        /// <param name="NIC">NIC Address</param>
        /// <param name="priWINS">Primary WINS server address</param>
        /// <param name="secWINS">Secondary WINS server address</param>
        /// <remarks>Requires a reference to the System.Management namespace</remarks>
        public void setWINS(string NIC, string priWINS, string secWINS)
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    if (objMO["Caption"].Equals(NIC))
                    {
                        try
                        {
                            ManagementBaseObject setWINS;
                            ManagementBaseObject wins =
                            objMO.GetMethodParameters("SetWINSServer");
                            wins.SetPropertyValue("WINSPrimaryServer", priWINS);
                            wins.SetPropertyValue("WINSSecondaryServer", secWINS);

                            setWINS = objMO.InvokeMethod("SetWINSServer", wins, null);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }

    }
}
