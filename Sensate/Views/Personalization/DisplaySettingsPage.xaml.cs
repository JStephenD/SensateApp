using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Sensate.ViewModels;
using System.Threading.Tasks;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DisplaySettingsPage : ContentPage {

		#region variables
		private Label[] labels;
		private bool isBold, isNight, isVibration, isAudio, isShortcut, isGesture, isHardware,
			introDone;
		private string textSize;
		#endregion variables


		public DisplaySettingsPage() {
			InitializeComponent();

			#region initialize variables
			labels = new Label[] {
				textTitle, textBased, textBold, textContrast, textNight, textTextSize,
				textSizeSmall, textSizeNormal, textSizeLarge
			};
			isVibration = Preferences.Get("VibrationFeedback", false, "GeneralSettings");
			isAudio = Preferences.Get("AudioFeedback", false, "GeneralSettings");

			isBold = Preferences.Get("BoldText", false, "GeneralSettings");
			isNight = Preferences.Get("NightMode", false, "GeneralSettings");
			textSize = Preferences.Get("TextSize", "1", "GeneralSettings");

			isShortcut = Preferences.Get("Shortcuts", false, "GeneralSettings");
			isGesture = Preferences.Get("Gesture", false, "GeneralSettings");
			isHardware = Preferences.Get("HardwareButtons", false, "GeneralSettings");

			introDone = Preferences.Get("IntroDone", false);
			#endregion initialize variables

			#region gesturerecognizers
			var tapslide = new TapGestureRecognizer();
			tapslide.Tapped += SliderDone;
			var dragslide = new DragGestureRecognizer();
			dragslide.DropCompleted += SliderDone;
			TextSize.GestureRecognizers.Add(tapslide);
			TextSize.GestureRecognizers.Add(dragslide);
			textsizeFrame.GestureRecognizers.Add(tapslide);
			textsizeFrame.GestureRecognizers.Add(dragslide);
			#endregion gesturerecognizers
		}

		#region gesturerecognizers
		public void SliderDone(object s, EventArgs e) { 
			var val = TextSize.Value;
			TextSize.Value = Math.Round(val);
			Console.WriteLine("sliderdone");
		}
		#endregion gesturerecognizers

		protected override void OnAppearing() {
			base.OnAppearing();

			SettingsHelper.ApplyDisplaySettings(labels: labels);

			BoldText.IsToggled = isBold;
			NightMode.IsToggled = isNight;

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
				isBold = e.Value;
				Preferences.Set("BoldText", isBold, "GeneralSettings");
				OnAppearing();
			} catch {
				Console.WriteLine("error 1 here");
			}
		}

		private void ToggledNightMode(object sender, ToggledEventArgs e) {
			isNight = e.Value;
			Preferences.Set("NightMode", isNight, "GeneralSettings");
			OnAppearing();
		}

		private void ChangeContrastIntensity(object sender, ValueChangedEventArgs e) {

		}

		private void ChangeTextSize(object sender, ValueChangedEventArgs e) {
			try {
				textSize = Math.Round(e.NewValue).ToString();
				Preferences.Set("TextSize", textSize, "GeneralSettings");
				OnAppearing();
			} catch {
				Console.WriteLine("error here");
			}
		}

		private async void Next(object sender, EventArgs e) {
			Preferences.Set("IntroDone", true);
			await SyncHelper.UploadSettings();
			Application.Current.MainPage = new AppShell();
		}

		private async void Confirm(object sender, EventArgs e) {
			try {
				await SyncHelper.UploadSettings();
				Console.WriteLine("Confirm button");
				await Shell.Current.GoToAsync($"//{nameof(MainSettingsPage)}");
			} catch {

			}
		}
	}
}