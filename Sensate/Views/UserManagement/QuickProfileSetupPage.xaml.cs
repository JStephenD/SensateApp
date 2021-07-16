using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Sensate.ViewModels;
using Firebase.Database;
using Sensate.Models;
using Firebase.Database.Query;
using Xamarin.Essentials;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QuickProfileSetupPage : ContentPage {

		private int age;

		public QuickProfileSetupPage() {
			InitializeComponent();

			accountname.Text = Preferences.Get("AccountName", "", "UserAccount");
			gender.SelectedItem = Preferences.Get("AccountGender", "", "UserAccount");
			birthdate.Date = DateTime.Parse(Preferences.Get("AccountBirthdate", DateTime.Now.ToString(), "UserAccount"));
		}

		private async void ConfirmCommand(object sender, EventArgs e) {
			//-----insert to database-----
			FirebaseClient firebaseClient = new FirebaseClient(@"https://sensatefirebase-a7ef8-default-rtdb.asia-southeast1.firebasedatabase.app/");
			age = DateTime.Now.Year - birthdate.Date.Year;

			await firebaseClient.Child("Users").PostAsync(new Users {
				UID = Preferences.Get("UID", ""),
				Name = accountname.Text,
				Age = age,
				Sex = gender.SelectedItem.ToString(),
				Birthdate = birthdate.Date.ToString()
				}
			);

			Preferences.Set("AccountName", accountname.Text, "UserAccount");
			Preferences.Set("AccountGender", gender.SelectedItem.ToString(), "UserAccount");
			Preferences.Set("AccountBirthdate", birthdate.Date.ToString(), "UserAccount");

			//go to personalization 
			await Shell.Current.GoToAsync(nameof(CategorizePage));
		}
	}
}