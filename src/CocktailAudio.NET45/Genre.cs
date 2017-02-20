using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailAudio.API
{
    /// <summary>
    /// Represents a genre
    /// </summary>
    public class Genre : IEquatable<Genre>
    {
        private readonly MusicDB _db;
        private readonly long _rowid;
        private readonly string _name;

        internal Genre(MusicDB db, long rowid, string name)
        {
            _db = db;
            _rowid = rowid;
            _name = name;
        }

        /// <summary>
        /// Returns the genre name
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        public Uri GetImageUri(ImageSize size)
        {
            string fileName;
            switch (size)
            {
                case ImageSize.Small:
                    fileName = "folder_s.jpg";
                    break;
                case ImageSize.A:
                    fileName = "folder_a.jpg";
                    break;
                case ImageSize.Medium:
                    fileName = "folder_m.jpg";
                    break;
                case ImageSize.Large:
                default:
                    fileName = "folder.jpg";
                    break;
            }
            return _db.MakeUri(string.Format(@"GenreArt\{0:D2}\[{1:D4}] {2}\{3}",
                _rowid % 100, _rowid, _name, fileName));
        }

        /// <summary>
        /// Returns all albums related to the genre.
        /// </summary>
        public IEnumerable<Album> Albums
        {
            get { return _db.QueryAlbums(string.Format("ROWID IN (SELECT AlbumID FROM Song WHERE GenreID={0})", _rowid)); }
        }

        /// <summary>
        /// Returns all artists related to the genre.
        /// </summary>
        public IEnumerable<Artist> Artists
        {
            get { return _db.QueryGenreArtists(_rowid); }
        }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return unchecked((int)_rowid);
        }

        public bool Equals(Genre obj)
        {
            return obj != null && obj._db == _db && obj._rowid == _rowid;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Genre);
        }

        internal long Rowid
        {
            get { return _rowid; }
        }
    }
}
