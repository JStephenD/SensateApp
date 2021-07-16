using System;
using System.Windows.Input;
using Xamarin.Forms;

using Sensate.Views;
using Xamarin.Essentials;

namespace Sensate.ViewModels {
	public class Intro3ViewModel : BaseViewModel {

        public ICommand SwipeCommand { get; }
        public ICommand SignupCommand { get; }
        public ICommand NotNowCommand { get; }

		public Intro3ViewModel() {
			SwipeCommand = new Command<string>(s => OnSwiped(s));
			SignupCommand = new Command(OnClickSignup);
			NotNowCommand = new Command(OnClickNotNow);
		}

		private async void OnSwiped(string direction) {
			switch (direction) {
				case "left":
					await Shell.Current.GoToAsync(nameof(SignupPage));
					break;
				case "right":
					await Shell.Current.GoToAsync(nameof(IntroPage2));
					break;
				case "up":
					break;
				case "down":
					break;
			}
		}

		private void OnClickSignup() { 
			Shell.Current.GoToAsync(nameof(SignupPage));
		}
		private void OnClickNotNow() {
			Shell.Current.GoToAsync(nameof(CategorizePage));
		}
	}
}