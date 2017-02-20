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
        private readonly int _trackNumber;
        private readonly string _name;
        private readonly long _genreId;
        private readonly long _artistId;
        private readonly TimeSpan _duration;

        internal Track(MusicDB db, long rowid, int trackNumber, string name, long genreId, long artistId, TimeSpan duration)
        {
            _db = db;
            _rowid = rowid;
            _trackNumber = trackNumber;
            _name = name;
            _genreId = genreId;
            _artistId = artistId;
            _duration = duration;
        }

        /// <summary>
        /// Returns the track number on the album
        /// </summary>
        public int TrackNumber
        {
            get { return _trackNumber; }
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
            get { return _db.GetGenre(_genreId); }
        }

        public Artist Artist
        {
            get { return _db.GetArtist(_artistId); }
        }

        public TimeSpan Duration
        {
            get { return _duration; }
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
