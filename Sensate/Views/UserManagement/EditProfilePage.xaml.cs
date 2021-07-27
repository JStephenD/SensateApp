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
using Plugin.TextToSpeech;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditProfilePage : ContentPage {

		private SyncHelper.Settings _settings;
		public EditProfilePage() {
			InitializeComponent();

			#region gesturerecognizers
			var backclick = new TapGestureRecognizer();
			backclick.Tapped += BackClick;
			back.GestureRecognizers.Add(backclick);

			var confirmbuttonclick = new TapGestureRecognizer();
			confirmbuttonclick.Tapped += ConfirmButtonClick;
			confirmButtonFrame.GestureRecognizers.Add(confirmbuttonclick);
			confirmButton.GestureRecognizers.Add(confirmbuttonclick);

			var editaccountclick = new TapGestureRecognizer();
			editaccountclick.Tapped += EditAccountClick;
			editAccountFrame.GestureRecognizers.Add(editaccountclick);
			editAccountButton.GestureRecognizers.Add(editaccountclick);
			#endregion gesturerecognizers
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			_settings = SyncHelper.GetCurrentSettings();

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

			await SyncHelper.UploadSettings();
			if (_settings.VibrationFeedback) Vibration.Vibrate();

			//Console.WriteLine(Shell.Current.Items);
			//foreach (var x in Shell.Current.Items) {
			//	if (x.Title == "Account") {
			//		Console.WriteLine(x);
			//		Console.WriteLine(x.Icon);
			//	}
			//}

			Application.Current.MainPage = new AppShell();
			Shell.Current.CurrentItem = Shell.Current.Items.FirstOrDefault(r => r.Title == "Account");
			await Shell.Current.GoToAsync($"//{nameof(AccountPage)}");
		}
		public async void BackClick(object s, EventArgs e) {
			if (_settings.VibrationFeedback) Vibration.Vibrate();
			await Shell.Current.GoToAsync($"//{nameof(AccountPage)}");
		}
		public async void EditAccountClick(object s, EventArgs e) {
			if (_settings.VibrationFeedback) Vibration.Vibrate();
			await Shell.Current.GoToAsync($"{nameof(EditAccountPage)}");
		}
		#endregion gesturerecognizer functions
	}
}