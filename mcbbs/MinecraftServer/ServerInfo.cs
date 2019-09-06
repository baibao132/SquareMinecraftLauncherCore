using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinecraftServer.Server.Forge;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Drawing;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using SikaDeerLauncher.MinecraftServer.Json;
using System.Security.Policy;

namespace MinecraftServer.Server
{
    public class ServerInfo
    {
        /// <summary>
        /// 服务器IP地址
        /// </summary>
        public string ServerIP { get; set; }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// 获取服务器MOTD
        /// </summary>
        public string MOTD { get; private set; }

        /// <summary>
        /// 获取服务器的最大玩家数量
        /// </summary>
        public int MaxPlayerCount { get; private set; }

        /// <summary>
        /// 获取服务器的在线人数
        /// </summary>
        public int CurrentPlayerCount { get; private set; }

        /// <summary>
        /// 获取服务器版本号
        /// </summary>
        public int ProtocolVersion { get; private set; }

        /// <summary>
        /// 获取服务器名称
        /// </summary>
        public string name { get; private set; }

        /// <summary>
        /// 获取服务器详细的服务器信息JsonResult
        /// </summary>
        public string JsonResult { get; private set; }

        /// <summary>
        /// 获取服务器Forge信息（如果可用）
        /// </summary>
        public ForgeInfo ForgeInfo { get; private set; }
        /// <summary>
        /// 获取此次连接服务器的延迟(ms)
        /// </summary>
        public long Ping { get; private set; }

        /// <summary>
        /// 获取服务器icon（如果可用）
        /// </summary>
        public Bitmap Icon { get; private set; }

        /// <summary>
        /// 获取与特定格式代码相关联的颜色代码
        /// </summary>
        public static Dictionary<char, string> MinecraftColors { get { return new Dictionary<char, string>() { { '0', "#000000" }, { '1', "#0000AA" }, { '2', "#00AA00" }, { '3', "#00AAAA" }, { '4', "#AA0000" }, { '5', "#AA00AA" }, { '6', "#FFAA00" }, { '7', "#AAAAAA" }, { '8', "#555555" }, { '9', "#5555FF" }, { 'a', "#55FF55" }, { 'b', "#55FFFF" }, { 'c', "#FF5555" }, { 'd', "#FF55FF" }, { 'e', "#FFFF55" }, { 'f', "#FFFFFF" } }; } }

        public ServerInfo(string ip,int port)
        {
            this.ServerIP = ip;
            this.ServerPort = port;
        }

        public void StartGetServerInfo()
        {
            try
            {
                // Some code source form:
                // Minecraft Client v1.9.0 for Minecraft 1.4.6 to 1.9.0 - By ORelio under CDDL-1.0
                // wiki.vg
                using (TcpClient tcp = new TcpClient(this.ServerIP, this.ServerPort))
                {
                    tcp.ReceiveBufferSize = 1024 * 1024;
                    byte[] packet_id = ProtocolHandler.getVarInt(0);
                    byte[] protocol_version = ProtocolHandler.getVarInt(-1);
                    byte[] server_adress_val = Encoding.UTF8.GetBytes(this.ServerIP);
                    byte[] server_adress_len = ProtocolHandler.getVarInt(server_adress_val.Length);
                    byte[] server_port = BitConverter.GetBytes((ushort)this.ServerPort); Array.Reverse(server_port);
                    byte[] next_state = ProtocolHandler.getVarInt(1);
                    byte[] packet2 = ProtocolHandler.concatBytes(packet_id, protocol_version, server_adress_len, server_adress_val, server_port, next_state);
                    byte[] tosend = ProtocolHandler.concatBytes(ProtocolHandler.getVarInt(packet2.Length), packet2);

                    tcp.Client.Send(tosend, SocketFlags.None);

                    byte[] status_request = ProtocolHandler.getVarInt(0);
                    byte[] request_packet = ProtocolHandler.concatBytes(ProtocolHandler.getVarInt(status_request.Length), status_request);

                    tcp.Client.Send(request_packet, SocketFlags.None);

                    ProtocolHandler handler = new ProtocolHandler(tcp);
                    int packetLength = handler.readNextVarIntRAW();
                    if (packetLength > 0)
                    {
                        List<byte> packetData = new List<byte>(handler.readDataRAW(packetLength));
                        if (ProtocolHandler.readNextVarInt(packetData) == 0x00) //Read Packet ID
                        {
                            string result = ProtocolHandler.readNextString(packetData); //Get the Json data
                            this.JsonResult = result;
                            SetInfoFromJsonText(result);
                        }
                    }
                }
                ReloadPing();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetInfoFromJsonText(string JsonText)
        {
            try
            {
                if (!String.IsNullOrEmpty(JsonText) && JsonText.StartsWith("{") && JsonText.EndsWith("}"))
                {
                    var jo2 = JsonConvert.DeserializeObject<jsonForge.Root>(JsonText);
                    MOTD = jo2.description;
                    MaxPlayerCount = jo2.players.max;
                    CurrentPlayerCount = jo2.players.online;
                    ProtocolVersion = jo2.version.protocol;
                    name = jo2.version.name;
                    byte[] arr = Convert.FromBase64String(jo2.favicon.Replace("data:image/png;base64,", ""));
                    using (MemoryStream ms = new MemoryStream(arr))
                    {
                        Icon = new Bitmap(ms);
                        if (jo2.modinfo != null)
                        {
                            ForgeInfo forgeInfo = new ForgeInfo(jo2.modinfo.modList.ToArray());
                            ForgeInfo = forgeInfo;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ReloadPing()
        {
            using (Ping pinger = new Ping())
            {
                var pingResulr = pinger.Send(this.ServerIP);
                if (pingResulr.Status == IPStatus.Success)
                {
                    this.Ping = pingResulr.RoundtripTime;
                }
            }
        }
    }
}
