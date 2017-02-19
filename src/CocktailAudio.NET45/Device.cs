using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CocktailAudio.API
{
    /// <summary>
    /// Represents a CocktailAudio device.
    /// </summary>
    public class Device
    {
        #region Private Fields
        private readonly string _name;
        private readonly IPAddress _ipAddress;
        #endregion

        /// <summary>
        /// Scans the network for CocktailAudio devices
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Device> Discover()
        {
          return Dns.GetHostAddresses(Environment.MachineName)
            .Where(_ => _.AddressFamily == AddressFamily.InterNetwork && !Equals(_, IPAddress.Loopback))
            .AsParallel()
            .SelectMany(UpnpDiscovery)
            .AsEnumerable();
        }

        /// <summary>
        /// Initialize a new instance of <see cref="Device"/> with known IP Address
        /// </summary>
        /// <param name="name">The device name</param>
        /// <param name="ipAddress">The device's IP address</param>
        public Device(string name, IPAddress ipAddress)
        {
            _name = name;
            _ipAddress = ipAddress;
        }

        /// <summary>
        /// Returns the device name
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Returns the IP Address
        /// </summary>
        public IPAddress IPAddress
        {
            get { return _ipAddress; }
        }

        /// <summary>
        /// Returns a <see cref="MusicDB"/> object allowing access to the device's music database.
        /// </summary>
        public MusicDB MusicDB
        {
            get
            {
                var databasePath = @"\\" + IPAddress + @"\LocalStorages\hdd1\.DB\.songs.db";
                return new MusicDB(databasePath);
            }
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", IPAddress, Name);
        }

        #region Private implementation

        private static IEnumerable<Device> UpnpDiscovery(IPAddress localAddress)
        {
            var localEndPoint = new IPEndPoint(localAddress, 0);
            var multicastEndPoint = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 1900);

            using (var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                udpSocket.Bind(localEndPoint);

                const string searchString =
                    "M-SEARCH * HTTP/1.1\r\nHOST: 239.255.255.250:1900\r\nMAN: \"ssdp:discover\"\r\nMX: 3\r\nST: ssdp:all\r\n\r\n";

                udpSocket.SendTo(Encoding.UTF8.GetBytes(searchString), SocketFlags.None, multicastEndPoint);

                var receiveBuffer = new byte[64000];

                var sw = Stopwatch.StartNew();
                var answersReceivedFrom = new HashSet<IPAddress>();

                while (sw.ElapsedMilliseconds < 1000)
                {
                    if (udpSocket.Available > 0)
                    {
                        EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                        var receivedBytes = udpSocket.ReceiveFrom(receiveBuffer, SocketFlags.None, ref sender);

                        if (receivedBytes > 0)
                        {
                            var ipAddres = ((IPEndPoint)sender).Address;
                            var response = Encoding.UTF8.GetString(receiveBuffer, 0, receivedBytes);
                            if (response.StartsWith("NOTIFY * HTTP/1.1\r\n"))
                            {
                                var data = response.Substring("NOTIFY * HTTP/1.1\r\n".Length)
                                    .Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(_ =>
                                    {
                                        var pos = _.IndexOf(':');
                                        return pos >= 0
                                            ? Tuple.Create(_.Substring(0, pos), _.Substring(pos + 1).TrimStart())
                                            : Tuple.Create(_, (string)null);
                                    })
                                    .ToDictionary(_ => _.Item1, _ => _.Item2);
                                if (data["NT"] == "upnp:rootdevice"
                                    && data.ContainsKey("Server") && data["Server"].Contains("CocktailAudio")
                                    && answersReceivedFrom.Add(ipAddres))
                                {
                                    Console.WriteLine(response);
                                    string name = null;
                                    if (data.ContainsKey("LOCATION"))
                                    {
                                        using (var webClient = new WebClient())
                                        {
                                            var description = XDocument.Load(webClient.OpenRead(data["LOCATION"]));
                                            XNamespace ud = "urn:schemas-upnp-org:device-1-0";
                                            var friendlyName = description.Root.Element(ud + "device").Element(ud + "friendlyName");
                                            name = friendlyName.Value;
                                        }
                                    }
                                    yield return new Device(name ?? ipAddres.ToString(), ipAddres);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
