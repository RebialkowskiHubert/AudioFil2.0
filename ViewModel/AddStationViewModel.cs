using AudioFil.Helpers;
using System;
using System.Windows;

namespace AudioFil.ViewModel
{
    public class AddStationViewModel : BindableBase
    {
        private bool edit;
        public bool Edit
        {
            get => edit;
            set => SetProperty(ref edit, value, nameof(Edit));
        }


        private bool add = true;
        public bool Add
        {
            get => add;
            set => SetProperty(ref add, value, nameof(Add));
        }

        private string stationName;
        public string StationName
        {
            get => stationName;
            set => SetProperty(ref stationName, value, nameof(StationName));
        }


        private Uri stationUrl;
        public Uri StationUrl
        {
            get => stationUrl;
            set => SetProperty(ref stationUrl, value, nameof(StationUrl));
        }

        public RelayCommand<Window> SaveCommand { get; set; }
        public RelayCommand<Window> UpdateCommand { get; set; }
        public RelayCommand<Window> CloseCommand { get; set; }

        private Radio oldRadio;


        public AddStationViewModel()
        {
            SaveCommand = new RelayCommand<Window>(Save);
            UpdateCommand = new RelayCommand<Window>(Update);
            CloseCommand = new RelayCommand<Window>(Close);
        }


        public void SetMode(bool mode, Radio old)
        {
            Edit = mode;
            Add = !mode;
            oldRadio = old;
        }

        private void Save(Window w)
        {
            XMLHandling xml = new XMLHandling();
            xml.AddRadio(new Radio(0, StationName, StationUrl));

            MessageBox.Show("Stacja została dodana");
            Close(w);
        }

        private void Update(Window w)
        {
            XMLHandling xml = new XMLHandling();
            xml.UpdateRadio(oldRadio, new Radio(0, StationName, StationUrl));

            MessageBox.Show("Stacja została zmodyfikowana");
            Close(w);
        }

        private void Close(Window w)
        {
            w.Close();
        }
    }
}
