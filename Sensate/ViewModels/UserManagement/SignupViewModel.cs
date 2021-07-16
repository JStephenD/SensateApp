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

		public SignupViewModel() { }

		private string email;
		public string Email {
			get { return email; }
			set { email = value; }
		}

		private string password;
		public string Password {
			get { return password; }
			set { password = value; }
		}

		private string confirmpassword;
		public string ConfirmPassword {
			get { return confirmpassword; }
			set { confirmpassword = value; }
		}

		public Command SigninCommand {
			get { return new Command(OnSigninClicked); }
		}

		public Command SignupCommand {
			get { return new Command(OnSignupClicked); }
		}

		private async void OnSigninClicked(object obj) {
			await App.Current.MainPage.Navigation.PushModalAsync(new SigninPage());
		}

		public string FirebaseAPIKey = "AIzaSyCCVSTuyUOF9KVv6fZJywVHckk4PttsUw8";

		private async void OnSignupClicked(object obj) {
			// Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
			//await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
			try {
				//credential checker 

				var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAPIKey));

				if (email != null) {
					if (password != null && confirmpassword != null && password.Length > 5) {
						if (password == confirmpassword) {
							var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
							var content = await auth.GetFreshAuthAsync();
							var serializedcontent = JsonConvert.SerializeObject(content);
							Preferences.Set("UID", auth.User.LocalId);
							Preferences.Set("MyFirebaseRefreshToken", serializedcontent);
							//string gettoken = auth.FirebaseToken;
							await App.Current.MainPage.DisplayAlert("Success!", "Welcome to Sensate! Please remember your credentials for future log-ins.", "OK");
							//redirect to quick profile
							await Shell.Current.GoToAsync(nameof(QuickProfileSetupPage));
						} else {
							await App.Current.MainPage.DisplayAlert("Alert", "Password and confirm password don't not match.", "OK");
						}
					} else {
						await App.Current.MainPage.DisplayAlert("Alert!", "Password must contain atleast 6 characters.", "OK");
					}
				} else {
					await App.Current.MainPage.DisplayAlert("Alert!", "Email must not be empty.", "OK");
				}

			} catch (Exception ex) {
				await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
			}

		}



	}
}

