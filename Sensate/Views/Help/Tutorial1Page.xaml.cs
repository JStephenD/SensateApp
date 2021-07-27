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
	public partial class Tutorial1Page : ContentPage {

		public ObservableCollection<TutorialContentModel> TutorialContent { get; set; } = 
			new ObservableCollection<TutorialContentModel>();

		private SyncHelper.Settings _settings;

		public Tutorial1Page() {
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
				Title = "Account Management", TitleSize = titlesize,
				Subtitle = "Welcome to the tutorial page for the Account Management.", SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-am-1.png"),
				ImageWidth = 100,
				Details = "If a user has low vision, it is recommended to be assisted by a friend, family member or a trusted person with normal vision. The account management requires signing up for an account, sign-ins and profile settings, therefore, information must be provided correctly and error-free.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "Sign-up for an account.",
				TitleSize = titlesize,
				Subtitle = "After the loading and welcome page, Sensate will ask you to either sign-up or skip the process. Click the sign-up button. Sensate will now show a sign-up form for you to fill in.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-am-2.png"),
				ImageWidth = 100,
				Details = "Enter the your desired username or email. Password and confirm password should match and must be more than 6 characters. Click the confirm button. An alert will notify you if the sign-up is successful or not.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "Sign-in your account.",
				TitleSize = titlesize,
				Subtitle = "After sign-up or if you already have an existing account, you can sign-in using your registered credentials.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-am-3.png"),
				ImageWidth = 100,
				Details = "On the sign-in form, just enter your registered username or email, and your password. Make sure to remember and protect your credentials.",
				DetailsSize = detailssize
			});
			
			TutorialContent.Add(new TutorialContentModel {
				Title = "Setup your profile.",
				TitleSize = titlesize,
				Subtitle = "After your first sign-up and sign-in, Sensate will display a quick profile setup page.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-am-4.png"),
				ImageWidth = 100,
				Details = "Just enter your name (full name or a nickname), sex, and birthdate. Click the confirm button to confirm your profile entries.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "View your profile.",
				TitleSize = titlesize,
				Subtitle = "While you're on the main interface (recognition mode or color enhancement mode), just click the menu button on the upper left corner and click \"Account\".",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-am-5.png"),
				ImageWidth = 100,
				Details = "You can now view your profile details such as name, sex, age, user type. You can also sycnhronize your account and edit your profile. Pressing the sign-out button signs out your account from the device.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "Edit your profile.",
				TitleSize = titlesize,
				Subtitle = "From the view profile or Account, click the edit profile button.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-am-6.jpg"),
				ImageWidth = 100,
				Details = "An edit profile interface will be displayed. You can now make changes on your profile detials such as name, sex, and birthdate (automatically computes the age). Make sure to click the confirm button to save and update all of your changes.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "Edit your account.",
				TitleSize = titlesize,
				Subtitle = "To edit your account means updating your account credentials. After clicking the edit profile in Account, click the edit account button on the bottom area.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-am-7.jpg"),
				ImageWidth = 100,
				Details = "An edit account interface will be displayed. You can now make changes on your account detials such as username or email, and password. If you want to update your password, always fill up the confirm password. Make sure to click the confirm button to save and update all of your changes.",
				DetailsSize = detailssize
			});

			TutorialContent.Add(new TutorialContentModel {
				Title = "Delete your account.",
				TitleSize = titlesize,
				Subtitle = "To delete your account means deleting or removing your account and all of the information. After clicking the edit account, click the 'delete account' button on the bottom area.",
				SubtitleSize = subtitlesize,
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial-am-8.jpg"),
				ImageWidth = 100,
				Details = "The 'delete account' interface will be displayed. You can select from the list of reasons in deleting the account, to provide the developers information. If you are sure enough to delete your account, click the confirm button. You will be logged out from your account and you can't access it anymore.",
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
			circle6.Fill = (index == 5) ? Brush.White : Brush.LightSkyBlue;
			circle7.Fill = (index == 6) ? Brush.White : Brush.LightSkyBlue;
			circle8.Fill = (index == 7) ? Brush.White : Brush.LightSkyBlue;
		}
	}
}