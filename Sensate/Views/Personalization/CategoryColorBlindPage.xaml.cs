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
		}

		private void Confirm(object sender, EventArgs e) {
			if (CBType.SelectedIndex == -1 ||
				AssistanceLevel.SelectedIndex == -1) {
				DisplayAlert("Error!", "Some Entries are Missing", "Ok");
			} else { 
				Preferences.Set("CBType", CBType.SelectedItem.ToString(), "CBSettings");
				Preferences.Set("AssistanceLevel", AssistanceLevel.SelectedItem.ToString(), "GeneralSettings");

				Navigation.PushAsync(new FeedbackSettingsPage());
			}
		}
	}
}