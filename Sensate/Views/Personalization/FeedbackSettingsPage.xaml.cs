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
		private SyncHelper.Settings _settings;
		private bool introDone;
		#endregion variables

		public FeedbackSettingsPage() {
			InitializeComponent();

			#region initalize variables
			labels = new Label[] {
				textTitle, textInfo1, textInfo2, textAudioFeedback, textVibrationFeedback
			};
			_settings = SyncHelper.GetCurrentSettings();

			introDone = Preferences.Get("IntroDone", false);

			VoiceSpeed.SelectedIndexChanged += VoiceSpeedChange;
			#endregion initalize variables
		}

		

		protected override void OnAppearing() {
			base.OnAppearing();

			_settings = SyncHelper.GetCurrentSettings();
			SettingsHelper.ApplyDisplaySettings(labels: labels);

			AudioFeedback.IsToggled = Preferences.Get("AudioFeedback", false, "GeneralSettings");
			VibrationFeedback.IsToggled = Preferences.Get("VibrationFeedback", false, "GeneralSettings");

			MoreAudioSettings.IsVisible = AudioFeedback.IsToggled;
			VoiceSpeed.SelectedIndex = Preferences.Get("VoiceSpeed", 1, "GeneralSettings");

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
				var newval = e.Value;
				Preferences.Set("AudioFeedback", newval, "GeneralSettings");

				OnAppearing();
			} catch {
				Console.WriteLine("toggled audio error");
			}
		}

		private void ToggledVibration(object sender, ToggledEventArgs e) {
			try {
				var newval = e.Value;
				Preferences.Set("VibrationFeedback", newval, "GeneralSettings");

				OnAppearing();
			} catch {
				Console.WriteLine("toggeld vibration error");
			}
		}

		private void VoiceSpeedChange(object s, EventArgs e) {
			Console.WriteLine("voice speed change");

			var newval = ((Picker)s).SelectedIndex;
			Preferences.Set("VoiceSpeed", newval, "GeneralSettings");
			OnAppearing();
		}

		private async void Next(object sender, EventArgs e) {
			Console.WriteLine("hello world");
			await SyncHelper.UploadSettings();

			await Shell.Current.GoToAsync(nameof(NavigationsSettingsPage));
		}

		private async void Confirm(object sender, EventArgs e) {
			try {
				await SyncHelper.UploadSettings();
				Console.WriteLine("Confirm button");
				await Shell.Current.GoToAsync($"//{nameof(MainSettingsPage)}");
			} catch {

			}
		}
	}
}