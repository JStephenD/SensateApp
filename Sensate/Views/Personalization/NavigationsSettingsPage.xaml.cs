using System;
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
		private Label[] labels;
		private bool isBold, isNight, isVibration, isAudio, isShortcut, isGesture, isHardware,
			introDone;


		bool GestureEnabled = Preferences.Get("Gesture", false, "GeneralSettings");
		List<Frame> frames;
		Frame currentFrame;
		int currentFrameIndex;
		ArrayList elements;
		#endregion variables


		public NavigationsSettingsPage() {
			InitializeComponent();

			#region initalize variables
			labels = new Label[] {
				textTitle, InfoText1, InfoText2, textShortcuts, textGesture, textHardware, 
			};
			isVibration = Preferences.Get("VibrationFeedback", false, "GeneralSettings");
			isAudio = Preferences.Get("AudioFeedback", false, "GeneralSettings");

			isBold = Preferences.Get("BoldText", false, "GeneralSettings");
			isNight = Preferences.Get("NightMode", false, "GeneralSettings");

			isShortcut = Preferences.Get("Shortcuts", false, "GeneralSettings");
			isGesture = Preferences.Get("Gesture", false, "GeneralSettings");
			isHardware = Preferences.Get("HardwareButtons", false, "GeneralSettings");

			introDone = Preferences.Get("IntroDone", false);
			#endregion initalize variables

			#region gesture
			frames = new List<Frame> { // list gesture-navigable components
				ShortcutsFrame,
				GestureFrame,
				HardwareButtonsFrame,
				NextFrame
			};
			var defaultframe = frames[0];
			currentFrame = defaultframe;
			currentFrameIndex = 0;

			elements = new ArrayList { // list general view components
				//MainWrapper,
				Header,
				ContentWrapper_Scroll,
				ContentWrapper,
				InfoText1,
				InfoText2,
				ShortcutsFrame,
				MoreShortcutsSettings,
				GestureFrame,
				HardwareButtonsFrame,
				NextFrame
			};

			if (GestureEnabled)
				currentFrame.BorderColor = Color.Green;
			#endregion gesture

			// add gesture recognizers for general view components
			#region gesturerecognizers
			var gestureswiperecognizer_l = new SwipeGestureRecognizer() { Direction = SwipeDirection.Left };
			gestureswiperecognizer_l.Swiped += GestureSwipe;
			var gestureswiperecognizer_r = new SwipeGestureRecognizer() { Direction = SwipeDirection.Right };
			gestureswiperecognizer_r.Swiped += GestureSwipe;
			var gestureswiperecognizer_u = new SwipeGestureRecognizer() { Direction = SwipeDirection.Up };
			gestureswiperecognizer_u.Swiped += GestureSwipe;
			var gestureswiperecognizer_d = new SwipeGestureRecognizer() { Direction = SwipeDirection.Down };
			gestureswiperecognizer_d.Swiped += GestureSwipe;
			var gesturetaprecognizer = new TapGestureRecognizer();
			gesturetaprecognizer.Tapped += GestureTap;
			foreach (View element in elements) {
				element.GestureRecognizers.Add(gestureswiperecognizer_l);
				element.GestureRecognizers.Add(gestureswiperecognizer_r);
				element.GestureRecognizers.Add(gestureswiperecognizer_u);
				element.GestureRecognizers.Add(gestureswiperecognizer_d);
				element.GestureRecognizers.Add(gesturetaprecognizer);
			}

			// add tap recognizer for content
			var taphandler = new TapGestureRecognizer();
			taphandler.Tapped += ElementTapHandler;

			ShortcutsFrame.GestureRecognizers.Add(taphandler);
			GestureFrame.GestureRecognizers.Add(taphandler);
			HardwareButtonsFrame.GestureRecognizers.Add(taphandler);
			NextFrame.GestureRecognizers.Add(taphandler);

			#endregion gesturerecognizers


			// set toggled status on start
			Shortcuts.IsToggled = Preferences.Get("Shortcuts", false, "GeneralSettings");
			Gesture.IsToggled = GestureEnabled;
			HardwareButtons.IsToggled = Preferences.Get("HardwareButtons", false, "GeneralSettings");

			MoreShortcutsSettings.IsVisible = Shortcuts.IsToggled;
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			SettingsHelper.ApplyDisplaySettings(labels: labels);

			Shortcuts.IsToggled = isShortcut;
			Gesture.IsToggled = isGesture;
			HardwareButtons.IsToggled = isHardware;

			if (introDone) {
				confirmFrame.IsVisible = true;
				NextFrame.IsVisible = false;
			} else {
				confirmFrame.IsVisible = false;
				NextFrame.IsVisible = true;
			}
		}

		private void ToggledShortcuts() {
			if (GestureEnabled) {
				Shortcuts.IsToggled = !Shortcuts.IsToggled;
			} else {
				Shortcuts.IsToggled = !Shortcuts.IsToggled;
			}
			var istoggled = Shortcuts.IsToggled;

			Preferences.Set("Shortcuts", istoggled, "GeneralSettings");

			if (istoggled) {
				MoreShortcutsSettings.IsVisible = true;
			} else {
				MoreShortcutsSettings.IsVisible = false;
			}
		}

		private void ToggledGesture() {
			// if gesture is enabled, do not allow direct taps to activate functions
			if (GestureEnabled) {
				Gesture.IsToggled = !Gesture.IsToggled;
			} else {
				Gesture.IsToggled = !Gesture.IsToggled;
			}
			Preferences.Set("Gesture", Gesture.IsToggled, "GeneralSettings");
			GestureEnabled = Gesture.IsToggled;
		}

		private void ToggledHardwareButtons() {
			if (GestureEnabled) {

			} else {
				HardwareButtons.IsToggled = ! HardwareButtons.IsToggled;
				Preferences.Set("HardwareButtons", HardwareButtons.IsToggled, "GeneralSettings");
			}
		}

		private async void Next(object s = null, EventArgs e = null) {
			await SyncHelper.UploadSettings();
			if (GestureEnabled) {

			} else {
				await Shell.Current.GoToAsync(nameof(DisplaySettingsPage));
			}
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
		}

		private void GestureSwipe(object sender, SwipedEventArgs e) {
			Console.WriteLine($"swiped {currentFrameIndex} {e.Direction}");
			if (GestureEnabled) {
				if ( (currentFrame == frames[0] && e.Direction == SwipeDirection.Left) || 
					 (currentFrame == frames[frames.Count-1] && e.Direction == SwipeDirection.Right)) {
					// do nothing
				} else if (e.Direction == SwipeDirection.Left) {
					currentFrameIndex--; 
					currentFrame = frames[currentFrameIndex];
					Console.WriteLine($"xx {currentFrameIndex}");
				} else if (e.Direction == SwipeDirection.Right) {
					currentFrameIndex++; 
					currentFrame = frames[currentFrameIndex];
				} else if (e.Direction == SwipeDirection.Up) {

				} else if (e.Direction == SwipeDirection.Down) {
				
				}
				ResetHighlight();
				Speak();
			}
			Console.WriteLine($"after {currentFrameIndex}");
		}

		private void ResetHighlight() { 
			for (int i = 0; i < frames.Count; i++) {
				if (i == currentFrameIndex) {
					frames[i].BorderColor = Color.Green;
				}
				else {
					frames[i].BorderColor = Color.Default;
				}
			}
		}

		private async void Confirm(object sender, EventArgs e) {
			try {
				await SyncHelper.UploadSettings();
				Console.WriteLine("Confirm button");
				await Shell.Current.GoToAsync($"//{nameof(MainSettingsPage)}");
			} catch {

			}
		}
		private void Speak() {
			
		}

		private void GestureTap(object sender, EventArgs e) {
			if (GestureEnabled) { 

			}
		}
	}
}