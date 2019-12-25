using AudioFil.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Interops.Signatures;

namespace AudioFil
{
    public class PlayerViewModel : BindableBase
    {
        private string title = "Brak tytułu";
        public string Title
        {
            get => title;
            set
            {
                if (title != value)
                {
                    if (string.IsNullOrEmpty(value))
                        title = "Brak tytułu";

                    SetProperty(ref title, value, nameof(Title));
                }
            }
        }

        private string description = "Gotowy";
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value, nameof(Description));
        }

        private ObservableCollection<BaseSource> elements;
        public ObservableCollection<BaseSource> Elements
        {
            get => elements;
            set => SetProperty(ref elements, value, nameof(Elements));
        }

        private BaseSource selectedElement;
        public BaseSource SelectedElement
        {
            get => selectedElement;
            set
            {
                if (selectedElement != null)
                    selectedElement.OnCurrentSongChanged -= OnSongChange;

                SetProperty(ref selectedElement, value, nameof(SelectedElement));
                OnSelectedElementChange(selectedElement, value);
            }
        }

        private string playButtonContent;
        public string PlayButtonContent
        {
            get => playButtonContent;
            set => SetProperty(ref playButtonContent, value, nameof(PlayButtonContent));
        }


        public RelayCommand PlayCommand { get; set; }
        public RelayCommand PlayPauseCommand { get; set; }
        public RelayCommand StopCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand PreviousCommand { get; set; }

        protected VlcMediaPlayer player;
        protected XMLHandling xml;
        protected LowLevelKeyboardListener keyListener;
        protected bool play = false;


        public PlayerViewModel()
        {
            PlayCommand = new RelayCommand(Play);
            PlayPauseCommand = new RelayCommand(PlayPause);
            StopCommand = new RelayCommand(Stop);
            NextCommand = new RelayCommand(Next);
            PreviousCommand = new RelayCommand(Previous);

            PlayButtonContent = "Play";

            InitPlayer();
        }


        protected virtual void Play()
        {
            try
            {
                if (SelectedElement == null)
                {
                    return;
                }

                player.SetMedia(SelectedElement.Url);
                player.Play();

                play = true;
                PlayButtonContent = "Pause";

                SelectedElement.OnCurrentSongChanged += OnSongChange;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected virtual void Pause()
        {
            player.Pause();
            play = false;
            PlayButtonContent = "Play";
        }

        protected virtual void Stop()
        {
            player.Stop();
            Title = "";
        }

        protected virtual void Next()
        {
            int index = Elements.IndexOf(SelectedElement) + 1;

            if (index < Elements.Count)
                SelectedElement = Elements[index];
        }

        protected virtual void Previous()
        {
            int index = Elements.IndexOf(SelectedElement) - 1;

            if (index >= 0)
                SelectedElement = (Radio)Elements[index];
            else
                SelectedElement = (Radio)Elements[Elements.Count - 1];
        }

        protected virtual void OnSelectedElementChange(BaseSource oldValue, BaseSource newValue)
        {
            if (newValue != null)
            {
                Play();

                if (SelectedElement.CurrentSong != null)
                    Title = SelectedElement.CurrentSong.Artist + " - " + SelectedElement.CurrentSong.Title;
            }
        }

        protected virtual void OnSongChange(object sender, CurrentSongEventArgs e)
        {

        }

        protected virtual bool IsSelected()
        {
            return SelectedElement != null;
        }

        protected void InitVlcPlayer()
        {
            DirectoryInfo libDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(),
                                                                        "libvlc",
                                                                        IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            string[] options = { };

            player = new VlcMediaPlayer(libDirectory);

            player.Buffering += (o, e) => CheckStatus();
            player.EncounteredError += (o, e) => CheckStatus();
            player.EndReached += (o, e) => CheckStatus();
            player.MediaChanged += (o, e) => CheckStatus();
            player.Opening += (o, e) => CheckStatus();
            player.Paused += (o, e) => CheckStatus();
            player.Stopped += (o, e) => CheckStatus();
        }


        private void PlayPause()
        {
            if (play)
                Pause();
            else
                Play();
        }

        private void InitPlayer()
        {
            try
            {
                xml = new XMLHandling();

                keyListener = new LowLevelKeyboardListener();

                App.Current.Exit += (ss, ee) => {
                    keyListener.UnHookKeyboard();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            InitVlcPlayer();
        }

        private void CheckStatus()
        {
            switch (player.State)
            {
                case MediaStates.Opening:
                    Description = "Otwieranie";
                    break;

                case MediaStates.Buffering:
                    Description = "Buforowanie";
                    break;

                case MediaStates.Playing:
                    Description = "Odtwarzanie";
                    break;

                case MediaStates.Paused:
                    Description = "Pauza";
                    break;

                case MediaStates.Stopped:
                    Description = "Zatrzymanie";
                    break;

                case MediaStates.Ended:
                    Description = "Zakończono";
                    break;

                case MediaStates.Error:
                    Description = "Błąd";
                    break;

                default:
                    Description = "Brak";
                    break;
            }
        }
    }
}
