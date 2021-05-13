using System;
using System.Collections.Generic;
using System.Text;
using Sensate.Views;
using Xamarin.Forms;

namespace Sensate.ViewModels {
	public class SignupViewModel : BaseViewModel {
		public Command SignupCommand { get; }

		public SignupViewModel() {
			SignupCommand = new Command(OnSignupClicked);
		}

		private async void OnSignupClicked(object obj) {
			// Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
			await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
		}
	}
}
