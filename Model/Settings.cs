using AudioFil.Helpers;

namespace AudioFil
{
    public class Settings : BindableBase
    {
		private string musicPath;
		public string MusicPath
		{
			get => musicPath;
			set => SetProperty(ref musicPath, value, nameof(MusicPath));
		}

		private string playlistPath;
		public string PlaylistPath
		{
			get => playlistPath;
			set => SetProperty(ref playlistPath, value, nameof(PlaylistPath));
		}

	}
}
