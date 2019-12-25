using AudioFil.Helpers;
using System.Windows;

namespace AudioFil
{
    public class SettingsViewModel : BindableBase
    {
		private Settings selectedSettings;
		public Settings SelectedSettings
		{
			get => selectedSettings;
			set => SetProperty(ref selectedSettings, value, nameof(SelectedSettings));
		}

		public RelayCommand SaveCommand { get; set; }

		private XMLHandling xml;

		public SettingsViewModel()
		{
			SaveCommand = new RelayCommand(Save);

			xml = new XMLHandling();
			SelectedSettings = xml.LoadSettings();
		}

		private void Save()
		{
			xml.SaveSettings(SelectedSettings);
			MessageBox.Show("Zapisano");
		}
	}
}
