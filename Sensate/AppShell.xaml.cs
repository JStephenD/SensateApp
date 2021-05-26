using System;
using System.Collections.Generic;
using Sensate.ViewModels;
using Sensate.Views;
using Xamarin.Forms;

namespace Sensate {
	public partial class AppShell : Xamarin.Forms.Shell {
		public AppShell() {
			InitializeComponent();

			Routing.RegisterRoute(nameof(SigninPage), typeof(SigninPage));
			Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));
			Routing.RegisterRoute(nameof(QuickProfileSetupPage), typeof(QuickProfileSetupPage));

			Routing.RegisterRoute(nameof(FAQPage), typeof(FAQPage));
			Routing.RegisterRoute(nameof(RateAndReviewPage), typeof(RateAndReviewPage));
			Routing.RegisterRoute(nameof(SensateAboutPage), typeof(SensateAboutPage));
			Routing.RegisterRoute(nameof(TutorialPage), typeof(TutorialPage));

			Routing.RegisterRoute(nameof(IntroPage), typeof(IntroPage));	
		}

		private async void OnMenuItemClicked(object sender, EventArgs e) {
			await Current.GoToAsync($"{nameof(SigninPage)}");
		}
	}
}
