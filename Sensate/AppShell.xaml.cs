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

			Routing.RegisterRoute(nameof(AccountPage), typeof(AccountPage));
			Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
			Routing.RegisterRoute(nameof(EditAccountPage), typeof(EditAccountPage));
			Routing.RegisterRoute(nameof(SigninPage), typeof(SigninPage));
			Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));
			Routing.RegisterRoute(nameof(QuickProfileSetupPage), typeof(QuickProfileSetupPage));
			Routing.RegisterRoute(nameof(DeleteAccountPage), typeof(DeleteAccountPage));

			Routing.RegisterRoute(nameof(CategorizePage), typeof(CategorizePage));
			Routing.RegisterRoute(nameof(CategoryLowVisionPage), typeof(CategoryLowVisionPage));
			Routing.RegisterRoute(nameof(CategoryColorBlindPage), typeof(CategoryColorBlindPage));
			Routing.RegisterRoute(nameof(CategoryNormalPage), typeof(CategoryNormalPage));

			Routing.RegisterRoute(nameof(FAQPage), typeof(FAQPage));
			Routing.RegisterRoute(nameof(RateAndReviewPage), typeof(RateAndReviewPage));
			Routing.RegisterRoute(nameof(SensateAboutPage), typeof(SensateAboutPage));
			Routing.RegisterRoute(nameof(TutorialPage), typeof(TutorialPage));

			Routing.RegisterRoute(nameof(Tutorial1Page), typeof(Tutorial1Page));
			Routing.RegisterRoute(nameof(Tutorial2Page), typeof(Tutorial2Page));
			Routing.RegisterRoute(nameof(Tutorial3Page), typeof(Tutorial3Page));
			Routing.RegisterRoute(nameof(Tutorial4Page), typeof(Tutorial4Page));
			Routing.RegisterRoute(nameof(Tutorial5Page), typeof(Tutorial5Page));

			Routing.RegisterRoute(nameof(CategorizePage), typeof(CategorizePage));
			Routing.RegisterRoute(nameof(CategoryColorBlindPage), typeof(CategoryColorBlindPage));
			Routing.RegisterRoute(nameof(CategoryColorBlindTestPage), typeof(CategoryColorBlindTestPage));
			Routing.RegisterRoute(nameof(CategoryLowVisionPage), typeof(CategoryLowVisionPage));
			Routing.RegisterRoute(nameof(CategoryNormalPage), typeof(CategoryNormalPage));

			Routing.RegisterRoute(nameof(MainSettingsPage), typeof(MainSettingsPage));
			Routing.RegisterRoute(nameof(DisplaySettingsPage), typeof(DisplaySettingsPage));
			Routing.RegisterRoute(nameof(FeedbackSettingsPage), typeof(FeedbackSettingsPage));
			Routing.RegisterRoute(nameof(NavigationsSettingsPage), typeof(NavigationsSettingsPage));

			Routing.RegisterRoute(nameof(IntroPage), typeof(IntroPage));
			Routing.RegisterRoute(nameof(IntroPage2), typeof(IntroPage2));
			Routing.RegisterRoute(nameof(IntroPage3), typeof(IntroPage3));

			Routing.RegisterRoute(nameof(ColorBlindModePage), typeof(ColorBlindModePage));
			Routing.RegisterRoute(nameof(RecognitionModePage), typeof(RecognitionModePage));
			Routing.RegisterRoute(nameof(RecognitionResultPage), typeof(RecognitionResultPage));

			if (Preferences.Get("UserCategory", "Normal", "GeneralSettings") == "LowVision") {
				MainFeature.Route = nameof(RecognitionModePage);
				MainFeature.ContentTemplate = new DataTemplate(typeof(RecognitionModePage));
				MainFeatureTitle.Title = "Recognition Mode";
				MainFeatureTitle.Icon = ImageSource.FromResource("Sensate.Assets.icon_lv.png", typeof(AppShell).Assembly);
			} else {
				MainFeature.Route = nameof(ColorBlindModePage);
				MainFeature.ContentTemplate = new DataTemplate(typeof(ColorBlindModePage));
				MainFeatureTitle.Title = "Colorblind Mode";
				MainFeatureTitle.Icon = ImageSource.FromResource("Sensate.Assets.icon_cb.png", typeof(AppShell).Assembly);
			}
		}

		private async void OnMenuItemClicked(object sender, EventArgs e) {
			Preferences.Set("MyFirebaseRefreshToken", "");
			await Current.GoToAsync($"{nameof(SigninPage)}");
		}

		protected override void OnAppearing() {
			Console.WriteLine("flyout open");
			if (Preferences.Get("UserCategory", "Normal", "GeneralSettings") == "LowVision") {
				MainFeature.Route = nameof(RecognitionModePage);
				MainFeature.ContentTemplate = new DataTemplate(typeof(RecognitionModePage));
				MainFeatureTitle.Title = "Recognition Mode";
				MainFeatureTitle.Icon = ImageSource.FromResource("Sensate.Assets.icon_lv.png", typeof(AppShell).Assembly);
			} else {
				MainFeature.Route = nameof(ColorBlindModePage);
				MainFeature.ContentTemplate = new DataTemplate(typeof(ColorBlindModePage));
				MainFeatureTitle.Title = "Colorblind Mode";
				MainFeatureTitle.Icon = ImageSource.FromResource("Sensate.Assets.icon_cb.png", typeof(AppShell).Assembly);
			}
		}


		public void CloseMenu(object s, EventArgs e) { 
			Shell.Current.FlyoutIsPresented = false;
		}
	}
}
