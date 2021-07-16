using System;
using System.Windows.Input;
using Xamarin.Forms;

using Sensate.Views;
using Xamarin.Essentials;

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
					Preferences.Set("Shortcuts", false, "GeneralSettings");
					Preferences.Set("Gesture", false, "GeneralSettings");
					Preferences.Set("HardwareButtons", false, "GeneralSettings");
					Shell.Current.GoToAsync(nameof(IntroPage2));
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