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
				//credential checker 
				string email, pword, conf_pword;
				email = SignUpEmail.Text;
				pword = SignUpPassword.Text;
				conf_pword = SignUpConfirmPassword.Text;
				var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAPIKey));

				if (email != null){
					if(pword != null && conf_pword!= null && pword.Length > 5){
						if (pword == conf_pword){
							var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, pword);
							//string gettoken = auth.FirebaseToken;
							await this.DisplayAlert("Success!", "Welcome to Sensate! Please remember your credentials for future log-ins.", "OK");
							//redirect to quick profile
							await Navigation.PushModalAsync(new AppShell());
						}else{
							await this.DisplayAlert("Alert", "Password and confirm password don't not match.", "OK");
						}
					}else{
						await this.DisplayAlert("Alert!", "Password must contain atleast 6 characters.", "OK");
					}
				}else{
						await this.DisplayAlert("Alert!", "Email must not be empty.", "OK");
				}

			} catch (Exception ex) {
				await this.DisplayAlert("Alert", ex.Message, "OK");
			}

		}
	}
}
