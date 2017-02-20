using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
        private readonly MusicDB _db;
        private readonly long _rowid;
        private readonly string _name;

        internal Album(MusicDB db, long rowid, string name)
        {
            _db = db;
            _rowid = rowid;
            _name = name;
        }

        /// <summary>
        /// Returns the album name
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Returns the Url where the image can be fetched
        /// </summary>
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
            return _db.MakeUri(string.Format(@"[{0:D4}] {1}\{2}",
                _rowid, _name, fileName));
        }


        /// <summary>
        /// Returns the album's performer
        /// </summary>
        public Artist Artist
        {
            get { return null; }
        }

        /// <summary>
        /// Returns the album's composer
        /// </summary>
        public Composer Composer
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
        public IEnumerable<Track> Tracks
        {
            get { return _db.QueryTracks(string.Format("AlbumID={0}", _rowid)); }
        }


        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return unchecked((int)_rowid);
        }

        public bool Equals(Album obj)
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
