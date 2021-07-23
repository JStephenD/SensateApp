using System;
using MediaManager;
using MediaManager.Forms;
using Sensate.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TutorialPage : ContentPage {
		public TutorialPage() {
			InitializeComponent();

			CrossMediaManager.Current.Init();
			//CrossMediaManager.Current.MediaPlayer.VideoView = (MediaManager.Video.IVideoView) tutorial1Video;
			//CrossMediaManager.Current.MediaPlayer.AutoAttachVideoView = false;

			CrossMediaManager.Current.PlayFromAssembly("Sensate.Assets.videos.tutorial_generalobject.mp4",
				typeof(TutorialPage).Assembly);
			CrossMediaManager.Current.Play();

		}

		protected override void OnAppearing() {
			base.OnAppearing();

		}

		public void ToggleTutorial(object s, EventArgs e) { 
			var imagebutton = (ImageButton)s;
			//if (imagebutton.GetType() == typeof(tutorial1Button)) {
			//	Console.WriteLine("1");
			//} else if (imagebutton.GetType() == typeof(tutorial2Button)) {
			//	Console.WriteLine("2");
			//}
		}
	}
}