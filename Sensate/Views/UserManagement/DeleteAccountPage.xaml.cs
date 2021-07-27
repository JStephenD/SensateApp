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
	public partial class DeleteAccountPage : ContentPage {

		private SyncHelper.Settings settings;

		public DeleteAccountPage() {
			InitializeComponent();

			#region gesturerecognizers
			var backclick = new TapGestureRecognizer();
			backclick.Tapped += BackClick;
			back.GestureRecognizers.Add(backclick);

			var confirmbuttonclick = new TapGestureRecognizer();
			confirmbuttonclick.Tapped += ConfirmButtonClick;
			confirmButtonFrame.GestureRecognizers.Add(confirmbuttonclick);
			confirmbutton.Clicked += ConfirmButtonClick;

			var cancelclick = new TapGestureRecognizer();
			cancelclick.Tapped += CancelClick;
			cancelButtonFrame.GestureRecognizers.Add(cancelclick);

			var reasonclick = new TapGestureRecognizer();
			reasonclick.Tapped += ReasonClick;
			reasonFrame.GestureRecognizers.Add(reasonclick);
			#endregion gesturerecognizers

			#region defaults
			
			#endregion defaults
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			settings = SyncHelper.GetCurrentSettings();

			switch (settings.TextSize) {
				case 0: 
					textTitle.FontSize = 26;
					textSubtitle.FontSize = 16;
					textInfo.FontSize = 16;
					break;
				case 1:
					textTitle.FontSize = 28;
					textSubtitle.FontSize = 18;
					textInfo.FontSize = 18;
					break;
				case 2:
					textTitle.FontSize = 30;
					textSubtitle.FontSize = 20;
					textInfo.FontSize = 20;
					break;
			}
		}

		#region gesturerecognizer functions
		public async void ConfirmButtonClick(object s, EventArgs e) {
			if (reasonPicker.SelectedIndex == -1) {
				if (settings.VibrationFeedback) Vibration.Vibrate();
				await DisplayAlert("", "Select a reason", "ok");
				return;
			}

			if (settings.VibrationFeedback) Vibration.Vibrate();
			await DisplayAlert("", "Saved", "ok");
		}
		public async void BackClick(object s, EventArgs e) {
			if (settings.VibrationFeedback) Vibration.Vibrate();
			await Shell.Current.GoToAsync($"//{nameof(AccountPage)}");
		}
		public async void CancelClick(object s, EventArgs e) {
			if (settings.VibrationFeedback) Vibration.Vibrate();
			Console.WriteLine("a");
			await Shell.Current.GoToAsync($"//{nameof(AccountPage)}");
		}
		public void ReasonClick(object s, EventArgs e) {
			reasonPicker.Focus();
		}
		#endregion gesturerecognizer functions
	}
}