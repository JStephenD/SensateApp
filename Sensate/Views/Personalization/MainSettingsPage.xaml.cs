using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Sensate.ViewModels;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainSettingsPage : ContentPage {
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
		}

		#region gesturerecognizer functions
		public void FeedbackFrameClick(object s, EventArgs e) { 
			Shell.Current.GoToAsync(nameof(FeedbackSettingsPage));
		}

		public void NavigationFrameClick(object s, EventArgs e) {
			Shell.Current.GoToAsync(nameof(NavigationsSettingsPage));
		}

		public void DisplayFrameClick(object s, EventArgs e) {
			Shell.Current.GoToAsync(nameof(DisplaySettingsPage));
		}

		public void HamburgerClick(object s, EventArgs e) { 
			Shell.Current.FlyoutIsPresented = true;
		}

		#endregion gesturerecognizer functions
	}
}