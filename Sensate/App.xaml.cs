using System;
using Sensate.Services;
using Sensate.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sensate {
	public partial class App : Application {

		public App() {
			InitializeComponent();

			DependencyService.Register<MockDataStore>();
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
