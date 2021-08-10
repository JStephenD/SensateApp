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
	public partial class Tutorial5Page : ContentPage {

		public ObservableCollection<TutorialContentModel> TutorialContent { get; set; } = 
			new ObservableCollection<TutorialContentModel>();

		private SyncHelper.Settings _settings;
		private float speakRate;
		private CancelMe cancelme;

		public Tutorial5Page() {
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
				Title = "Shortcuts and Navigation", TitleSize = titlesize,
				Subtitle = "Welcome to the tutorial page of the Shortcuts and Navigation", SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.For_Blue_Background.png"),
				ImageWidth = 100,
				Details = "Note: Make sure that the shortcuts are activated on the interactive settings. If you have severe low vision, please be guided by someone who has a normal vision, so that your confugation settings are updated correctly.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "Switch Mode Shortcuts",
				TitleSize = titlesize,
				Subtitle = "This shortcut is applicable on both of the recognition mode and the color enhancment feature for switching modes.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.For_Blue_Background.png"),
				ImageWidth = 100,
				Details = "Just shake your device if you want to switch into a different mode, for example switching from object detection to text detection, or switching filters on the color enhancement feature. Switching or flipping the camera mode from back to front or vice versa, can be done double tapping the camera interface.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "The Menu Bar",
				TitleSize = titlesize,
				Subtitle = "The menu bar is an interface that contains different options or list to access other functionalities.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.For_Blue_Background.png"),
				ImageWidth = 100,
				Details = "Just swipe from the left side of the screen going to the right to open the menu bar. To close it, just swipe from right to left.",
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
					if (_settings.AudioFeedback)
						await cancelme.Speak($"{TutorialContent[i].Details}", speakRate);
				}
			}
		}

		private void SetCircleFill(int index) { 
			circle1.Fill = (index == 0) ? Brush.White : Brush.LightSkyBlue;
			circle2.Fill = (index == 1) ? Brush.White : Brush.LightSkyBlue;
			circle3.Fill = (index == 2) ? Brush.White : Brush.LightSkyBlue;
		}
	}
}
