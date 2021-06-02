using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Sensate.ViewModels;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FeedbackSettingsPage : ContentPage {
		public FeedbackSettingsPage() {
			InitializeComponent();

			AudioFeedback.IsToggled = Preferences.Get("AudioFeedback", false, "GeneralSettings");
			VibrationFeedback.IsToggled = Preferences.Get("VibrationFeedback", false, "GeneralSettings");

			MoreAudioSettings.IsVisible = AudioFeedback.IsToggled;
		}

		private void ToggledAudio(object sender, ToggledEventArgs e) {
			var istoggled = AudioFeedback.IsToggled;

			Preferences.Set("AudioFeedback", istoggled, "GeneralSettings");
			
			if (istoggled) { 
				MoreAudioSettings.IsVisible = true;
			} else {
				MoreAudioSettings.IsVisible = false;
			}
		}

		private void ToggledVibration(object sender, ToggledEventArgs e) {
			Preferences.Set("VibrationFeedback", VibrationFeedback.IsToggled, "GeneralSettings");
		}

		private void Next(object sender, EventArgs e) {
			Navigation.PushAsync(new NavigationsSettingsPage());
		}
	}
}