using System;
using Sensate.Services;
using Sensate.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sensate {
	public partial class App : Application {

		public App() {
			InitializeComponent();
			DependencyService.Register<MockDataStore>();

			Preferences.Set("IntroDone", false);

			//if (!string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", ""))) {
			if (Preferences.Get("IntroDone", false)) { // if done intro, proceed to main shell
				MainPage = new AppShell();
			} else {
				MainPage = new IntroPage();
			}
		}

		protected override void OnStart() {
		}

		protected override void OnSleep() {
		}

		protected override void OnResume() {
		}
	}
}
