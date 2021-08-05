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
using Firebase.Database;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditAccountPage : ContentPage {

		private SyncHelper.Settings settings;

		public static string FirebaseAPIKey = "AIzaSyCCVSTuyUOF9KVv6fZJywVHckk4PttsUw8";
		public static FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAPIKey));

		private static readonly string firestore_url = @"https://sensatefirebase-a7ef8-default-rtdb.asia-southeast1.firebasedatabase.app/";
		private static readonly FirebaseClient firebase = new FirebaseClient(firestore_url);

		private static readonly string UID = Preferences.Get("UID", "");
		private static readonly string FirebaseToken = Preferences.Get("FirebaseToken", "");

		public EditAccountPage() {
			InitializeComponent();

			#region gesturerecognizers
			var backclick = new TapGestureRecognizer();
			backclick.Tapped += BackClick;
			back.GestureRecognizers.Add(backclick);

			var confirmbuttonclick = new TapGestureRecognizer();
			confirmbuttonclick.Tapped += ConfirmButtonClick;
			confirmButtonFrame.GestureRecognizers.Add(confirmbuttonclick);
			confirmbutton.Clicked += ConfirmButtonClick;

			var deleteaccountclick = new TapGestureRecognizer();
			deleteaccountclick.Tapped += DeleteAccountClick;
			deleteAccountFrame.GestureRecognizers.Add(deleteaccountclick);
			deleteAccountButton.GestureRecognizers.Add(deleteaccountclick);
			#endregion gesturerecognizers

			#region defaults
			
			#endregion defaults
		}

		protected override async void OnAppearing() {
			base.OnAppearing();

			settings = SyncHelper.GetCurrentSettings();

			var user = await authProvider.GetUserAsync(FirebaseToken);
			var currentemail = user.Email;
			accountEmail.Text = currentemail;
		}

		#region gesturerecognizer functions
		public async void ConfirmButtonClick(object s, EventArgs e) {

			if (string.IsNullOrEmpty(accountEmail.Text)) {
				await DisplayAlert("Alert", "Email should not be empty", "ok");
				return;
			}
			if (string.IsNullOrEmpty(password.Text)) {
				await DisplayAlert("Alert", "Password should not be empty", "ok");
				return;
			}
			if (string.IsNullOrEmpty(confirmpassword.Text)) {
				await DisplayAlert("Alert", "Confirm password should not be empty", "ok");
				return;
			}
			if (password.Text.Length < 6 || confirmpassword.Text.Length < 6) {
				await DisplayAlert("Alert", "Passwords must at least contain 6 characters", "ok");
				return;
			}
			if (password.Text != confirmpassword.Text) {
				await DisplayAlert("Alert", "Password and confirm password should match", "ok");
				return;
			}

			//await authProvider.ChangeUserEmail(FirebaseToken, accountEmail.Text);
			try {
				await authProvider.ChangeUserPassword(FirebaseToken, password.Text);
			} catch { Console.WriteLine("Could not complete action"); }

			await SyncHelper.UploadSettings();
			if (settings.VibrationFeedback) Vibration.Vibrate();
		}
		public async void BackClick(object s, EventArgs e) {
			if (settings.VibrationFeedback) Vibration.Vibrate();
			await Shell.Current.GoToAsync($"//{nameof(AccountPage)}");
		}
		public async void DeleteAccountClick(object s, EventArgs e) {
			if (settings.VibrationFeedback) Vibration.Vibrate();
			Console.WriteLine("a");
			await Shell.Current.GoToAsync($"{nameof(DeleteAccountPage)}");
		}
		#endregion gesturerecognizer functions
	}
}
