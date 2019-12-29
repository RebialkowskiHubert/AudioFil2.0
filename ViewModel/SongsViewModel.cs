using System;
using System.Windows.Threading;

namespace AudioFil
{
    public sealed class SongsViewModel : PlayerViewModel
    {
        private float totalSongTime;
        public float TotalSongTime
        {
            get => totalSongTime;
            set => SetProperty(ref totalSongTime, value, nameof(TotalSongTime));
        }

        private float currentSongTime;
        public float CurrentSongTime
        {
            get => currentSongTime;
            set => SetProperty(ref currentSongTime, value, nameof(CurrentSongTime));
        }

        private string currentSongTimeString;
        public string CurrentSongTimeString
        {
            get => currentSongTimeString;
            set => SetProperty(ref currentSongTimeString, value, nameof(CurrentSongTimeString));
        }



        private bool isDraggingSlider;

        public SongsViewModel()
        {
            Elements = xml.LoadSongs();

            player.EndReached += (o, e) => GoToNext();

            InitTimer();
        }

        public void StartDrag()
        {
            isDraggingSlider = true;
        }

        public void StopDrag()
        {
            isDraggingSlider = false;
            player.Position = CurrentSongTime;
        }

        public void ValueChange()
        {
            CurrentSongTimeString = TimeSpan.FromMilliseconds(player.Time).ToString(@"hh\:mm\:ss");
        }

        private void InitTimer()
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if(SelectedElement != null && player.Time != 0 && !isDraggingSlider)
            {
                TotalSongTime = player.Position;
                CurrentSongTime = player.Position;
            }
        }

        private void GoToNext()
        {
            CurrentSongTimeString = "00:00:00";

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
