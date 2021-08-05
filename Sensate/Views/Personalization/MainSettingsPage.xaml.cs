using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Sensate.ViewModels;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainSettingsPage : ContentPage {

		private Label[] labels;
		private SyncHelper.Settings _settings;

		public MainSettingsPage() {
			InitializeComponent();

			Shell.SetNavBarIsVisible(this, false);

			#region gesturerecognizers
			var feedbackframeclick = new TapGestureRecognizer();
			feedbackframeclick.Tapped += FeedbackFrameClick;
			var navigationframeclick = new TapGestureRecognizer();
			navigationframeclick.Tapped += NavigationFrameClick;
			var displayframeclick = new TapGestureRecognizer();
			displayframeclick.Tapped += DisplayFrameClick;
			FeedbackFrame.GestureRecognizers.Add(feedbackframeclick);
			NavigationFrame.GestureRecognizers.Add(navigationframeclick);
			DisplayFrame.GestureRecognizers.Add(displayframeclick);

			var hamburgerclick = new TapGestureRecognizer();
			hamburgerclick.Tapped += HamburgerClick;
			hamburger.GestureRecognizers.Add(hamburgerclick);
			#endregion gesturerecognizers

			labels = new Label[] { textTitle, textFeedbackOption, textNavigationOption, textDisplayOption };
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			_settings = SyncHelper.GetCurrentSettings();

			switch (_settings.TextSize) {
				case 0:
					textTitle.FontSize = 32;
					textFeedbackOption.FontSize = 30;
					textNavigationOption.FontSize = 30;
					textDisplayOption.FontSize = 30;
					break;
				case 1:
					textTitle.FontSize = 34;
					textFeedbackOption.FontSize = 32;
					textNavigationOption.FontSize = 32;
					textDisplayOption.FontSize = 32;
					break;
				case 2:
					textTitle.FontSize = 36;
					textFeedbackOption.FontSize = 34;
					textNavigationOption.FontSize = 34;
					textDisplayOption.FontSize = 34;
					break;
			}
			foreach (var lab in labels) lab.FontAttributes = (_settings.BoldText == true) ?
														FontAttributes.Bold : FontAttributes.None;
		}

		#region gesturerecognizer functions
		public async void FeedbackFrameClick(object s, EventArgs e) {
			if (_settings.VibrationFeedback) Vibration.Vibrate();
			Console.WriteLine("feedbackclick");
			try {
				await Shell.Current.GoToAsync(nameof(FeedbackSettingsPage)); 
			} catch (Exception myexception) {
				Console.WriteLine(myexception.Message);
			}
		}

		public async void NavigationFrameClick(object s, EventArgs e) {
			if (_settings.VibrationFeedback) Vibration.Vibrate();
			await Shell.Current.GoToAsync(nameof(NavigationsSettingsPage));
		}

		public async void DisplayFrameClick(object s, EventArgs e) {
			if (_settings.VibrationFeedback) Vibration.Vibrate();
			await Shell.Current.GoToAsync(nameof(DisplaySettingsPage));
		}

		public void HamburgerClick(object s, EventArgs e) { 
			Shell.Current.FlyoutIsPresented = true;
		}

		#endregion gesturerecognizer functions
	}
}