using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Sensate.ViewModels;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainSettingsPage : ContentPage {

		private Label[] labels;

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

			SettingsHelper.ApplyDisplaySettings(labels: labels);
		}

		#region gesturerecognizer functions
		public async void FeedbackFrameClick(object s, EventArgs e) {
			Console.WriteLine("feedbackclick");
			try {
				await Shell.Current.GoToAsync(nameof(FeedbackSettingsPage)); 
			} catch (Exception myexception) {
				Console.WriteLine(myexception.Message);
			}
		}

		public async void NavigationFrameClick(object s, EventArgs e) {
			await Shell.Current.GoToAsync(nameof(NavigationsSettingsPage));
		}

		public async void DisplayFrameClick(object s, EventArgs e) {
			await Shell.Current.GoToAsync(nameof(DisplaySettingsPage));
		}

		public void HamburgerClick(object s, EventArgs e) { 
			Shell.Current.FlyoutIsPresented = true;
		}

		#endregion gesturerecognizer functions
	}
}