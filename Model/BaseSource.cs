using AudioFil.Helpers;
using System;

namespace AudioFil
{
    public class BaseSource : BindableBase
    {
        public event EventHandler<CurrentSongEventArgs> OnCurrentSongChanged;

        private int id;
        public int Id
        {
            get => id;
            set => id = value;
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value, nameof(Name));
        }

        private Uri url;
        public Uri Url
        {
            get => url;
            set => SetProperty(ref url, value, nameof(Url));
        }

        private SongInfo currentSong;
        public SongInfo CurrentSong
        {
            get => currentSong;
            set
            {
                SetProperty(ref currentSong, value, nameof(CurrentSong));
                OnCurrentSongChanged?.Invoke(this, new CurrentSongEventArgs(currentSong, value));
            }
        }
    }
}
