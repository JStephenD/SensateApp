using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Reflection;
using System.IO;
using Google.Cloud.Vision.V1;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecognitionResultPage : ContentPage {
		Assembly assembly;
		bool isVibration;
		bool isGesture;
		private SyncHelper.Settings _settings;
		CancelMe cancelme;


		public RecognitionResultPage() {
			InitializeComponent();

			Shell.SetNavBarIsVisible(this, false);
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			#region defaults
			_settings = SyncHelper.GetCurrentSettings();
			cancelme = new CancelMe();

			#endregion defaults
		}

		protected override void OnDisappearing() {
			base.OnDisappearing();
			cancelme.CancelToken();
		}

	}
}