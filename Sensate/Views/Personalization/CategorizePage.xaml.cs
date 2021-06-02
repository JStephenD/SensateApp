using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Sensate.ViewModels;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategorizePage : ContentPage {
		public CategorizePage() {
			InitializeComponent();
		}

		private async void ClickLowVision(object sender, EventArgs e) {
			Preferences.Set("UserCategory", "LowVision", "GeneralSettings");
			await Navigation.PushAsync(new CategoryLowVisionPage());
		}

		private async void ClickColorBlind(object sender, EventArgs e) {
			Preferences.Set("UserCategory", "ColorBlind", "GeneralSettings");
			await Navigation.PushAsync(new CategoryColorBlindPage());
		}

		private async void ClickNormal(object sender, EventArgs e) {
			Preferences.Set("UserCategory", "Normal", "GeneralSettings");
			await Navigation.PushAsync(new CategoryNormalPage());
		}
	}
}