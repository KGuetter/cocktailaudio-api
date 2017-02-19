using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CocktailAudio.API.Test
{
    /// <summary>
    /// Tests for the <see cref="MusicDB"/> class with a local test database
    /// </summary>
    [TestFixture]
    public class MusicDBTests
    {
        private static readonly Device TestDevice = Device.Discover().FirstOrDefault();

        [Test]
        public void VerifyGenres()
        {
            if (TestDevice == null)
                Assert.Inconclusive("No CocktailAudio device found on network");

            var sut = TestDevice.MusicDB;

            var genres = sut.Genres.ToArray();

            Assert.That(genres, Is.Not.Empty);
        }

        [Test]
        public void VerifyArtists()
        {
            if (TestDevice == null)
                Assert.Inconclusive("No CocktailAudio device found on network");

            var sut = TestDevice.MusicDB;

            var artists = sut.Artists.ToArray();

            Assert.That(artists, Is.Not.Empty);
        }

        [Test]
        public void VerifyComposers()
        {
            if (TestDevice == null)
                Assert.Inconclusive("No CocktailAudio device found on network");

            var sut = TestDevice.MusicDB;

            var composers = sut.Composers.ToArray();

            Assert.That(composers, Is.Not.Empty);
        }
    }
}
