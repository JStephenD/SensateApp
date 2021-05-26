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
			if (!string.IsNullOrEmpty(Preferences.Get("MyFirebaseRefreshToken", ""))) {
				MainPage = new AppShell();

			} else {
				MainPage = new NavigationPage(new SigninPage());

			}
			MainPage = new AppShell();
		}

		protected override void OnStart() {
		}

		protected override void OnSleep() {
		}

		protected override void OnResume() {
		}
	}
}
