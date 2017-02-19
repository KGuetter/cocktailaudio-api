using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CocktailAudio.API.Test
{
    [TestFixture]
    public class DeviceTest
    {
        [Test]
        public void DiscoveryTest()
        {
            var devices = Device.Discover().ToArray();

            foreach (var device in devices)
            {
                Console.WriteLine(device);
            }
        }
    }
}
