using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sensate.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using Sensate.Models;
using Firebase.Auth;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignupPage : ContentPage {

		public SignupPage() {
			InitializeComponent();

			this.BindingContext = new SignupViewModel();
		}

		public string FirebaseAPIKey = "AIzaSyCzPHs2jYwNCC_IQRc9j7qOCQUMue_fB0o";

		async private void Signin_Clicked(object sender, EventArgs e) {
			await Navigation.PushModalAsync(new SigninPage());
		}
		async private void Signup_Clicked(object sender, EventArgs e) {
			try {
				//credential checker pa kulang
				var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAPIKey));
				if (SignUpPassword.Text == SignUpConfirmPassword.Text) {
					var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(SignUpEmail.Text, SignUpPassword.Text);
					//string gettoken = auth.FirebaseToken;
					await this.DisplayAlert("Success!", "Welcome to Sensate.", "OK");
					await Navigation.PushModalAsync(new AppShell());

				} else {
					await this.DisplayAlert("Alert", "Password does not match.", "Ok");
				}
			} catch (Exception ex) {
				await this.DisplayAlert("Alert", ex.Message, "Ok");
			}

		}
	}
}