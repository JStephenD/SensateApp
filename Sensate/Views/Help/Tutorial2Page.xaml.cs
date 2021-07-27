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
	public partial class Tutorial2Page : ContentPage {

		public ObservableCollection<TutorialContentModel> TutorialContent { get; set; } = 
			new ObservableCollection<TutorialContentModel>();

		private SyncHelper.Settings _settings;

		public Tutorial2Page() {
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
				Title = "Interactive Settings", TitleSize = titlesize,
				Subtitle = "Welcome to the tutorial page of the Interactive Settings", SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-is-1.png"),
				ImageWidth = 100,
				Details = "Note: The interactive settings is configured after setting up the profile. Users with low vision are still advised to be guided by a trusted person with normal vision so that the settings are properly configured. For future reconfigurations, users could just visit the settings anytime through the menu bar.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "User categorization.",
				TitleSize = titlesize,
				Subtitle = "The user must choose from the three types of Sensate user such as low vision, colorblind or with normal vision.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-is-2.jpg"),
				ImageWidth = 100,
				Details = "Low vision users are those who suffer from conditions such as farsightedness, age-related blindness, disease-related blindness, blurry visions, and any more. For colorblindness, Sensate only provides assistance on three types such as protanopia, deuteranopia, and tritanopia. Colorblind users are advised to be aware of their conditions before using Sensate. Select the button of your choice and you will be directed to a form specific on your condition, to determine your assistance level.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "Feedback settings.",
				TitleSize = titlesize,
				Subtitle = "The feeback settings include audio feedback and vibration feedback, which is suitable for low vision users.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-is-3.jpg"),
				ImageWidth = 100,
				Details = "After user categorization, the feedback settings will show you the default configurations based on your user type and condition, you have to review it before proceeding to the next settings. You can make changes if you would like on the audio and vibration feedback.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "Navigation settings.",
				TitleSize = titlesize,
				Subtitle = "The navigation settings include shortcuts while using Sensate, which are gesture-based.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-is-4.jpg"),
				ImageWidth = 100,
				Details = "After the feedback settings, the navigation settings will show you the default configurations based on your user type and condition, you have to review it before proceeding to the next settings. You can make changes if you would like on the shortcuts. A shortcut chart is also provided for more details.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "Display settings.",
				TitleSize = titlesize,
				Subtitle = "The display settings include bold text display and text size for users who would like to see bigger font size.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-is-5.jpg"),
				ImageWidth = 100,
				Details = "After the navigation settings, the display settings will show you the default configurations based on your user type and condition, you have to review it before proceeding to the main page. You can make changes if you would like on the bold text and text size settings.",
				DetailsSize = detailssize
			});

			SetCircleFill(0);
		}

		protected override void OnDisappearing() {
			base.OnDisappearing();
			TutorialContent.Clear();
		}

		private async void Back(object s, EventArgs e) { 
			await Shell.Current.GoToAsync($"//{nameof(TutorialPage)}");
		}

		private void carousel_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e) {
			for (int i=0; i<TutorialContent.Count; i++) { 
				if (e.CurrentItem.Equals(TutorialContent[i])) {
					SetCircleFill(i);
				}
			}
		}

		private void SetCircleFill(int index) { 
			circle1.Fill = (index == 0) ? Brush.White : Brush.LightSkyBlue;
			circle2.Fill = (index == 1) ? Brush.White : Brush.LightSkyBlue;
			circle3.Fill = (index == 2) ? Brush.White : Brush.LightSkyBlue;
			circle4.Fill = (index == 3) ? Brush.White : Brush.LightSkyBlue;
			circle5.Fill = (index == 4) ? Brush.White : Brush.LightSkyBlue;
		}
	}
}