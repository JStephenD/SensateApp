using System;
using System.Windows.Input;
using Xamarin.Forms;

using Sensate.Views;

namespace Sensate.ViewModels {
	public class Intro2ViewModel : BaseViewModel {

        public ICommand SwipeCommand { get; }

		public Intro2ViewModel() {
			SwipeCommand = new Command<string>(s => OnSwiped(s));
		}

		private void OnSwiped(string direction) {
			switch (direction) {
				case "left":
					Application.Current.MainPage = new IntroPage3();
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