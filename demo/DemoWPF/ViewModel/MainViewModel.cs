using CocktailAudio.API;
using GalaSoft.MvvmLight;
using System.Linq;
using System.Windows;

namespace DemoWPF.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private Device _device;
        private MusicDB _db;
        private GenreViewModel[] _genres;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (!IsInDesignMode)
            {
                _device = Device.Discover().FirstOrDefault();
                if (_device != null)
                    _db = _device.MusicDB;
            }
        }

        public Device Device
        {
            get { return _device; }
        }

        public GenreViewModel[] Genres
        {
            get
            {
                return _genres ?? (_genres = _db.Genres.Select(_ => new GenreViewModel(_)).ToArray());
            }
        }
    }
}