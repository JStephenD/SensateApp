using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Sensate.Views;
using System.Threading.Tasks;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DisplaySettingsPage : ContentPage {

		#region variables
		private readonly Label[] labels;
		private readonly Label[] textsizex;
		private SyncHelper.Settings _settings;
		private readonly bool introDone;
		#endregion variables


		public DisplaySettingsPage() {
			try {
				InitializeComponent();
			} catch { Console.WriteLine("error initialize component display settings"); }

			#region initialize variables
			labels = new Label[] {
				textTitle, textBased, textBold, textNight, textTextSize,
				textSizeSmall, textSizeNormal, textSizeLarge
			};
			textsizex = new Label[] { textSizeSmall, textSizeNormal, textSizeLarge };

			introDone = Preferences.Get("IntroDone", false);

			#endregion initialize variables

			#region gesturerecognizers
			#endregion gesturerecognizers
		}

		#region gesturerecognizers
		#endregion gesturerecognizers

		protected override void OnAppearing() {
			base.OnAppearing();

			try {
			_settings = SyncHelper.GetCurrentSettings();
			} catch { Console.WriteLine("error getting current settings"); }

			try {
				switch (_settings.TextSize) {
					case 0:
						textTitle.FontSize = 26;
						textBased.FontSize = 14;
						textBold.FontSize = 16;
						textNight.FontSize = 16;
						textTextSize.FontSize = 20;
						foreach (var lab in textsizex) lab.FontSize = 20;
						break;
					case 1:
						textTitle.FontSize = 28;
						textBased.FontSize = 16;
						textBold.FontSize = 18;
						textNight.FontSize = 18;
						textTextSize.FontSize = 22;
						foreach (var lab in textsizex) lab.FontSize = 22;
						break;
					case 2:
						textTitle.FontSize = 30;
						textBased.FontSize = 18;
						textBold.FontSize = 20;
						textNight.FontSize = 20;
						textTextSize.FontSize = 24;
						foreach (var lab in textsizex) lab.FontSize = 24;
						break;
				}
			} catch { Console.WriteLine("error text stuff"); }

			TextSize.Value = _settings.TextSize;
			BoldText.IsToggled = Preferences.Get("BoldText", false, "GeneralSettings");
			NightMode.IsToggled = Preferences.Get("NightMode", false, "GeneralSettings");

			if (introDone) {
				confirmFrame.IsVisible = true;
				nextimagebutton.IsVisible = false;
			} else {
				confirmFrame.IsVisible = false;
				nextimagebutton.IsVisible = true;
			}
		}

		private void ToggledBoldText(object sender, ToggledEventArgs e) {
			try {
				if (_settings.VibrationFeedback) Vibration.Vibrate();
				var newval = e.Value;
				Preferences.Set("BoldText", newval, "GeneralSettings");
				OnAppearing();
			} catch {
				Console.WriteLine("error 1 here");
			}
		}

		private void ToggledNightMode(object sender, ToggledEventArgs e) {
			if (_settings.VibrationFeedback) Vibration.Vibrate();
			var newval = e.Value;
			Preferences.Set("NightMode", newval, "GeneralSettings");
			OnAppearing();
		}

		private void ChangeTextSize(object sender, ValueChangedEventArgs e) {
			try {
				var newval = (int) Math.Round(e.NewValue);
				Preferences.Set("TextSize", newval, "GeneralSettings");
				switch (newval) {
					case 0:
						textTitle.FontSize = 26;
						textBased.FontSize = 14;
						textBold.FontSize = 16;
						textNight.FontSize = 16;
						textTextSize.FontSize = 20;
						foreach (var lab in textsizex) lab.FontSize = 20;
						break;
					case 1:
						textTitle.FontSize = 28;
						textBased.FontSize = 16;
						textBold.FontSize = 18;
						textNight.FontSize = 18;
						textTextSize.FontSize = 22;
						foreach (var lab in textsizex) lab.FontSize = 22;
						break;
					case 2:
						textTitle.FontSize = 30;
						textBased.FontSize = 18;
						textBold.FontSize = 20;
						textNight.FontSize = 20;
						textTextSize.FontSize = 24;
						foreach (var lab in textsizex) lab.FontSize = 24;
						break;
				}
			} catch {
				Console.WriteLine("error here");
			}
		}

		private async void Next(object sender, EventArgs e) {
			if (_settings.VibrationFeedback) Vibration.Vibrate();
			Preferences.Set("IntroDone", true);
			await SyncHelper.UploadSettings();
			Application.Current.MainPage = new AppShell();
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