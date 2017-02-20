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
        private Dictionary<long, Genre> _genres;
        private Dictionary<long, Artist> _artists;
        private Dictionary<long, Composer> _composers;
        private Dictionary<long, Artist[]> _genreArtists;
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
                EnsureGenresLoaded();
                return _genres.Values;
            }
        }

        /// <summary>
        /// Returns a list of all performers
        /// </summary>
        public IEnumerable<Artist> Artists
        {
            get
            {
                EnsureArtistsLoaded();
                return _artists.Values;
            }
        }

        /// <summary>
        /// Returns a list of all composers
        /// </summary>
        public IEnumerable<Composer> Composers
        {
            get
            {
                EnsureComposersLoaded();
                return _composers.Values;
            }
        }

        #region Helpers for data classes

        internal Genre GetGenre(long rowid)
        {
            EnsureGenresLoaded();
            return _genres[rowid];
        }

        internal Artist GetArtist(long rowid)
        {
            EnsureArtistsLoaded();
            return _artists[rowid];
        }

        internal Composer GetComposer(long rowid)
        {
            EnsureComposersLoaded();
            return _composers[rowid];
        }

        /// <summary>
        /// Returns all <see cref="Artist"/>s related to a genre
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<Artist> QueryGenreArtists(long genreId)
        {
            if (_genreArtists == null)
                _genreArtists = new Dictionary<long, Artist[]>();
            Artist[] artists;
            if (!_genreArtists.TryGetValue(genreId, out artists))
            {
                var list = new List<Artist>();
                using (var connection = new SQLiteConnection(_connectionString, true))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand("SELECT DISTINCT ArtistID FROM Song WHERE GenreID=" + genreId, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(GetArtist(reader.GetInt64(0)));
                            }
                        }
                    }
                }
                artists = list.ToArray();
                _genreArtists.Add(genreId, artists);
            }
            return artists;
        }

        /// <summary>
        /// Returns all <see cref="Album"/>s filtered by the specified condition
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<Album> QueryAlbums(string condition)
        {
            using (var connection = new SQLiteConnection(_connectionString, true))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT ROWID, Name FROM Album WHERE " + condition, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Album(this, reader.GetInt64(0), reader.GetString(1));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns all <see cref="Track"/>s filtered by the specified condition
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<Track> QueryTracks(string condition)
        {
            using (var connection = new SQLiteConnection(_connectionString, true))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT ROWID, Track, Name, GenreID, ArtistID, nov_Time FROM Song WHERE " + condition + " ORDER BY Track", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Track(this, reader.GetInt64(0), 
                                reader.GetInt32(1), reader.GetString(2), reader.GetInt64(3), reader.GetInt64(4), TimeSpan.FromSeconds(reader.GetInt32(5)));
                        }
                    }
                }
            }
        }

        internal Uri MakeUri(string relativePath)
        {
            var path = Path.Combine(Path.GetDirectoryName(_databasePath), relativePath);
            if (File.Exists(path))
                return new Uri(path);
            return null;
        }

        #endregion

        #region Private implementation

        private void InvalidatedCacheIfModified()
        {
            var lastModified = new FileInfo(_databasePath).LastWriteTimeUtc;
            if (lastModified != _lastModified)
            {
                _genres = null;
                _artists = null;
                _composers = null;
                _genreArtists = null;
                _lastModified = lastModified;
            }
        }


        private void EnsureGenresLoaded()
        {
            InvalidatedCacheIfModified();
            if (_genres == null)
            {
                _genres = new Dictionary<long, Genre>();
                using (var connection = new SQLiteConnection(_connectionString, true))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand("SELECT ROWID, Name FROM Genre", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var rowid = reader.GetInt64(0);
                                _genres.Add(rowid, new Genre(this, rowid, reader.GetString(1)));
                            }
                        }
                    }
                }
            }
        }


        private void EnsureArtistsLoaded()
        {
            InvalidatedCacheIfModified();
            if (_artists == null)
            {
                _artists = new Dictionary<long, Artist>();
                using (var connection = new SQLiteConnection(_connectionString, true))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand("SELECT ROWID, Name FROM Artist", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var rowid = reader.GetInt64(0);
                                _artists.Add(rowid, new Artist(this, rowid, reader.GetString(1)));
                            }
                        }
                    }
                }
            }
        }

        private void EnsureComposersLoaded()
        {
            InvalidatedCacheIfModified();
            if (_composers == null)
            {
                _composers = new Dictionary<long, Composer>();
                using (var connection = new SQLiteConnection(_connectionString, true))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand("SELECT ROWID, Name FROM Composer", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var rowid = reader.GetInt64(0);
                                _composers.Add(rowid, new Composer(this, rowid, reader.GetString(1)));
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
