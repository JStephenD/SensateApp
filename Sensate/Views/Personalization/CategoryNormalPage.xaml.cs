using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Sensate.ViewModels;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoryNormalPage : ContentPage {
		public CategoryNormalPage() {
			InitializeComponent();
		}

		private async void Confirm(object sender, EventArgs e) {
			if (Purpose.SelectedIndex == -1) {
				await DisplayAlert("Error!", "Some Entries are Missing", "Ok");
			} else {
				Preferences.Set("LVCause", Purpose.SelectedItem.ToString(), "GeneralSettings");
				
				await Shell.Current.GoToAsync(nameof(FeedbackSettingsPage));
			}
		}
	}
}