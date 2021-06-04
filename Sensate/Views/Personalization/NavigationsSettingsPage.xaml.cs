using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Sensate.ViewModels;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NavigationsSettingsPage : ContentPage {
		public NavigationsSettingsPage() {
			InitializeComponent();

			Shortcuts.IsToggled = Preferences.Get("Shortcuts", false, "GeneralSettings");
			Gesture.IsToggled = Preferences.Get("Gesture", false, "GeneralSettings");
			HardwareButtons.IsToggled = Preferences.Get("HardwareButtons", false, "GeneralSettings");

			MoreShortcutsSettings.IsVisible = Shortcuts.IsToggled;
		}

		private void ToggledShortcuts(object sender, ToggledEventArgs e) {
			var istoggled = Shortcuts.IsToggled;

			Preferences.Set("Shortcuts", istoggled, "GeneralSettings");

			if (istoggled) {
				MoreShortcutsSettings.IsVisible = true;
			} else {
				MoreShortcutsSettings.IsVisible = false;
			}
		}

		private void ToggledGesture(object sender, ToggledEventArgs e) {
			Preferences.Set("Gesture", Gesture.IsToggled, "GeneralSettings");
		}

		private void ToggledHardwareButtons(object sender, ToggledEventArgs e) {
			Preferences.Set("HardwareButtons", HardwareButtons.IsToggled, "GeneralSettings");
		}

		private void Next(object sender, EventArgs e) {
			Application.Current.MainPage = new AppShell();
		}
	}
}