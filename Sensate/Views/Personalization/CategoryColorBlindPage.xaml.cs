using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Sensate.ViewModels;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoryColorBlindPage : ContentPage {
		public CategoryColorBlindPage() {
			InitializeComponent();

			var cbtypeclick = new TapGestureRecognizer();
			cbtypeclick.Tapped += ClickCBTypeArrow;
			CBTypeArrow.GestureRecognizers.Add(cbtypeclick);

			var assistancelevelclick = new TapGestureRecognizer();
			assistancelevelclick.Tapped += ClickCBAsstistanceArrow;
			CBAssistanceArrow.GestureRecognizers.Add(assistancelevelclick);
		}

		public void ClickCBTypeArrow(object s, EventArgs e) {
			CBType.Focus();
		}

		public void ClickCBAsstistanceArrow(object s, EventArgs e) {
			AssistanceLevel.Focus();
		}
		private async void Confirm(object sender, EventArgs e) {
			if (CBType.SelectedIndex == -1 ||
				AssistanceLevel.SelectedIndex == -1) {
				await DisplayAlert("Error!", "Some Entries are Missing", "Ok");
			} else { 
				Preferences.Set("CBType", CBType.SelectedItem.ToString(), "CBSettings");
				Preferences.Set("AssistanceLevel", AssistanceLevel.SelectedItem.ToString(), "GeneralSettings");

				if (AssistanceLevel.SelectedIndex == 0) {
					Preferences.Set("VibrationFeedback", true, "GeneralSettings");
					Preferences.Set("BoldText", false, "GeneralSettings");
					Preferences.Set("Shortcuts", false, "GeneralSettings");
					Preferences.Set("TextSize", 1, "GeneralSettings");
				} else if (AssistanceLevel.SelectedIndex == 1) {
					Preferences.Set("VibrationFeedback", true, "GeneralSettings");
					Preferences.Set("BoldText", true, "GeneralSettings");
					Preferences.Set("Shortcuts", true, "GeneralSettings");
					Preferences.Set("TextSize", 1, "GeneralSettings");
				}

				await Shell.Current.GoToAsync(nameof(FeedbackSettingsPage));
			}
		}
	}
}