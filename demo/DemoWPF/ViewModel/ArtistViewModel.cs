using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocktailAudio.API;
using GalaSoft.MvvmLight;

namespace DemoWPF.ViewModel
{
    public class ArtistViewModel : ViewModelBase
    {
        private readonly Artist _artist;
        private AlbumViewModel[] _albums;

        public ArtistViewModel(Artist artist)
        {
            _artist = artist;
        }

        public string Name
        {
            get { return _artist.Name; }
        }

        public Uri ImageUri
        {
            get { return _artist.GetImageUri(ImageSize.Small); }
        }

        public AlbumViewModel[] Albums
        {
            get
            {
                return _albums ?? (_albums = _artist.Albums.Select(_ => new AlbumViewModel(_)).ToArray());
            }
        }
    }
}
