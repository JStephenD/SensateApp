using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Sensate.ViewModels;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoryColorBlindTestPage : ContentPage {
		public CategoryColorBlindTestPage() {
			InitializeComponent();

			var enchromaClick = new TapGestureRecognizer();
			enchromaClick.Tapped += EnchromaClick;
			enchromaFrame.GestureRecognizers.Add(enchromaClick);
			imgEnchroma.GestureRecognizers.Add(enchromaClick);

			var eyequeClick = new TapGestureRecognizer();
			eyequeClick.Tapped += EyequeClick;
			eyequeFrame.GestureRecognizers.Add(eyequeClick);
			imgEyeQue.GestureRecognizers.Add(eyequeClick);
		}

		public async void EnchromaClick(object s, EventArgs e) {
			await Browser.OpenAsync("https://enchroma.com/pages/test", BrowserLaunchMode.SystemPreferred);
		}

		public async void EyequeClick(object s, EventArgs e) {
			await Browser.OpenAsync("https://www.eyeque.com/color-blind-test/", BrowserLaunchMode.SystemPreferred);
		}

		public async void Done(object s, EventArgs e) {
			await Shell.Current.GoToAsync($"//{nameof(ColorBlindModePage)}");
		}
	}
}