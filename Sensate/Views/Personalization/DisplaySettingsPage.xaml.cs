using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Sensate.ViewModels;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DisplaySettingsPage : ContentPage {

		#region variables
		private Label[] labels;
		private bool isBold, isNight, isVibration, isAudio, isShortcut, isGesture, isHardware;
		#endregion variables


		public DisplaySettingsPage() {
			InitializeComponent();

			#region initialize variables
			labels = new Label[] { 
				textTitle, textBased, textBold, textContrast, textNight, textTextSize
			};
			isVibration = Preferences.Get("VibrationFeedback", false, "GeneralSettings");
			isAudio = Preferences.Get("AudioFeedback", false, "GeneralSettings");

			isBold = Preferences.Get("BoldText", false, "GeneralSettings");
			isNight = Preferences.Get("NightMode", false, "GeneralSettings");

			isShortcut = Preferences.Get("Shortcuts", false, "GeneralSettings");
			isGesture = Preferences.Get("Gesture", false, "GeneralSettings");
			isHardware = Preferences.Get("HardwareButtons", false, "GeneralSettings");
			#endregion initialize variables

		}

		protected override void OnAppearing() {
			base.OnAppearing();

			foreach (var lab in labels)
				if (isBold)
					lab.FontAttributes = FontAttributes.Bold;
				else
					lab.FontAttributes = FontAttributes.None;

			BoldText.IsToggled = isBold;
			NightMode.IsToggled = isNight;
		}

		private async void ToggledBoldText(object sender, ToggledEventArgs e) {
			isBold = e.Value;
			Preferences.Set("BoldText", isBold, "GeneralSettings");
			OnAppearing();
			await SyncHelper.UploadSettings();
		}

		private async void ToggledNightMode(object sender, ToggledEventArgs e) {
			isNight = e.Value;
			Preferences.Set("BoldText", isNight, "GeneralSettings");
			OnAppearing();
			await SyncHelper.UploadSettings();
		}

		private void ChangeContrastIntensity(object sender, ValueChangedEventArgs e) {

		}

		private void ChangeTextSize(object sender, ValueChangedEventArgs e) {

		}

		private void Next(object sender, EventArgs e) {
			Navigation.PushAsync(new NavigationsSettingsPage());
		}
	}
}