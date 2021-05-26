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

		public class Username
        {
			Username username { get; set; } = new Username();
		}

		public class Sex
        {
			Sex sex { get; set; } = new Sex();
        }

		public class Birthdate
        {
			Birthdate birthdate { get; set; } = new Birthdate();
        }
		


	}
}