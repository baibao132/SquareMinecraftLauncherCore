using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

#if DEBUG
using System.Diagnostics;
#endif

namespace SquareMinecraftLauncher.Minecraft.MCServerPing
    {
        public class MinecraftServer
        {
            private static readonly Dictionary<char, ConsoleColor> Colours = new Dictionary<char, ConsoleColor>
        {
             { '0', ConsoleColor.Black       },
             { '1', ConsoleColor.DarkBlue    },
             { '2', ConsoleColor.DarkGreen   },
             { '3', ConsoleColor.DarkCyan    },
             { '4', ConsoleColor.DarkRed     },
             { '5', ConsoleColor.DarkMagenta },
             { '6', ConsoleColor.Yellow      },
             { '7', ConsoleColor.Gray        },
             { '8', ConsoleColor.DarkGray    },
             { '9', ConsoleColor.Blue        },
             { 'a', ConsoleColor.Green       },
             { 'b', ConsoleColor.Cyan        },
             { 'c', ConsoleColor.Red         },
             { 'd', ConsoleColor.Magenta     },
             { 'e', ConsoleColor.Yellow      },
             { 'f', ConsoleColor.White       },
             { 'k', Console.ForegroundColor  },
             { 'l', Console.ForegroundColor  },
             { 'm', Console.ForegroundColor  },
             { 'n', Console.ForegroundColor  },
             { 'o', Console.ForegroundColor  },
             { 'r', ConsoleColor.White       }
        };

            private static NetworkStream _stream;
            private static List<byte> _buffer;
            private static int _offset;

            public static void server(string ip,int port)
            {

                var client = new TcpClient();
                var task = client.ConnectAsync(ip, port);
                Console.WriteLine("Connecting to Minecraft server..");

                while (!task.IsCompleted)
                {
#if DEBUG
                    Debug.WriteLine("Connecting..");
#endif
                    Thread.Sleep(250);
                }

                if (!client.Connected)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Unable to connect to the server");
                    Console.ResetColor();
                    Console.ReadKey(true);
                    Environment.Exit(1);
                }

                _buffer = new List<byte>();
                _stream = client.GetStream();
                Console.WriteLine("Sending status request");


                /*
                 * Send a "Handshake" packet
                 * http://wiki.vg/Server_List_Ping#Ping_Process
                 */
                WriteVarInt(47);
                WriteString(ip);
                WriteShort((short)port);
                WriteVarInt(1);
                Flush(0);

                /*
                 * Send a "Status Request" packet
                 * http://wiki.vg/Server_List_Ping#Ping_Process
                 */
                Flush(0);

                /*
                 * If you are using a modded server then use a larger buffer to account, 
                 * see link for explanation and a motd to HTML snippet
                 * https://gist.github.com/csh/2480d14fbbb33b4bbae3#gistcomment-2672658
                 */
                var buffer = new byte[Int16.MaxValue];
                // var buffer = new byte[4096];
                _stream.Read(buffer, 0, buffer.Length);

                try
                {
                    var length = ReadVarInt(buffer);
                    var packet = ReadVarInt(buffer);
                    var jsonLength = ReadVarInt(buffer);
#if DEBUG
                    Console.WriteLine("Received packet 0x{0} with a length of {1}", packet.ToString("X2"), length);
#endif
                    var json = ReadString(buffer, jsonLength);
                    var ping = JsonConvert.DeserializeObject<PingPayload.Root>(json);

                    Console.WriteLine("Software: {0}", ping.version.name);
                    Console.WriteLine("Protocol: {0}", ping.version.protocol);
                    Console.WriteLine("Players Online: {0}/{1}", ping.players.online, ping.players.max);
                    string str = "";
                    for (int i = 0; i < ping.players.sample.Count; i++) {
                        str += "\n" + ping.players.sample[i].name;
                    }
                    Console.WriteLine(str);
                }
                catch (IOException ex)
                {
                    /*
                     * If an IOException is thrown then the server didn't 
                     * send us a VarInt or sent us an invalid one.
                     */
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Unable to read packet length from server,");
                    Console.WriteLine("are you sure it's a Minecraft server?");
#if DEBUG
                    Console.WriteLine("Here are the details:");
                    Console.WriteLine(ex.ToString());
#endif
                    Console.ResetColor();
                }
            }

            #region Read/Write methods
            internal static byte ReadByte(byte[] buffer)
            {
                var b = buffer[_offset];
                _offset += 1;
                return b;
            }

            internal static byte[] Read(byte[] buffer, int length)
            {
                var data = new byte[length];
                Array.Copy(buffer, _offset, data, 0, length);
                _offset += length;
                return data;
            }

            internal static int ReadVarInt(byte[] buffer)
            {
                var value = 0;
                var size = 0;
                int b;
                while (((b = ReadByte(buffer)) & 0x80) == 0x80)
                {
                    value |= (b & 0x7F) << (size++ * 7);
                    if (size > 5)
                    {
                        throw new IOException("This VarInt is an imposter!");
                    }
                }
                return value | ((b & 0x7F) << (size * 7));
            }

            internal static string ReadString(byte[] buffer, int length)
            {
                var data = Read(buffer, length);
                return Encoding.UTF8.GetString(data);
            }

            internal static void WriteVarInt(int value)
            {
                while ((value & 128) != 0)
                {
                    _buffer.Add((byte)(value & 127 | 128));
                    value = (int)((uint)value) >> 7;
                }
                _buffer.Add((byte)value);
            }

            internal static void WriteShort(short value)
            {
                _buffer.AddRange(BitConverter.GetBytes(value));
            }

            internal static void WriteString(string data)
            {
                var buffer = Encoding.UTF8.GetBytes(data);
                WriteVarInt(buffer.Length);
                _buffer.AddRange(buffer);
            }

            internal static void Write(byte b)
            {
                _stream.WriteByte(b);
            }

            internal static void Flush(int id = -1)
            {
                var buffer = _buffer.ToArray();
                _buffer.Clear();

                var add = 0;
                var packetData = new[] { (byte)0x00 };
                if (id >= 0)
                {
                    WriteVarInt(id);
                    packetData = _buffer.ToArray();
                    add = packetData.Length;
                    _buffer.Clear();
                }

                WriteVarInt(buffer.Length + add);
                var bufferLength = _buffer.ToArray();
                _buffer.Clear();

                _stream.Write(bufferLength, 0, bufferLength.Length);
                _stream.Write(packetData, 0, packetData.Length);
                _stream.Write(buffer, 0, buffer.Length);
            }
            #endregion
        }

        #region Server ping 
        /// <summary>
        /// C# represenation of the following JSON file
        /// https://gist.github.com/thinkofdeath/6927216
        /// </summary>
        class PingPayload
        {
        public class Version
        {
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int protocol { get; set; }
        }

        public class Sample
        {
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
        }

        public class Players
        {
            /// <summary>
            /// 
            /// </summary>
            public int max { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int online { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<Sample> sample { get; set; }
        }

        public class Extra
        {
            /// <summary>
            /// 
            /// </summary>
            public string color { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string text { get; set; }
        }

        public class Description
        {
            /// <summary>
            /// 
            /// </summary>
            public List<Extra> extra { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string text { get; set; }
        }

        public class Modinfo
        {
            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> modList { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public Version version { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Players players { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Description description { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string favicon { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Modinfo modinfo { get; set; }
        }

    }

    class VersionPayload
        {
            [JsonProperty(PropertyName = "protocol")]
            public int Protocol { get; set; }

            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
        }

        class PlayersPayload
        {
            [JsonProperty(PropertyName = "max")]
            public int Max { get; set; }

            [JsonProperty(PropertyName = "online")]
            public int Online { get; set; }

            [JsonProperty(PropertyName = "sample")]
            public List<Player> Sample { get; set; }
        }

        class Player
        {
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }
        }
        #endregion
    }