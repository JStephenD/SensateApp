using System;
using System.Collections.Generic;
using Sensate.ViewModels;
using Sensate.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sensate {
	public partial class AppShell : Xamarin.Forms.Shell {
		public AppShell() {
			InitializeComponent();

			Routing.RegisterRoute(nameof(SigninPage), typeof(SigninPage));
			Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));
			Routing.RegisterRoute(nameof(QuickProfileSetupPage), typeof(QuickProfileSetupPage));

			Routing.RegisterRoute(nameof(CategorizePage), typeof(CategorizePage));
			Routing.RegisterRoute(nameof(CategoryLowVisionPage), typeof(CategoryLowVisionPage));
			Routing.RegisterRoute(nameof(CategoryColorBlindPage), typeof(CategoryColorBlindPage));
			Routing.RegisterRoute(nameof(CategoryNormalPage), typeof(CategoryNormalPage));

			Routing.RegisterRoute(nameof(FAQPage), typeof(FAQPage));
			Routing.RegisterRoute(nameof(RateAndReviewPage), typeof(RateAndReviewPage));
			Routing.RegisterRoute(nameof(SensateAboutPage), typeof(SensateAboutPage));
			Routing.RegisterRoute(nameof(TutorialPage), typeof(TutorialPage));

			Routing.RegisterRoute(nameof(CategorizePage), typeof(CategorizePage));
			Routing.RegisterRoute(nameof(CategoryColorBlindPage), typeof(CategoryColorBlindPage));
			Routing.RegisterRoute(nameof(CategoryLowVisionPage), typeof(CategoryLowVisionPage));
			Routing.RegisterRoute(nameof(CategoryNormalPage), typeof(CategoryNormalPage));

			Routing.RegisterRoute(nameof(DisplaySettingsPage), typeof(DisplaySettingsPage));
			Routing.RegisterRoute(nameof(FeedbackSettingsPage), typeof(FeedbackSettingsPage));
			Routing.RegisterRoute(nameof(NavigationsSettingsPage), typeof(NavigationsSettingsPage));

			Routing.RegisterRoute(nameof(IntroPage), typeof(IntroPage));
			Routing.RegisterRoute(nameof(IntroPage2), typeof(IntroPage2));
			Routing.RegisterRoute(nameof(IntroPage3), typeof(IntroPage3));

			Routing.RegisterRoute(nameof(ColorBlindModePage), typeof(ColorBlindModePage));


			if (Preferences.Get("UserCategory", "Normal", "GeneralSettings") == "LowVision") {
				MainFeature.Route = nameof(Test1);
				MainFeature.ContentTemplate = new DataTemplate(typeof(Test1));
				MainFeatureTitle.Title = "Low Vision Mode";
			} else {
				MainFeature.Route = nameof(ColorBlindModePage);
				MainFeature.ContentTemplate = new DataTemplate(typeof(ColorBlindModePage));
				MainFeatureTitle.Title = "Color Blind Mode";
			}
		}

		private async void OnMenuItemClicked(object sender, EventArgs e) {
			Preferences.Set("MyFirebaseRefreshToken", "");
			await Current.GoToAsync($"{nameof(SigninPage)}");
		}
	}
}
