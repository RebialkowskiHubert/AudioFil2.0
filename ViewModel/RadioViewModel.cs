﻿using AudioFil.Helpers;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Tulpep.NotificationWindow;

namespace AudioFil
{
    public sealed class RadioViewModel : PlayerViewModel
    { 
        public RelayCommand<Radio> AddCommand { get; set; }
        public RelayCommand UpdateCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        public RadioViewModel()
        {
            Elements = xml.LoadRadios();

            AddCommand = new RelayCommand<Radio>(Add);
            UpdateCommand = new RelayCommand(Update, IsSelected);
            DeleteCommand = new RelayCommand(Delete, IsSelected);

            keyListener.OnKeyPressed += OnKeyPressed;
            keyListener.HookKeyboard();

            InitRadioListener();
        }

        protected override void Next()
        {
            int index = Elements.IndexOf(SelectedElement) + 1;

            if (index < Elements.Count)
                SelectedElement = Elements[index];
        }

        protected override void Previous()
        {
            int index = Elements.IndexOf(SelectedElement) - 1;

            if (index >= 0)
                SelectedElement = (Radio)Elements[index];
            else
                SelectedElement = (Radio)Elements[Elements.Count - 1];
        }

        protected override void OnSelectedElementChange(BaseSource value)
        {
            if (value != null)
            {
                SelectedElement.OnCurrentSongChanged -= OnSongChange;
                Play();

                Title = SelectedElement.CurrentSong.Artist + " - " + SelectedElement.CurrentSong.Title;
            }

            DeleteCommand.RaiseCanExecuteChanged();
            UpdateCommand.RaiseCanExecuteChanged();
        }

        protected override void OnSongChange(object sender, CurrentSongEventArgs e)
        {
            Title = e.NewSong.Artist + " - " + e.NewSong.Title;

            App.Current.Dispatcher.InvokeAsync(() =>
            {
                PopupNotifier popup = new PopupNotifier
                {
                    TitleText = SelectedElement.Name,
                    ContentText = Title
                };
                popup.Popup();
            });
        }

        private void InitRadioListener()
        {
            foreach (Radio radio in Elements)
            {
                RadioListener listener = new RadioListener(radio);
                radio.OnMetadataChanged += listener.UpdateCurrentSong;
                listener.Start();

                radio.OnCurrentSongChanged += (ss, ee) =>
                {
                    App.Current.Dispatcher.InvokeAsync(() =>
                    {
                        radio.CurrentSong.Artist = ee.NewSong.Artist;
                        radio.CurrentSong.Title = ee.NewSong.Title;
                    });
                };
            }
        }

        private bool IsSelected()
        {
            return SelectedElement != null;
        }

        private void Add(Radio r = null)
        {
            AddStationView av = new AddStationView();

            if (r != null)
            {
                AddStationViewModel avm = new AddStationViewModel();
                avm.SetMode(true, (Radio)SelectedElement);
                avm.StationName = SelectedElement.Name;
                avm.StationUrl = SelectedElement.Url;
                av.DataContext = avm;
            }

            av.ShowDialog();

            av.Closed += (ss, ee) =>
            {
                Elements = xml.LoadRadios();
            };
        }

        private void Update()
        {
            Add((Radio)SelectedElement);
        }

        private void Delete()
        {
            MessageBoxResult result = MessageBox.Show($"Czy na pewno chcesz usunąć {SelectedElement.Name}?", "Usuń", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                xml.DeleteRadio((Radio)SelectedElement);
                Elements.Remove(Elements.Where(r => r == SelectedElement).FirstOrDefault());
            }
        }

        private void OnKeyPressed(object sender, KeyPressedArgs e)
        {
            switch (e.KeyPressed)
            {
                case Key.MediaPlayPause:
                    if (play)
                        Stop();
                    else
                        Play();

                    play = !play;
                    break;

                case Key.MediaNextTrack:
                    Next();
                    break;

                case Key.MediaPreviousTrack:
                    Previous();
                    break;
            }
        }
    }
}