using System;
using System.Windows.Input;
using Xamarin.Forms;

using Sensate.Views;

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

		private void OnSwiped(string direction) {
			switch (direction) {
				case "left":
					Application.Current.MainPage = new SigninPage();
					break;
				case "right":
					Application.Current.MainPage = new IntroPage2();
					break;
				case "up":
					break;
				case "down":
					break;
			}
		}

		private void OnClickSignup() { 

		}
		private void OnClickNotNow() { 

		}
	}
}