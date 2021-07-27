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
		private readonly Label[] labels;
		private SyncHelper.Settings _settings;
		private readonly bool introDone;
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

			// voice gender not supported yer
			voicegenderStack.IsVisible = false;

			_settings = SyncHelper.GetCurrentSettings();

			switch (_settings.TextSize) {
				case 0:
					textTitle.FontSize = 26;
					textInfo1.FontSize = 14;
					textInfo2.FontSize = 14;
					textAudioFeedback.FontSize = 22;
					textVibrationFeedback.FontSize = 22;
					break;
				case 1:
					textTitle.FontSize = 28;
					textInfo1.FontSize = 16;
					textInfo2.FontSize = 16;
					textAudioFeedback.FontSize = 24;
					textVibrationFeedback.FontSize = 24;
					break;
				case 2:
					textTitle.FontSize = 30;
					textInfo1.FontSize = 18;
					textInfo2.FontSize = 18;
					textAudioFeedback.FontSize = 26;
					textVibrationFeedback.FontSize = 26;
					break;
			}

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
				if (_settings.VibrationFeedback) Vibration.Vibrate();
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
				if (_settings.VibrationFeedback) Vibration.Vibrate();

				OnAppearing();
			} catch {
				Console.WriteLine("toggeld vibration error");
			}
		}

		private void VoiceSpeedChange(object s, EventArgs e) {
			Console.WriteLine("voice speed change");

			var newval = ((Picker)s).SelectedIndex;
			Preferences.Set("VoiceSpeed", newval, "GeneralSettings");
			if (_settings.VibrationFeedback) Vibration.Vibrate();

			OnAppearing();
		}

		private async void Next(object sender, EventArgs e) {
			if (_settings.VibrationFeedback) Vibration.Vibrate();

			Console.WriteLine("hello world");
			await SyncHelper.UploadSettings();

			await Shell.Current.GoToAsync(nameof(NavigationsSettingsPage));
		}

		private async void Confirm(object sender, EventArgs e) {
			try {
				if (_settings.VibrationFeedback) Vibration.Vibrate();
				await SyncHelper.UploadSettings();
				Console.WriteLine("Confirm button");
				await Shell.Current.GoToAsync($"//{nameof(MainSettingsPage)}");
			} catch {

			}
		}
	}
}