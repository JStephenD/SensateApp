using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Firebase.Auth;
using Newtonsoft.Json;
using Sensate.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sensate.ViewModels {
	public class SignupViewModel : BaseViewModel {

		private SyncHelper.Settings _settings;

		public SignupViewModel() {
			_settings = SyncHelper.GetCurrentSettings();
		}

		private string email;
		public string Email {
			get { return email; }
			set { SetProperty(ref email, value); }
		}

		private string password;
		public string Password {
			get { return password; }
			set { SetProperty(ref password, value); }
		}

		private string confirmpassword;
		public string ConfirmPassword {
			get { return confirmpassword; }
			set { SetProperty(ref confirmpassword, value); }
		}

		public Command SigninCommand {
			get { return new Command(OnSigninClicked); }
		}

		public Command SignupCommand {
			get { return new Command(OnSignupClicked); }
		}

		private async void OnSigninClicked(object obj) {
			if (_settings.VibrationFeedback) Vibration.Vibrate();
			await App.Current.MainPage.Navigation.PushModalAsync(new SigninPage());
		}

		public string FirebaseAPIKey = "AIzaSyCCVSTuyUOF9KVv6fZJywVHckk4PttsUw8";

		private async void OnSignupClicked(object obj) {
			// Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
			//await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");

			//if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmpassword)) {
			//	await App.Current.MainPage.DisplayAlert("Alert", "Incomplete fields", "OK");
			//	return;
			//}

			try {
				//credential checker 

				var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAPIKey));

				if (!string.IsNullOrEmpty(email)) {
					if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirmpassword) && password.Length > 5) {
						if (password == confirmpassword) {
							var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
							var content = await auth.GetFreshAuthAsync();
							var serializedcontent = JsonConvert.SerializeObject(content);
							if (_settings.VibrationFeedback) Vibration.Vibrate();
							Preferences.Set("UID", auth.User.LocalId);
							Preferences.Set("FirebaseToken", auth.FirebaseToken);
							Preferences.Set("MyFirebaseRefreshToken", serializedcontent);
							//string gettoken = auth.FirebaseToken;
							await App.Current.MainPage.DisplayAlert("Success!", "Welcome to Sensate! Please remember your credentials for future log-ins.", "OK");
							//redirect to quick profile
							await SyncHelper.UploadSettings();
							await Shell.Current.GoToAsync(nameof(QuickProfileSetupPage));
						} else {
							if (_settings.VibrationFeedback) Vibration.Vibrate();
							await App.Current.MainPage.DisplayAlert("Alert", "Password and confirm password don't match.", "OK");
						}
					} else {
						if (_settings.VibrationFeedback) Vibration.Vibrate();
						await App.Current.MainPage.DisplayAlert("Alert!", "Password must contain atleast 6 characters.", "OK");
					}
				} else {
					if (_settings.VibrationFeedback) Vibration.Vibrate();
					await App.Current.MainPage.DisplayAlert("Alert!", "Email must not be empty.", "OK");
				}

			} catch (Exception ex) {
				Console.WriteLine(ex);
				if (_settings.VibrationFeedback) Vibration.Vibrate();
				if (ex.Message.Contains("EMAIL_EXISTS")) {
					await App.Current.MainPage.DisplayAlert("Alert", "Email Exists", "OK");
				} else if (ex.Message.Contains("INVALID_EMAIL")) {
					await App.Current.MainPage.DisplayAlert("Alert", "Invalid Email", "OK");
				} else {
					await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
				}
			}

		}

	}
}

