using System;
using System.Collections.Generic;
using System.Text;
using Sensate.Views;
using Xamarin.Forms;
using Firebase.Auth;
using System.ComponentModel;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace Sensate.ViewModels {
	public class SigninViewModel : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;
		public SigninViewModel() { }

		private string email;  
        public string Email {  
            get { return email; }  
            set {  
                email = value;  
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));}  
        }

		private string password;  
        public string Password {  
            get { return password; }  
            set {  
                password = value;  
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));}  
        }
		
		public Command SigninCommand
        {
			get { return new Command(OnSigninClicked); }
        }

		public Command SignupCommand
		{
			get { return new Command(OnSignupClicked); }
		}

		public string FirebaseAPIKey = "AIzaSyCCVSTuyUOF9KVv6fZJywVHckk4PttsUw8";

		private async void OnSigninClicked(object obj) {
			// Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
			//await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
			var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAPIKey));
			try
			{ 
				var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
				var content = await auth.GetFreshAuthAsync();
				var serializedcontent = JsonConvert.SerializeObject(content);
				Preferences.Set("MyFirebaseRefreshToken", serializedcontent);
				await App.Current.MainPage.DisplayAlert("Welcome back!", "You are logged in again.", "OK");
				//enters the homepage
				//await App.Current.MainPage.Navigation.PushModalAsync(new AppShell());
				Application.Current.MainPage = new AppShell();

			}
			catch (Exception){
				await App.Current.MainPage.DisplayAlert("Alert!", "Invalid Email/Password.", "Ok");
			}
		}

		private async void OnSignupClicked(object obj) {
			await App.Current.MainPage.Navigation.PushModalAsync(new SignupPage());
		}
	}
}
