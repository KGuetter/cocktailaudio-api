using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailAudio.API
{
    /// <summary>
    /// Allows read-only access to the music database
    /// </summary>
    public class MusicDB
    {
        #region Private Fields
        //private SQLiteConnection _connection;
        #endregion

        /// <summary>
        /// Returns a list of all genres
        /// </summary>
        public IEnumerable<Genre> Genres
        {
            get { yield break; }
        }

        /// <summary>
        /// Returns a list of all performers
        /// </summary>
        public IEnumerable<Artist> Performers
        {
            get { yield break; }
        }

        /// <summary>
        /// Returns a list of all composers
        /// </summary>
        public IEnumerable<Artist> Composers
        {
            get { yield break; }
        }

        /// <summary>
        /// Returns all <see cref="Album"/>s filtered by the specified conditions
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Album> QueryAlbums(/* condition*/)
        {
            yield break;
        }
    }
}
