using System;
using System.Collections.Generic;
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
