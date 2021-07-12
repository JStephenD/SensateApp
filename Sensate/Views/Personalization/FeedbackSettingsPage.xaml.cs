using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Sensate.ViewModels;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FeedbackSettingsPage : ContentPage {

		#region variables
		private Label[] labels;
		private bool isBold, isNight, isVibration, isAudio, isShortcut, isGesture, isHardware;
		#endregion variables

		public FeedbackSettingsPage() {
			InitializeComponent();

			#region initalize variables
			labels = new Label[] {
				textTitle, textInfo1, textInfo2, textAudioFeedback, textVibrationFeedback
			};
			isVibration = Preferences.Get("VibrationFeedback", false, "GeneralSettings");
			isAudio = Preferences.Get("AudioFeedback", false, "GeneralSettings");

			isBold = Preferences.Get("BoldText", false, "GeneralSettings");
			isNight = Preferences.Get("NightMode", false, "GeneralSettings");

			isShortcut = Preferences.Get("Shortcuts", false, "GeneralSettings");
			isGesture = Preferences.Get("Gesture", false, "GeneralSettings");
			isHardware = Preferences.Get("HardwareButtons", false, "GeneralSettings");
			#endregion initalize variables
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			foreach (var lab in labels)
				if (isBold)
					lab.FontAttributes = FontAttributes.Bold;
				else
					lab.FontAttributes = FontAttributes.None;

			AudioFeedback.IsToggled = Preferences.Get("AudioFeedback", false, "GeneralSettings");
			VibrationFeedback.IsToggled = Preferences.Get("VibrationFeedback", false, "GeneralSettings");
			MoreAudioSettings.IsVisible = AudioFeedback.IsToggled;
		}

		private async void ToggledAudio(object sender, ToggledEventArgs e) {
			var istoggled = AudioFeedback.IsToggled;

			Preferences.Set("AudioFeedback", istoggled, "GeneralSettings");
			
			if (istoggled) { 
				MoreAudioSettings.IsVisible = true;
			} else {
				MoreAudioSettings.IsVisible = false;
			}

			OnAppearing();
			await SyncHelper.UploadSettings();
		}

		private void ToggledVibration(object sender, ToggledEventArgs e) {
			Preferences.Set("VibrationFeedback", VibrationFeedback.IsToggled, "GeneralSettings");
		}

		private void Next(object sender, EventArgs e) {
			Navigation.PushAsync(new NavigationsSettingsPage());
		}
	}
}