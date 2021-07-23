using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Sensate.ViewModels;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoryLowVisionPage : ContentPage {
		public CategoryLowVisionPage() {
			InitializeComponent();
		}

		private async void Confirm(object sender, EventArgs e) {
			if (LVCause.SelectedIndex == -1 ||
				LVSeverity.SelectedIndex == -1) {
				await DisplayAlert("Error!", "Some Entries are Missing", "Ok");
			} else {
				Preferences.Set("LVCause", LVCause.SelectedItem.ToString(), "LVSettings");
				Preferences.Set("LVSeverity", LVSeverity.SelectedItem.ToString(), "LVSettings");

				if (LVSeverity.SelectedIndex == 0) {
					Preferences.Set("AudioFeedback", true, "GeneralSettings");

					var birthdate = Preferences.Get("AccountBirthdate", DateTime.Now.ToString(), "UserAccount");
					if (DateTime.Now.Year - DateTime.Parse(birthdate).Year >= 40) Preferences.Set("VoiceSpeed", 2, "GeneralSettings");
					else Preferences.Set("VoiceSpeed", 1, "GeneralSettings");

					Preferences.Set("VibrationFeedback", true, "GeneralSettings");
					Preferences.Set("Shortcuts", false, "GeneralSettings");
					Preferences.Set("BoldText", false, "GeneralSettings");
					Preferences.Set("TextSize", 1, "GeneralSettings");
				} else if (LVSeverity.SelectedIndex == 1) {
					Preferences.Set("AudioFeedback", true, "GeneralSettings");

					var birthdate = Preferences.Get("AccountBirthdate", DateTime.Now.ToString(), "UserAccount");
					if (DateTime.Now.Year - DateTime.Parse(birthdate).Year >= 40) Preferences.Set("VoiceSpeed",1, "GeneralSettings");
					else Preferences.Set("VoiceSpeed", 0, "GeneralSettings");

					Preferences.Set("VibrationFeedback", true, "GeneralSettings");
					Preferences.Set("Shortcuts", true, "GeneralSettings");
					Preferences.Set("BoldText", false, "GeneralSettings");
					Preferences.Set("TextSize", 2, "GeneralSettings");
				} else if (LVSeverity.SelectedIndex == 2) {
					Preferences.Set("AudioFeedback", true, "GeneralSettings");

					Preferences.Set("VibrationFeedback", true, "GeneralSettings");
					Preferences.Set("Shortcuts", true, "GeneralSettings");
					Preferences.Set("BoldText", true, "GeneralSettings");
					Preferences.Set("TextSize", 2, "GeneralSettings");
				}

				await Shell.Current.GoToAsync(nameof(FeedbackSettingsPage));
			}
		}
	}
}