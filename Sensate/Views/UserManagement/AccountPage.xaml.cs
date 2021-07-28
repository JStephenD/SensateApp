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
	public partial class AccountPage : ContentPage {

		private SyncHelper.Settings _settings;
		public AccountPage() {
			InitializeComponent();

			#region gesturerecognizers
			var synchronizeframeclick = new TapGestureRecognizer();
			synchronizeframeclick.Tapped += SynchronizeFrameClick;
			var editaccountframeclick = new TapGestureRecognizer();
			editaccountframeclick.Tapped += EditAccountFrameClick;
			var logoutframeclick = new TapGestureRecognizer();
			logoutframeclick.Tapped += LogoutFrameClick;
			synchronizeFrame.GestureRecognizers.Add(synchronizeframeclick);
			editprofileFrame.GestureRecognizers.Add(editaccountframeclick);
			logoutframe.GestureRecognizers.Add(logoutframeclick);

			var hamburgerclick = new TapGestureRecognizer();
			hamburgerclick.Tapped += HamburgerClick;
			hamburger.GestureRecognizers.Add(hamburgerclick);
			#endregion gesturerecognizers

			#region defaults
			#endregion defaults
		}

		protected override void OnAppearing() { 
			base.OnAppearing();

			_settings = SyncHelper.GetCurrentSettings();

			if (Preferences.Get("UID", "") == "") { 
				logoutFrameText.Text = "SIGN IN";
			} else { 
				logoutFrameText.Text = "LOG OUT";
			}
			var birthdate = Preferences.Get("AccountBirthdate", DateTime.Now.ToString(), "UserAccount");
			accountName.Text = Preferences.Get("AccountName", "", "UserAccount");
			ageText.Text = (DateTime.Now.Year - DateTime.Parse(birthdate).Year).ToString();
			accountName.Text = Preferences.Get("AccountName", "", "UserAccount");
			genderIcon.Source = (Preferences.Get("AccountGender", "Others", "UserAccount") == "Male") ?
									ImageSource.FromResource("Sensate.Assets.gender-male.png", typeof(AccountPage).Assembly) :
								(Preferences.Get("AccountGender", "Others", "UserAccount") == "Female") ?
									ImageSource.FromResource("Sensate.Assets.gender-female.png", typeof(AccountPage).Assembly) :

									ImageSource.FromResource("Sensate.Assets.gender-neutral.png", typeof(AccountPage).Assembly);
			categoryIcon.Source = (Preferences.Get("UserCategory", "LowVision", "GeneralSettings") == "LowVision") ? 
									ImageSource.FromResource("Sensate.Assets.eye-censored.png", typeof(AccountPage).Assembly) : 
								  (Preferences.Get("UserCategory", "LowVision", "GeneralSettings") == "ColorBlind") ?
								   ImageSource.FromResource("Sensate.Assets.tri-color.png", typeof(AccountPage).Assembly) :

								   ImageSource.FromResource("Sensate.Assets.eye-glow.png", typeof(AccountPage).Assembly);
		}

		#region gesturerecognizer functions
		public async void SynchronizeFrameClick(object s, EventArgs e) {
			if (Preferences.Get("UID", "") == "") {
				// not logged in
				if (_settings.VibrationFeedback) Vibration.Vibrate();
				
			} else { 
				await SyncHelper.LoadSettings();
				if (_settings.VibrationFeedback) Vibration.Vibrate();
				this.OnAppearing();
			}
		}
		public void EditAccountFrameClick(object s, EventArgs e) {
			if (_settings.VibrationFeedback) Vibration.Vibrate();
			Shell.Current.GoToAsync(nameof(EditProfilePage));
		}
		public void LogoutFrameClick(object s, EventArgs e) {
			if (_settings.VibrationFeedback) Vibration.Vibrate();
			if (logoutFrameText.Text == "SIGN IN") { 
				Shell.Current.GoToAsync(nameof(SigninPage));
			} else { 
				Preferences.Set("UID", "");
				Preferences.Set("FirebaseToken", "");
				this.OnAppearing();
			}
		}
		public void HamburgerClick(object s, EventArgs e) {
			Shell.Current.FlyoutIsPresented = true;
		}
		#endregion gesturerecognizer functions
	}
}