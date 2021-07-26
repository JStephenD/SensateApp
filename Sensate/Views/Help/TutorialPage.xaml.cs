using System;
using MediaManager;
using MediaManager.Forms;
using Sensate.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TutorialPage : ContentPage {


		public TutorialPage() {
			InitializeComponent();

			BindingContext = this;

			#region gesturerecognizers
			var tut1click = new TapGestureRecognizer();
			tut1click.Tapped += Tut1Click;
			tutorial1Frame.GestureRecognizers.Add(tut1click);

			var tut2click = new TapGestureRecognizer();
			tut2click.Tapped += Tut2Click;
			tutorial2Frame.GestureRecognizers.Add(tut2click);

			var tut3click = new TapGestureRecognizer();
			tut3click.Tapped += Tut3Click;
			tutorial3Frame.GestureRecognizers.Add(tut3click);

			var tut4click = new TapGestureRecognizer();
			tut4click.Tapped += Tut4Click;
			tutorial4Frame.GestureRecognizers.Add(tut4click);

			var tut5click = new TapGestureRecognizer();
			tut5click.Tapped += Tut5Click;
			tutorial5Frame.GestureRecognizers.Add(tut5click);
			#endregion gesturerecognizers
		}

		protected override void OnAppearing() {
			base.OnAppearing();

		}

		protected override void OnDisappearing() {
			base.OnDisappearing();

		}

		public async void Tut1Click(object s, EventArgs e)  { 
			await Shell.Current.GoToAsync(nameof(Tutorial1Page));
		}
		public async void Tut2Click(object s, EventArgs e) {
			await Shell.Current.GoToAsync(nameof(Tutorial2Page));
		}
		public async void Tut3Click(object s, EventArgs e) {
			await Shell.Current.GoToAsync(nameof(Tutorial3Page));
		}
		public async void Tut4Click(object s, EventArgs e) {
			await Shell.Current.GoToAsync(nameof(Tutorial4Page));
		}
		public async void Tut5Click(object s, EventArgs e) {
			await Shell.Current.GoToAsync(nameof(Tutorial5Page));
		}
	}
}