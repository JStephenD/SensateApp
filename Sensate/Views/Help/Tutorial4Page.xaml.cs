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
	public partial class Tutorial4Page : ContentPage {

		public ObservableCollection<TutorialContentModel> TutorialContent { get; set; } = 
			new ObservableCollection<TutorialContentModel>();

		private SyncHelper.Settings _settings;
		private float speakRate;
		private CancelMe cancelme;

		public Tutorial4Page() {
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
				Title = "Color Enhancement", TitleSize = titlesize,
				Subtitle = "Welcome to the tutorial page of the Color Enhancement", SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-ce-1.png"),
				ImageWidth = 100,
				Details = "Note: Your mobile phone camera must have at least 5 megapixels resolution. The filters will help you discern colors, therefore, you must be aware of your condition so that we will know what filter is the best for you.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "Filter on camera.",
				TitleSize = titlesize,
				Subtitle = "The user must pan the camera around to the objects or scenery of interest.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-ce-1.png"),
				ImageWidth = 100,
				Details = "Clicking the color enhancement toggle button switches the color enhancement filter on and off. To change the filter, just go the settings and reconfigure your colorblindness type. ",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "Filter on image-upload.",
				TitleSize = titlesize,
				Subtitle = "The user must click the image-upload button and upload a picture of interest.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-ce-1.png"),
				ImageWidth = 100,
				Details = "Clicking the color enhancement toggle button switches the color enhancement filter on and off.",
				DetailsSize = detailssize
			});

			//TutorialContent.Add(new TutorialContentModel {
			//	Title = "Other functionalities.",
			//	TitleSize = titlesize,
			//	Subtitle = "Welcome to the tutorial page of the Recognition Mode",
			//	SubtitleSize = subtitlesize,
			//	Image = ImageSource.FromResource("Sensate.Assets.For_Blue_Background.png"),
			//	ImageWidth = 100,
			//	Details = "Note: Your Mobile phone camera must have at least 5 megapixels resolution. Listen Well to the audio feedback of Sensate during this mode. It is recommended to capture an object very near to you so that it will be recognized better. Also, make sure to have a stable internet connection.",
			//	DetailsSize = detailssize
			//});

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
					if (_settings.AudioFeedback)
						await cancelme.Speak($"{TutorialContent[i].Details}", speakRate);
				}
			}
		}

		private void SetCircleFill(int index) { 
			circle1.Fill = (index == 0) ? Brush.White : Brush.LightSkyBlue;
			circle2.Fill = (index == 1) ? Brush.White : Brush.LightSkyBlue;
			circle3.Fill = (index == 2) ? Brush.White : Brush.LightSkyBlue;
			//circle4.Fill = (index == 3) ? Brush.White : Brush.LightSkyBlue;
		}
	}
}