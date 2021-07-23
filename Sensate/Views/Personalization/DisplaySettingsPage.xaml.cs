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
		private Label[] labels;
		private SyncHelper.Settings _settings;
		private bool introDone;
		#endregion variables


		public DisplaySettingsPage() {
			InitializeComponent();

			#region initialize variables
			labels = new Label[] {
				textTitle, textBased, textBold, textNight, textTextSize,
				textSizeSmall, textSizeNormal, textSizeLarge
			};
			_settings = SyncHelper.GetCurrentSettings();

			introDone = Preferences.Get("IntroDone", false);

			TextSize.Value = _settings.TextSize;
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
				var newval = e.Value;
				Preferences.Set("BoldText", newval, "GeneralSettings");
				OnAppearing();
			} catch {
				Console.WriteLine("error 1 here");
			}
		}

		private void ToggledNightMode(object sender, ToggledEventArgs e) {
			var newval = e.Value;
			Preferences.Set("NightMode", newval, "GeneralSettings");
			OnAppearing();
		}

		private void ChangeTextSize(object sender, ValueChangedEventArgs e) {
			try {
				var newval = (int)Math.Round(e.NewValue);
				Preferences.Set("TextSize", newval, "GeneralSettings");
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