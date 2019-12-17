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
        protected VlcMediaPlayer player;

        protected XMLHandling xml;

        protected LowLevelKeyboardListener keyListener;

        protected bool play = false;

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
                SetProperty(ref selectedElement, value, nameof(SelectedElement));
                OnSelectedElementChange(value);
            }
        }


        public RelayCommand PlayCommand { get; set; }
        public RelayCommand StopCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand PreviousCommand { get; set; }


        public PlayerViewModel()
        {
            PlayCommand = new RelayCommand(Play);
            StopCommand = new RelayCommand(Stop);
            NextCommand = new RelayCommand(Next);
            PreviousCommand = new RelayCommand(Previous);

            InitPlayer();
        }

        protected virtual void Play()
        {
            if (SelectedElement == null)
            {
                if (Elements != null && Elements[0] != null)
                    SelectedElement = (Radio)Elements[0];
                else
                    return;
            }

            player.SetMedia(SelectedElement.Url);

            player.Play();

            SelectedElement.OnCurrentSongChanged += OnSongChange;
        }

        protected virtual void Stop()
        {
            player.Stop();
            Title = "";
        }

        protected virtual void Next()
        {
           
        }

        protected virtual void Previous()
        {
            
        }

        protected virtual void OnSelectedElementChange(BaseSource baseSource)
        {

        }

        protected virtual void OnSongChange(object sender, CurrentSongEventArgs e)
        {

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
