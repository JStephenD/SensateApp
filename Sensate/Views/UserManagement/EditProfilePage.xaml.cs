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
	public partial class EditProfilePage : ContentPage {

		public EditProfilePage() {
			InitializeComponent();

			#region gesturerecognizers
			var backclick = new TapGestureRecognizer();
			backclick.Tapped += BackClick;
			back.GestureRecognizers.Add(backclick);

			var confirmbuttonclick = new TapGestureRecognizer();
			confirmbuttonclick.Tapped += ConfirmButtonClick;
			confirmbuttonFrame.GestureRecognizers.Add(confirmbuttonclick);
			confirmbutton.Clicked += ConfirmButtonClick;

			var editaccountclick = new TapGestureRecognizer();
			editaccountclick.Tapped += EditAccountClick;
			editAccount.GestureRecognizers.Add(editaccountclick);
			#endregion gesturerecognizers
		}

		protected override void OnAppearing() {
			base.OnAppearing();
			#region defaults
			accountName.Text = Preferences.Get("AccountName", "", "UserAccount");
			gender.SelectedItem = Preferences.Get("AccountGender", "", "UserAccount");
			birthdate.Date = DateTime.Parse(Preferences.Get("AccountBirthdate", DateTime.Now.ToString(), "UserAccount"));
			usertype.SelectedItem = Preferences.Get("UserCategory", "", "GeneralSettings");
			#endregion defaults
		}

		#region gesturerecognizer functions
		public async void ConfirmButtonClick(object s, EventArgs e) {
			Preferences.Set("AccountName", accountName.Text, "UserAccount");
			Preferences.Set("AccountGender", gender.SelectedItem.ToString(), "UserAccount");
			Preferences.Set("AccountBirthdate", birthdate.Date.ToString(), "UserAccount");
			Preferences.Set("UserCategory", usertype.SelectedItem.ToString(), "GeneralSettings");

			await DisplayAlert("","Saved","ok");

			await SyncHelper.UploadSettings();

			this.OnAppearing();
		}
		public void BackClick(object s, EventArgs e) {
			Shell.Current.GoToAsync(nameof(AccountPage));
		}
		public void EditAccountClick(object s, EventArgs e) {
			Shell.Current.GoToAsync(nameof(EditAccountPage));
		}
		#endregion gesturerecognizer functions
	}
}