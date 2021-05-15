using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sensate.Views;
using Xamarin.Forms;

namespace Sensate.ViewModels {
	public class QuickProfileSetupViewModel : BaseViewModel {
		public Command ConfirmCommand { get; }
		public QuickProfileSetupViewModel() {
			ConfirmCommand = new Command(OnConfirmCommand);
		}

		private async void OnConfirmCommand() {
			await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
		}
	}
}