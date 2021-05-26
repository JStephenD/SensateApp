using System;
using System.Windows.Input;
using Xamarin.Forms;

using Sensate.Views;

namespace Sensate.ViewModels {
	public class IntroViewModel : BaseViewModel {

        public ICommand SwipeCommand { get; }

		public IntroViewModel() {
			SwipeCommand = new Command<string>(s => OnSwiped(s));
		}

		private void OnSwiped(string direction) {
			Console.WriteLine("swiped left");
			Console.WriteLine(nameof(SignupPage));
			switch (direction) {
				case "left":
					Application.Current.MainPage = new IntroPage2();
					break;
				case "right":
					break;
				case "up":
					break;
				case "down":
					break;
			}
		}
	}
}