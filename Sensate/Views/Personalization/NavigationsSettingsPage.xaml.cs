﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Sensate.ViewModels;
using System.Collections.Generic;
using System.Collections;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NavigationsSettingsPage : ContentPage {

		#region variables
		private readonly Label[] labels;
		private readonly SyncHelper.Settings _settings;
		private readonly bool introDone;

		#endregion variables


		public NavigationsSettingsPage() {
			InitializeComponent();

			#region initalize variables
			labels = new Label[] {
				textTitle, InfoText1, InfoText2, textShortcuts, textGesture, textHardware, 
			};
			_settings = SyncHelper.GetCurrentSettings();

			introDone = Preferences.Get("IntroDone", false);
			#endregion initalize variables

			// add gesture recognizers for general view components
			var taphandler = new TapGestureRecognizer();
			taphandler.Tapped += ElementTapHandler;

			ShortcutsFrame.GestureRecognizers.Add(taphandler);
			GestureFrame.GestureRecognizers.Add(taphandler);
			HardwareButtonsFrame.GestureRecognizers.Add(taphandler);
			NextFrame.GestureRecognizers.Add(taphandler);
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			SettingsHelper.ApplyDisplaySettings(labels: labels);

			Shortcuts.IsToggled = Preferences.Get("Shortcuts", false, "GeneralSettings");
			Gesture.IsToggled = Preferences.Get("Gesture", false, "GeneralSettings");
			HardwareButtons.IsToggled = Preferences.Get("HardwareButtons", false, "GeneralSettings");

			MoreShortcutsSettings.IsVisible = Shortcuts.IsToggled;

			if (introDone) {
				confirmFrame.IsVisible = true;
				NextFrame.IsVisible = false;
			} else {
				confirmFrame.IsVisible = false;
				NextFrame.IsVisible = true;
			}
		}

		private void ToggledShortcuts() {
			Shortcuts.IsToggled = !Shortcuts.IsToggled;
			var istoggled = Shortcuts.IsToggled;
			Preferences.Set("Shortcuts", istoggled, "GeneralSettings");
			if (istoggled) {
				MoreShortcutsSettings.IsVisible = true;
			} else {
				MoreShortcutsSettings.IsVisible = false;
			}
		}

		private void ToggledGesture() {
			Gesture.IsToggled = !Gesture.IsToggled;
			Preferences.Set("Gesture", Gesture.IsToggled, "GeneralSettings");
		}

		private void ToggledHardwareButtons() {
			HardwareButtons.IsToggled = ! HardwareButtons.IsToggled;
			Preferences.Set("HardwareButtons", HardwareButtons.IsToggled, "GeneralSettings");
		}

		private async void Next(object s = null, EventArgs e = null) {
			await SyncHelper.UploadSettings();
			await Shell.Current.GoToAsync(nameof(DisplaySettingsPage));
		}

		private void ElementTapHandler(object sender, EventArgs e) {
			Console.WriteLine("from tap handler");
			if (sender.Equals(ShortcutsFrame))
				ToggledShortcuts();
			if (sender.Equals(GestureFrame))
				ToggledGesture();
			if (sender.Equals(HardwareButtonsFrame))
				ToggledHardwareButtons();
			if (sender.Equals(NextFrame))
				Next();
			OnAppearing();
		}

		private async void Confirm(object sender, EventArgs e) {
			try {
				await SyncHelper.UploadSettings();
				Console.WriteLine("Confirm button");
				await Shell.Current.GoToAsync($"//{nameof(MainSettingsPage)}");
			} catch {

			}
		}
	}
}