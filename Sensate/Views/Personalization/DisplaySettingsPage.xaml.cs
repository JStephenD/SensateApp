using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Sensate.ViewModels;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DisplaySettingsPage : ContentPage {
		public DisplaySettingsPage() {
			InitializeComponent();

			//AudioFeedback.IsToggled = Preferences.Get("AudioFeedback", false, "GeneralSettings");
			//VibrationFeedback.IsToggled = Preferences.Get("VibrationFeedback", false, "GeneralSettings");

			//MoreAudioSettings.IsVisible = AudioFeedback.IsToggled;
		}

		private void ToggledBoldText(object sender, ToggledEventArgs e) {

		}

		private void ToggledNightMode(object sender, ToggledEventArgs e) {

		}

		private void ChangeContrastIntensity(object sender, ValueChangedEventArgs e) {

		}

		private void ChangeTextSize(object sender, ValueChangedEventArgs e) {

		}

		private void Next(object sender, EventArgs e) {
			Navigation.PushAsync(new NavigationsSettingsPage());
		}
	}
}