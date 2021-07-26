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
				Title = "Recognition Mode", TitleSize = titlesize,
				Subtitle = "Welcome to the tutorial page of the Recognition Mode", SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.For_Blue_Background.png"),
				ImageWidth = 100,
				Details = "Note: Your Mobile phone camera must have at least 5 megapixels resolution. Listen Well to the audio feedback of Sensate during this mode. It is recommended to capture an object very near to you so that it will be recognized better. Also, make sure to have a stable internet connection.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "Recognition Mode",
				TitleSize = titlesize,
				Subtitle = "Welcome to the tutorial page of the Recognition Mode",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.For_Blue_Background.png"),
				ImageWidth = 100,
				Details = "Note: Your Mobile phone camera must have at least 5 megapixels resolution. Listen Well to the audio feedback of Sensate during this mode. It is recommended to capture an object very near to you so that it will be recognized better. Also, make sure to have a stable internet connection.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "Recognition Mode",
				TitleSize = titlesize,
				Subtitle = "Welcome to the tutorial page of the Recognition Mode",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.For_Blue_Background.png"),
				ImageWidth = 100,
				Details = "Note: Your Mobile phone camera must have at least 5 megapixels resolution. Listen Well to the audio feedback of Sensate during this mode. It is recommended to capture an object very near to you so that it will be recognized better. Also, make sure to have a stable internet connection.",
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
		}
	}
}