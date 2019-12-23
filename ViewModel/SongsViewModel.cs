namespace AudioFil
{
    public sealed class SongsViewModel : PlayerViewModel
    {
        public SongsViewModel()
        {
            Elements = xml.LoadSongs();
        }
    }
}
