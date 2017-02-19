using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailAudio.API
{
    /// <summary>
    /// Represents a music track
    /// </summary>
    public class Track : IEquatable<Track>
    {
        private readonly MusicDB _db;
        private readonly long _rowid;
        private readonly string _name;
        private readonly long _genreId;
        private readonly long _artistId;

        internal Track(MusicDB db, long rowid, string name, long genreId, long artistId)
        {
            _db = db;
            _rowid = rowid;
            _name = name;
            _genreId = genreId;
            _artistId = artistId;
        }

        /// <summary>
        /// Returns the track name
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        public Genre Genre
        {
            get { return _db.Genres.First(_ => _.Rowid == _genreId); }
        }

        public Artist Artist
        {
            get { return _db.Artists.First(_ => _.Rowid == _artistId); }
        }

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return unchecked((int)_rowid);
        }

        public bool Equals(Track obj)
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
