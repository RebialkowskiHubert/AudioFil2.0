namespace AudioFil
{
    public sealed class SongsViewModel : PlayerViewModel
    {
        public SongsViewModel()
        {
            Elements = xml.LoadSongs();

            player.EndReached += (o, e) => GoToNext();
        }

        private void GoToNext()
        {
            if (Elements == null)
                return;

            int index = Elements.IndexOf(SelectedElement);
            index++;

            InitVlcPlayer();
            player.EndReached += (o, e) => GoToNext();

            if (index < Elements.Count)
                SelectedElement = Elements[index];
            else
                SelectedElement = Elements[0];
        }
    }
}
