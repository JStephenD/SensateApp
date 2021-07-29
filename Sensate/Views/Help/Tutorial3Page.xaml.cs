using System;
using MediaManager;
using MediaManager.Forms;
using Sensate.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Tutorial3Page : ContentPage {

		public ObservableCollection<TutorialContentModel> TutorialContent { get; set; } = 
			new ObservableCollection<TutorialContentModel>();

		private SyncHelper.Settings _settings;
		private float speakRate;
		private CancelMe cancelme;

		public Tutorial3Page() {
			InitializeComponent();

			Shell.SetNavBarIsVisible(this, false);
			BindingContext = this;

			#region gesturerecognizers
			var backclick = new TapGestureRecognizer();
			backclick.Tapped += Back;
			backImage.GestureRecognizers.Add(backclick);
			#endregion gesturerecognizers
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			_settings = SyncHelper.GetCurrentSettings();
			cancelme = new CancelMe();

			speakRate = (_settings.VoiceSpeed == 0) ? .7f :
						(_settings.VoiceSpeed == 1) ? 1f :
													1.3f;

			var titlesize = (_settings.TextSize == 0) ? 20 :
							(_settings.TextSize == 1) ? 22 :
							24;

			var subtitlesize = (_settings.TextSize == 0) ? 18 :
							(_settings.TextSize == 1) ? 20 :
							22;

			var detailssize = (_settings.TextSize == 0) ? 16 :
							(_settings.TextSize == 1) ? 18 :
							20;

			TutorialContent.Add(new TutorialContentModel{ 
				Title = "Recognition Mode", TitleSize = titlesize,
				Subtitle = "Welcome to the tutorial page of the Recognition Mode", SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-rm-1.png"),
				ImageWidth = 100,
				Details = "Note: Your Mobile phone camera must have at least 5 megapixels resolution. Listen Well to the audio feedback of Sensate during this mode. It is recommended to capture an object very near to you so that it will be recognized better. Also, make sure to have a stable internet connection.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "Switching modes.",
				TitleSize = titlesize,
				Subtitle = "You can switch on different detection modes such as objects, texts, faces and brands. Use the shortcuts to switch on different modes.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-rm-2.png"),
				ImageWidth = 100,
				Details = "Switch on object detection by ____. The text detection is activated through ____. Face detection can be done by ___. Finally, you can switch to brand detection by __",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "Capturing an object.",
				TitleSize = titlesize,
				Subtitle = "The camera interface of the recognition mode provides a capture button to capture scenes.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-rm-3.jpg"),
				ImageWidth = 100,
				Details = "The capture button (in circular shape) on the lower middle area is used for capturing the object. Focus on the object you want to be recognized and click the capture button. Wait for a second and an audio will then feedback you what is the object.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel{ 
				Title = "Other functionalities to use.", 
				TitleSize = titlesize - 2,
				Subtitle = "The interface provides more buttons and a slider to personalize the capture.", 
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-rm-4.png"),
				ImageWidth = 100,
				Details = "Near the capture button is the zoom slider in which you can use to zoom in or zoom out the scene on the camera. The flash button (with a lightning icon) on the upper right corner is used for activating the camera flash on and off. Next to the flash button, on its right, is a switch camera button (with a camera switch icon) used for switching between front and back camera.  Left of the flash button is the recognition mode which is represented by icons to display what mode you are in. The menu button can be found on the upper left portion to open the menu bar. Beside it is the 'mode icon' displayed to tell if the user is on recognition or color enhancement mode.",
				DetailsSize = detailssize
			});

			SetCircleFill(0);
		}

		protected override void OnDisappearing() {
			base.OnDisappearing();
			TutorialContent.Clear();
			cancelme.CancelToken();
		}

		private async void Back(object s, EventArgs e) { 
			await Shell.Current.GoToAsync($"//{nameof(TutorialPage)}");
		}

		private async void carousel_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e) {
			for (int i=0; i<TutorialContent.Count; i++) { 
				if (e.CurrentItem.Equals(TutorialContent[i])) {
					SetCircleFill(i);
					await cancelme.Speak($"{TutorialContent[i].Details}", speakRate);
				}
			}
		}

		private void SetCircleFill(int index) { 
			circle1.Fill = (index == 0) ? Brush.White : Brush.LightSkyBlue;
			circle2.Fill = (index == 1) ? Brush.White : Brush.LightSkyBlue;
			circle3.Fill = (index == 2) ? Brush.White : Brush.LightSkyBlue;
			circle4.Fill = (index == 3) ? Brush.White : Brush.LightSkyBlue;
		}
	}
}