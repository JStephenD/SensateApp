using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sensate.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Sensate.Models;
using System.Windows.Input;
using System.IO;
using Firebase.Auth;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditAccountPage : ContentPage {

		private SyncHelper.Settings settings;

		public EditAccountPage() {
			InitializeComponent();

			#region gesturerecognizers
			var backclick = new TapGestureRecognizer();
			backclick.Tapped += BackClick;
			back.GestureRecognizers.Add(backclick);

			var confirmbuttonclick = new TapGestureRecognizer();
			confirmbuttonclick.Tapped += ConfirmButtonClick;
			confirmbuttonFrame.GestureRecognizers.Add(confirmbuttonclick);
			confirmbutton.Clicked += ConfirmButtonClick;

			var deleteaccountclick = new TapGestureRecognizer();
			deleteaccountclick.Tapped += DeleteAccountClick;
			deleteAccount.GestureRecognizers.Add(deleteaccountclick);
			#endregion gesturerecognizers

			#region defaults
			
			#endregion defaults
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			settings = SyncHelper.GetCurrentSettings();
		}

		#region gesturerecognizer functions
		public async void ConfirmButtonClick(object s, EventArgs e) {
			if (settings.VibrationFeedback) Vibration.Vibrate();
			await DisplayAlert("","Saved","ok");

			await SyncHelper.UploadSettings();
		}
		public async void BackClick(object s, EventArgs e) {
			if (settings.VibrationFeedback) Vibration.Vibrate();
			await Shell.Current.GoToAsync($"//{nameof(AccountPage)}");
		}
		public void DeleteAccountClick(object s, EventArgs e) {
			if (settings.VibrationFeedback) Vibration.Vibrate();
			Console.WriteLine("a");
		}
		#endregion gesturerecognizer functions
	}
}