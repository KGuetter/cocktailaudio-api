using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocktailAudio.API;
using GalaSoft.MvvmLight;

namespace DemoWPF.ViewModel
{
    public class AlbumViewModel : ViewModelBase
    {
        private readonly Album _album;
        private TrackViewModel[] _tracks;

        public AlbumViewModel(Album album)
        {
            _album = album;
        }

        public string Name
        {
            get { return _album.Name; }
        }

        public Uri ImageUri
        {
            get { return _album.GetImageUri(ImageSize.Small); }
        }

        public TrackViewModel[] Tracks
        {
            get
            {
                return _tracks ?? (_tracks = _album.Tracks.Select(_ => new TrackViewModel(_)).ToArray());
            }
        }
    }
}
