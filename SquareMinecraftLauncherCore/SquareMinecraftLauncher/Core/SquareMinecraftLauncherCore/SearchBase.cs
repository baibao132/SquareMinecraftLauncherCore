using SquareMinecraftLauncher.Minecraft;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace SquareMinecraftLauncher.Core
{
    internal class SearchBase
    {
        int ra;
        public List<JavaVersion> vs = new List<JavaVersion>();
        public void addSubDirectory()
        {
            string[] path = { "Program Files\\Java", "Program Files (x86)\\Java" };
            foreach (var t in GetRemovableDeviceID())
            {
                foreach (var i in path)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(t + i);
                    FileInfo[] j;
                    try
                    {
                        j = directoryInfo.GetFiles("javaw.exe",SearchOption.AllDirectories);
                    }
                    catch(Exception ex)
                    {
                        continue;
                    }
                     foreach (var y in j)
                    {
                        bool s = false;
                        foreach (var p in vs) if (p.Path.Equals(y.FullName)) { s = true;break; }
                        if (s) continue;
                        addrelativeDocument(y.FullName);
                    }
                }
            }

        }
        public void addrelativeDocument(string path)
        {
            JavaVersion javaVersion = new JavaVersion();
            javaVersion.Path = path;
            javaVersion.Version = GetProductVersion(path);
            vs.Add(javaVersion);
        }

        private string GetProductVersion(string path)
        {
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(path);
            return info.ProductName;
        }


        public List<string> GetRemovableDeviceID()
        {
            List<string> deviceIDs = new List<string>();
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT  *  From  Win32_LogicalDisk ");
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {

                switch (int.Parse(mo["DriveType"].ToString()))
                {
                    case (int)DriveType.Removable:   //可以移动磁盘     
                        {
                            //MessageBox.Show("可以移动磁盘");
                            deviceIDs.Add(mo["DeviceID"].ToString());
                            break;
                        }
                    case (int)DriveType.Fixed:   //本地磁盘     
                        {
                            //MessageBox.Show("本地磁盘");
                            deviceIDs.Add(mo["DeviceID"].ToString());
                            break;
                        }
                }
            }
            return deviceIDs;
        }
    }
}
