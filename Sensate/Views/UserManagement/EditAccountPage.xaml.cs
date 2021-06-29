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
	public partial class EditAccountPage : ContentPage {

		public EditAccountPage() {
			InitializeComponent();

			#region gesturerecognizers
			var backclick = new TapGestureRecognizer();
			backclick.Tapped += BackClick;
			back.GestureRecognizers.Add(backclick);

			var confirmbuttonclick = new TapGestureRecognizer();
			confirmbuttonclick.Tapped += ConfirmButtonClick;
			confirmbuttonFrame.GestureRecognizers.Add(confirmbuttonclick);
			confirmbutton.Clicked += ConfirmButtonClick;

			var deleteaccountclick = new TapGestureRecognizer();
			deleteaccountclick.Tapped += DeleteAccountClick;
			deleteAccount.GestureRecognizers.Add(deleteaccountclick);
			#endregion gesturerecognizers

			#region defaults
			
			#endregion defaults
		}

		#region gesturerecognizer functions
		public void ConfirmButtonClick(object s, EventArgs e) {
			DisplayAlert("","Saved","ok");
		}
		public void BackClick(object s, EventArgs e) {
			Shell.Current.GoToAsync("..");
		}
		public void DeleteAccountClick(object s, EventArgs e) {
			Console.WriteLine("a");
		}
		#endregion gesturerecognizer functions
	}
}