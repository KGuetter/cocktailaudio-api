using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailAudio.API
{
    /// <summary>
    /// Represents a CocktailAudio device.
    /// </summary>
    public class Device
    {
        #region Private Fields
        private readonly string _name;
        private readonly string _ipAddress;
        #endregion

        /// <summary>
        /// Scans the network for CocktailAudio devices
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Device> Discover()
        {
            yield break;
        }

        /// <summary>
        /// Initialize a new instance of <see cref="Device"/> with known IP Address
        /// </summary>
        /// <param name="name">The device name</param>
        /// <param name="ipAddress">The device's IP address</param>
        public Device(string name, string ipAddress)
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
        public string IPAddress
        {
            get { return _ipAddress; }
        }

        /// <summary>
        /// Returns a <see cref="MusicDB"/> object allowing access to the device's music database.
        /// </summary>
        public MusicDB MusicDB
        {
            get { return null; }
        }
    }
}
