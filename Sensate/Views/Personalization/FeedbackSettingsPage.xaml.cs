using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Speech.Synthesis;

using Sensate.ViewModels;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FeedbackSettingsPage : ContentPage {

		#region variables
		private Label[] labels;
		private bool isBold, isNight, isVibration, isAudio, isShortcut, isGesture, isHardware, 
			introDone;
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

			introDone = Preferences.Get("IntroDone", false);
			#endregion initalize variables
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			SettingsHelper.ApplyDisplaySettings(labels: labels);

			AudioFeedback.IsToggled = Preferences.Get("AudioFeedback", false, "GeneralSettings");
			VibrationFeedback.IsToggled = Preferences.Get("VibrationFeedback", false, "GeneralSettings");
			MoreAudioSettings.IsVisible = AudioFeedback.IsToggled;
			if (introDone) { 
				confirmFrame.IsVisible = true;
				nextFrame.IsVisible = false;
			} else {
				confirmFrame.IsVisible = false;
				nextFrame.IsVisible = true;
			}
		}

		private void ToggledAudio(object sender, ToggledEventArgs e) {
			try {
				isAudio = e.Value;
				Preferences.Set("AudioFeedback", isAudio, "GeneralSettings");

				OnAppearing();
			} catch {
				Console.WriteLine("toggled audio error");
			}
		}

		private void ToggledVibration(object sender, ToggledEventArgs e) {
			try {
				isVibration = e.Value;
				Preferences.Set("VibrationFeedback", isVibration, "GeneralSettings");

				OnAppearing();
			} catch {
				Console.WriteLine("toggeld vibration error");
			}
		}

		private async void Next(object sender, EventArgs e) {
			if (isVibration) { 
				Vibration.Vibrate();
			}
			if (isAudio) {
				await TextToSpeech.SpeakAsync("hello");
			}
			Console.WriteLine("hello world");
			await SyncHelper.UploadSettings();

			//Navigation.PushAsync(new NavigationsSettingsPage());
		}

		private async void Confirm(object sender, EventArgs e) {
			try {
				await SyncHelper.UploadSettings();
				Console.WriteLine("Confirm button");
			} catch { 

			}
		}
	}
}