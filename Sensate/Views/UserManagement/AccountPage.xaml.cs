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
			accountName.Text = Preferences.Get("AccountName", "", "UserAccount");
			#endregion defaults
		}

		protected override void OnAppearing() { 
			base.OnAppearing();
			if (Preferences.Get("UID", "") == "") { 
				logoutFrameText.Text = "Sign in";
			} else { 
				logoutFrameText.Text = "Log out";
			}
			var birthdate = Preferences.Get("AccountBirthdate", DateTime.Now.ToString(), "UserAccount");
			ageText.Text = (DateTime.Now.Year - DateTime.Parse(birthdate).Year).ToString();
			accountName.Text = Preferences.Get("AccountName", "", "UserAccount");
		}

		#region gesturerecognizer functions
		public async void SynchronizeFrameClick(object s, EventArgs e) {
			if (Preferences.Get("UID", "") == "") {
				// not logged in
			} else { 
				await SyncHelper.LoadSettings();
				this.OnAppearing();
			}
		}
		public void EditAccountFrameClick(object s, EventArgs e) {
			Shell.Current.GoToAsync(nameof(EditProfilePage));
		}
		public void LogoutFrameClick(object s, EventArgs e) {
			if (logoutFrameText.Text == "Sign in") { 
				Shell.Current.GoToAsync(nameof(SigninPage));
			} else { 
				Preferences.Set("UID", "");
				this.OnAppearing();
			}
		}
		public void HamburgerClick(object s, EventArgs e) {
			Shell.Current.FlyoutIsPresented = true;
		}
		#endregion gesturerecognizer functions
	}
}