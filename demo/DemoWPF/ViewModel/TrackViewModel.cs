using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocktailAudio.API;
using GalaSoft.MvvmLight;

namespace DemoWPF.ViewModel
{
    public class TrackViewModel : ViewModelBase
    {
        private readonly Track _track;

        public TrackViewModel(Track track)
        {
            _track = track;
        }

        public int TrackNumber
        {
            get { return _track.TrackNumber; }
        }

        public string Name
        {
            get { return _track.Name; }
        }

        public TimeSpan Duration
        {
            get { return _track.Duration; }
        }
    }
}
