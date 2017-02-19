using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailAudio.API
{
    /// <summary>
    /// Represents the composer of an <see cref="Album"/> 
    /// </summary>
    public class Composer : IEquatable<Composer>
    {
        private readonly MusicDB _db;
        private readonly long _rowid;
        private readonly string _name;

        internal Composer(MusicDB db, long rowid, string name)
        {
            _db = db;
            _rowid = rowid;
            _name = name;
        }

    /// <summary>
    /// Returns the composer's name
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

        public bool Equals(Composer obj)
        {
            return obj != null && obj._db == _db && obj._rowid == _rowid;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Composer);
        }

        internal long Rowid
        {
            get { return _rowid; }
        }

    }
}
