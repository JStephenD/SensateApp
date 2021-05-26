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
using SQLite;
using Firebase.Auth;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SigninPage : ContentPage {

		public SigninPage() {
			InitializeComponent();

			this.BindingContext = new SigninViewModel();
		}

		async private void Signup_Clicked(object sender, EventArgs e)
        {
			await Navigation.PushModalAsync(new SignupPage());
		}
		
		public string FirebaseAPIKey = "AIzaSyCzPHs2jYwNCC_IQRc9j7qOCQUMue_fB0o";
		async private void Signin_Clicked(object sender, EventArgs e)
		{
			
			var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAPIKey));
			try
			{ 
				var auth = await authProvider.SignInWithEmailAndPasswordAsync(SignInEmail.Text, SignInPassword.Text);
				var content = await auth.GetFreshAuthAsync();
				var serializedcontent = JsonConvert.SerializeObject(content);
				Preferences.Set("MyFirebaseRefreshToken", serializedcontent);
				await this.DisplayAlert("Welcome back!", "You are logged in again.", "OK");
				await Navigation.PushModalAsync(new AppShell());
	
			}
			catch (Exception ex){
				await this.DisplayAlert("Alert!", "Invalid Email/Password.", "Ok");
			}
		}
	}
}