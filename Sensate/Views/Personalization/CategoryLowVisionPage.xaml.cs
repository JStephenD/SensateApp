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

		private void Confirm(object sender, EventArgs e) {
			if (LVCause.SelectedIndex == -1 ||
				LVSeverity.SelectedIndex == -1 || 
				AssistanceLevel.SelectedIndex == -1) {
				DisplayAlert("Error!", "Some Entries are Missing", "Ok");
			} else {
				Preferences.Set("LVCause", LVCause.SelectedItem.ToString(), "LVSettings");
				Preferences.Set("LVSeverity", LVSeverity.SelectedItem.ToString(), "LVSettings");
				Preferences.Set("AssistanceLevel", AssistanceLevel.SelectedItem.ToString(), "GeneralSettings");
		
				Navigation.PushAsync(new FeedbackSettingsPage());
			}
		}
	}
}