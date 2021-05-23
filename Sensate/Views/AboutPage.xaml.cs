using System;
using System.ComponentModel;
using Firebase.Auth;
using Newtonsoft.Json;
using Sensate.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sensate.Views {
	public partial class AboutPage : ContentPage {
		public string FirebaseAPIKey = "AIzaSyCzPHs2jYwNCC_IQRc9j7qOCQUMue_fB0o";
		public AboutPage() {
			InitializeComponent();
			this.BindingContext = new AboutViewModel();
			RefreshToken();
		}
		async private void RefreshToken(){
			var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAPIKey));
			try{
				var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken",""));
				var refreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
				Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
			}catch (Exception ex){
				await this.DisplayAlert("Alert!", "Token Expired.", "Ok");
			}
		}
		private void Logout_Clicked(System.Object sender, System.EventArgs e)
        {
			Preferences.Remove("MyFirebaseRefreshToken");
			App.Current.MainPage = new NavigationPage(new SigninPage());

		}
	}
}