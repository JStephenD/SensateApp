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
	public partial class TutorialPage : ContentPage {

		public ObservableCollection<TutorialContent> Tutorial1Content { get; set; }
			= new ObservableCollection<TutorialContent>();

		public TutorialPage() {
			InitializeComponent();

			BindingContext = this;
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			#region tutorial1
			Tutorial1Content.Add(new TutorialContent { 
				Title = "Recognition Mode", Details = "Go to recognition mode of the application.",
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial1_01", typeof(TutorialPage).Assembly)
			});
			Tutorial1Content.Add(new TutorialContent {
				Title = "General Object Detection",
				Details = "Select General Object Detection Mode.",
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial1_02", typeof(TutorialPage).Assembly)
			});
			Tutorial1Content.Add(new TutorialContent {
				Title = "Point The Camera",
				Details = "Point the camera towards the object you desire to get information from.",
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial1_03", typeof(TutorialPage).Assembly)
			});
			Tutorial1Content.Add(new TutorialContent {
				Title = "Capture Image",
				Details = "Click the shutter button and wait for the response",
				Image = ImageSource.FromResource("Sensate.Assets.tutorial.tutorial1_04", typeof(TutorialPage).Assembly)
			});
			#endregion tutorial1
		}

		protected override void OnDisappearing() {
			base.OnDisappearing();

			Tutorial1Content.Clear();
		}

		public void ToggleTutorial(object s, EventArgs e) { 
			var imagebutton = (ImageButton)s;
			//if (imagebutton.GetType() == typeof(tutorial1Button)) {
			//	Console.WriteLine("1");
			//} else if (imagebutton.GetType() == typeof(tutorial2Button)) {
			//	Console.WriteLine("2");
			//}
		}

		public class TutorialContent{ 
			
			public string Title { get; set; }
			public string Details { get; set; }
			public ImageSource Image { get; set; }
		}
	}
}