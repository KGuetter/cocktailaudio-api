using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailAudio.API
{
    /// <summary>
    /// Represents an Album
    /// </summary>
    public class Album
    {

        /// <summary>
        /// Returns the album name
        /// </summary>
        public string Name
        {
            get { return null; }
        }

        /// <summary>
        /// Returns the album's performer
        /// </summary>
        public Artist Performer
        {
            get { return null; }
        }

        /// <summary>
        /// Returns the album's composer
        /// </summary>
        public Artist Composer
        {
            get { return null; }
        }

        /// <summary>
        /// Returns the album's genre
        /// </summary>
        public Genre Genre
        {
            get { return null; }
        }

        /// <summary>
        /// Returns the album's tracks
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Track> GetTracks
        {
            get { yield break; }
        }
    }
}
