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
				LVSeverity.SelectedIndex == -1 || 
				AssistanceLevel.SelectedIndex == -1) {
				await DisplayAlert("Error!", "Some Entries are Missing", "Ok");
			} else {
				Preferences.Set("LVCause", LVCause.SelectedItem.ToString(), "LVSettings");
				Preferences.Set("LVSeverity", LVSeverity.SelectedItem.ToString(), "LVSettings");
				Preferences.Set("AssistanceLevel", AssistanceLevel.SelectedItem.ToString(), "GeneralSettings");

				await Shell.Current.GoToAsync(nameof(FeedbackSettingsPage));
			}
		}
	}
}