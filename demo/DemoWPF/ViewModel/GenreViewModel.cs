using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocktailAudio.API;
using GalaSoft.MvvmLight;

namespace DemoWPF.ViewModel
{
    public class GenreViewModel : ViewModelBase
    {
        private readonly Genre _genre;
        private ArtistViewModel[] _artists;

        public GenreViewModel(Genre genre)
        {
            _genre = genre;
        }

        public string Name
        {
            get { return _genre.Name; }
        }

        public Uri ImageUri
        {
            get { return _genre.GetImageUri(ImageSize.Small); }
        }

        public ArtistViewModel[] Artists
        {
            get
            {
                return _artists ?? (_artists = _genre.Artists.Select(_ => new ArtistViewModel(_)).ToArray());
            }
        }
    }
}
