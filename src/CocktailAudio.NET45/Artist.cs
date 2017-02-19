using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailAudio.API
{
    /// <summary>
    /// Represents the performer of an <see cref="Album"/> 
    /// </summary>
    public class Artist : IEquatable<Artist>
    {
        private readonly MusicDB _db;
        private readonly long _rowid;
        private readonly string _name;

        internal Artist(MusicDB db, long rowid, string name)
        {
            _db = db;
            _rowid = rowid;
            _name = name;
        }

        /// <summary>
        /// Returns the artist's name
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Returns the Url where the image can be fetched
        /// </summary>
        public Uri Image
        {
            get
            {
                return _db.MakeUri(string.Format(@"ArtistArt\{0:D2}\[{1:D4}] {2}\folder.jpg",
                    _rowid % 100, _rowid, _name));
            }
        }

        /// <summary>
        /// Returns all albums containing at least one track from this artist
        /// </summary>
        public IEnumerable<Album> Albums
        {
            get { return _db.QueryAlbums(string.Format("ROWID IN (SELECT AlbumID FROM Song WHERE ArtistID={0})", _rowid)); }
        }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return unchecked((int)_rowid);
        }

        public bool Equals(Artist obj)
        {
            return obj != null && obj._db == _db && obj._rowid == _rowid;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Artist);
        }

        internal long Rowid
        {
            get { return _rowid; }
        }

    }
}
