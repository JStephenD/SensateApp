using System;
using System.Collections.Generic;
using System.Text;
using Sensate.Views;
using Xamarin.Forms;

namespace Sensate.ViewModels {
	public class SigninViewModel : BaseViewModel {
		public Command SigninCommand { get; }

		public SigninViewModel() {
			SigninCommand = new Command(OnSigninClicked);
		}

		private async void OnSigninClicked(object obj) {
			// Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
			await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
		}
	}
}
