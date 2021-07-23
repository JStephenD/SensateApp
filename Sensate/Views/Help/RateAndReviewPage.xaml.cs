using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Sensate.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RateAndReviewPage : ContentPage {

		private SyncHelper.Settings _settings;
		private Label[] labels;

		private static string FirebaseAPIKey = "AIzaSyCCVSTuyUOF9KVv6fZJywVHckk4PttsUw8";
		private static FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAPIKey));

		private static readonly string firestore_url = @"https://sensatefirebase-a7ef8-default-rtdb.asia-southeast1.firebasedatabase.app/";
		private static readonly FirebaseClient firebase = new FirebaseClient(firestore_url);

		private static readonly string UID = Preferences.Get("UID", "");

		public RateAndReviewPage() {
			InitializeComponent();

			_settings = SyncHelper.GetCurrentSettings();
			labels = new Label[] { textTitle, textDetails, textStarSlider, textComment };
		}

		protected override async void OnAppearing() {
			base.OnAppearing();

			#region display
			if (_settings.TextSize == 0) {
				textTitle.FontSize = 28;
				textDetails.FontSize = 20;
				textStarSlider.FontSize = 18;
				textComment.FontSize = 18;
				commentEntry.FontSize = 18;
				submitButton.FontSize = 22;
			} else if (_settings.TextSize == 1) {
				textTitle.FontSize = 30;
				textDetails.FontSize = 22;
				textStarSlider.FontSize = 20;
				textComment.FontSize = 20;
				commentEntry.FontSize = 20;
				submitButton.FontSize = 24;
			} else {
				textTitle.FontSize = 32;
				textDetails.FontSize = 24;
				textStarSlider.FontSize = 22;
				textComment.FontSize = 22;
				commentEntry.FontSize = 22;
				submitButton.FontSize = 26;
			}

			var fontBold = (_settings.BoldText) ? FontAttributes.Bold : FontAttributes.None;
			foreach (var label in labels) label.FontAttributes = fontBold;
			commentEntry.FontAttributes = fontBold;
			submitButton.FontAttributes = fontBold;

			starSlider.ThumbImageSource = ImageSource.FromResource("Sensate.Assets.star.png", 
												typeof(RateAndReviewPage).Assembly);

			stack.Children.Clear();
			for (int i = 0; i <= 4; i++) {
				stack.Children.Add(new Ellipse {
					WidthRequest = 13,
					HeightRequest = 13,
					Fill = Brush.Yellow,
					Margin = new Thickness(await GetOffsetFor(i), 0, 0, 0),
				});
			}
			#endregion display

		}

		protected override void OnDisappearing() {
			base.OnDisappearing();


		}

		public async void Confirm(object s, EventArgs e) { 
			Vibration.Vibrate();
			var rating = (int) starSlider.Value + 1;
			var comment = commentEntry.Text;
			try {
				await firebase
					  .Child("Reviews")
					  .PostAsync(new ReviewContent { StarRating = rating, Comment = comment, 
										UID = UID});
			} catch { 
				await DisplayAlert("Error", "Unsuccessful due to some errors", "Ok");
			}
		}

		public void SliderValueChange(object s, ValueChangedEventArgs e) { 
			var value = starSlider.Value;
			starSlider.Value = (int) Math.Round(value);
		}

		private async Task<double> GetOffsetFor(int index) {
			
			var w = await Task.Run(WaitWidth);
			var mleft = w / 5 - 13;

			Console.WriteLine(w);
			Console.WriteLine(mleft);

			return (index == 0) ? 0 : mleft;
		}

		private double WaitWidth() { 
			while (true) 
				if (stack.Width != -1) 
					return stack.Width;
		}

		public class ReviewContent { 
			public string UID { get; set; }
			public int StarRating { get; set; } = 5;
			public string Comment { get; set; } = "";
		}
	}
}