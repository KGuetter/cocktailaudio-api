using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
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
        private readonly string _databasePath;
        private readonly string _connectionString;
        private DateTime _lastModified;
        private Genre[] _genres;
        private Artist[] _artists;
        private Composer[] _composers;
        #endregion

        /// <summary>
        /// Initialize a new instance of the <see cref="MusicDB"/> class.
        /// </summary>
        /// <param name="databasePath">Path (local or UNC) to .songs.db</param>
        public MusicDB(string databasePath)
        {
            _databasePath = databasePath;
            _lastModified = new FileInfo(_databasePath).LastWriteTimeUtc;
            var builder = new SQLiteConnectionStringBuilder
            {
                DataSource = databasePath,
                FailIfMissing = true,
                ReadOnly = true
            };
            _connectionString = builder.ToString();
        }

        /// <summary>
        /// Returns a list of all genres
        /// </summary>
        public IEnumerable<Genre> Genres
        {
            get
            {
                InvalidatedCacheIfModified();
                if (_genres == null)
                {
                    var genres = new List<Genre>();
                    using (var connection = new SQLiteConnection(_connectionString, true))
                    {
                        connection.Open();
                        using (var command = new SQLiteCommand("SELECT ROWID, Name FROM Genre", connection))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    genres.Add(new Genre(this, reader.GetInt64(0), reader.GetString(1)));
                                }
                            }
                        }
                    }
                    _genres = genres.ToArray();
                }
                return _genres;
            }
        }

        /// <summary>
        /// Returns a list of all performers
        /// </summary>
        public IEnumerable<Artist> Artists
        {
            get
            {
                InvalidatedCacheIfModified();
                if (_artists == null)
                {
                    var artists = new List<Artist>();
                    using (var connection = new SQLiteConnection(_connectionString, true))
                    {
                        connection.Open();
                        using (var command = new SQLiteCommand("SELECT ROWID, Name FROM Artist", connection))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    artists.Add(new Artist(this, reader.GetInt64(0), reader.GetString(1)));
                                }
                            }
                        }
                    }
                    _artists = artists.ToArray();
                }
                return _artists;
            }
        }

        /// <summary>
        /// Returns a list of all composers
        /// </summary>
        public IEnumerable<Composer> Composers
        {
            get
            {
                InvalidatedCacheIfModified();
                if (_composers == null)
                {
                    var composers = new List<Composer>();
                    using (var connection = new SQLiteConnection(_connectionString, true))
                    {
                        connection.Open();
                        using (var command = new SQLiteCommand("SELECT ROWID, Name FROM Composer", connection))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    composers.Add(new Composer(this, reader.GetInt64(0), reader.GetString(1)));
                                }
                            }
                        }
                    }
                    _composers = composers.ToArray();
                }
                return _composers;
            }
        }

        /// <summary>
        /// Returns all <see cref="Album"/>s filtered by the specified conditions
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Album> QueryAlbums(/* condition*/)
        {
            yield break;
        }

        #region Private implementation

        private void InvalidatedCacheIfModified()
        {
            var lastModified = new FileInfo(_databasePath).LastWriteTimeUtc;
            if (lastModified != _lastModified)
            {
                _genres = null;
                _artists = null;
                _composers = null;
                _lastModified = lastModified;
            }
        }
        #endregion
    }
}
