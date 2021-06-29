using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using SkiaSharp.Views.Forms;
using Xamarin.CommunityToolkit.Effects;

using Sensate.ViewModels;
using SkiaSharp;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ColorBlindModePage : ContentPage {

		#region variables
		private SKBitmap bitmap;
		private string cbmode, cbmodeorig;
		private bool iscapturemode = true;
		private bool isuploadmode = false;
		private bool isbusy = false;
		private Assembly assembly;

		private static float[] Protanopia = {
			0.567F, 0.433F,         0F, 0F, 0,
			0.558F, 0.442F,         0F, 0F, 0,
					0F, 0.242F, 0.758F, 0F, 0,
					0F,         0F,        0F, 1F, 0,
		};

		private static float[] Deuteranopia = {
			0.625f,0.375f,0,0,0,
			0.7f,0.3f,0,0,0,
			0,0.3f,0.7f,0,0,
			0,0,0,1,0
		};

		private static float[] Tritanopia = {
			0.95f,0.05f,0,0,0,
			0,0.433f,0.567f,0,0,
			0,0.475f,0.525f,0,0,
			0,0,0,1,0
		};

		private SKPaint paintProtanopia = new SKPaint {
			ColorFilter = SKColorFilter.CreateColorMatrix(Protanopia),
			Style = SKPaintStyle.Fill
		};

		private SKPaint paintDeuteranopia = new SKPaint {
			ColorFilter = SKColorFilter.CreateColorMatrix(Deuteranopia),
			Style = SKPaintStyle.Fill
		};
		private SKPaint paintTritanopia = new SKPaint {
			ColorFilter = SKColorFilter.CreateColorMatrix(Tritanopia),
			Style = SKPaintStyle.Fill
		};
		#endregion variables

		public ColorBlindModePage() {
			InitializeComponent();

			Shell.SetNavBarIsVisible(this, false);

			#region gesturerecognizers
			var zoominframeclick = new TapGestureRecognizer();
			var zoomoutframeclick = new TapGestureRecognizer();
			zoominframeclick.Tapped += ZoomInClick;
			zoomoutframeclick.Tapped += ZoomOutClick;
			zoomInFrame.GestureRecognizers.Add(zoominframeclick);
			zoomOutFrame.GestureRecognizers.Add(zoomoutframeclick);

			var opennavbarclick = new TapGestureRecognizer();
			opennavbarclick.Tapped += OpenNavBarClick;
			OpenNavFrame.GestureRecognizers.Add(opennavbarclick);

			var cameraframeclick = new TapGestureRecognizer();
			var uploadframeclick = new TapGestureRecognizer();
			cameraframeclick.Tapped += CameraFrameClick;
			uploadframeclick.Tapped += UploadFrameClick;
			cameraFrame.GestureRecognizers.Add(cameraframeclick);
			uploadFrame.GestureRecognizers.Add(uploadframeclick);

			var rotatecamframeclick = new TapGestureRecognizer();
			rotatecamframeclick.Tapped += RotateCamFrameClick;
			rotatecamFrame.GestureRecognizers.Add(rotatecamframeclick);
			
			var flashframeclick = new TapGestureRecognizer();
			flashframeclick.Tapped += FlashFrameClick;
			flashFrame.GestureRecognizers.Add(flashframeclick);
			#endregion gesturerecognizers

			cameraView.CaptureMode = CameraCaptureMode.Photo;
			cameraView.IsVisible = true;
			canvasView.IsVisible = true;

			togglefilter.IsVisible = false;
			togglefilterFrame.IsVisible = false;

			debugimage.IsVisible = false;

			cbmode = cbmodeorig = Preferences.Get("CBType", "Protanopia", "CBSettings");

			if (Preferences.Get("UserCategory", "Normal", "GeneralSettings") == "Normal") {
				canvasView.IsVisible = false;
			} else {
				StartCaptureMode();
			}
		}

		#region zooming
		private void ZoomSlider_ValueChanged(object sender, ValueChangedEventArgs e) {
			cameraView.Zoom = zoomSlider.Value;
		}

		public void ZoomInClick(object s, EventArgs e) {
			double currval = zoomSlider.Value, maxzoom = cameraView.MaxZoom;
			cameraView.Zoom = Math.Min(currval + 1, maxzoom);
			zoomSlider.Value = cameraView.Zoom;
		}

		public void ZoomOutClick(object s, EventArgs e) {
			double currval = zoomSlider.Value, minzoom = 1;
			cameraView.Zoom = Math.Max(currval - 1, minzoom);
			zoomSlider.Value = cameraView.Zoom;
		}
		#endregion zooming

		#region navigation
		public void OpenNavBarClick(object s, EventArgs e) {
			Shell.Current.FlyoutIsPresented = true;
		}
		public void RotateCamFrameClick(object s, EventArgs e) {
			if (cameraView.CameraOptions == CameraOptions.Back)
				cameraView.CameraOptions = CameraOptions.Front;
			else
				cameraView.CameraOptions = CameraOptions.Back;
		}
		public void FlashFrameClick(object s, EventArgs e) {
			if (cameraView.FlashMode == CameraFlashMode.Off)
				cameraView.FlashMode = CameraFlashMode.Torch;
			else
				cameraView.FlashMode = CameraFlashMode.Off;
		}
		#endregion navigation

		#region imagemode
		public void CameraFrameClick(object s, EventArgs e) {
			cameraFrame.BackgroundColor = Color.FromHex("#FF881A");
			uploadFrame.BackgroundColor = Color.FromHex("#EFEFEF");
			IconTintColorEffect.SetTintColor(cameraImage, Color.White);
			IconTintColorEffect.SetTintColor(uploadImage, Color.FromHex("#00384F"));

			togglefilter.IsVisible = false;
			togglefilterFrame.IsVisible = false;
			Task.Run(StartCaptureMode);
			iscapturemode = true;
			isuploadmode = false;
		}
		public async void UploadFrameClick(object s, EventArgs e) {
			uploadFrame.BackgroundColor = Color.FromHex("#FF881A");
			cameraFrame.BackgroundColor = Color.FromHex("#EFEFEF");
			IconTintColorEffect.SetTintColor(uploadImage, Color.White);
			IconTintColorEffect.SetTintColor(cameraImage, Color.FromHex("#00384F"));

			togglefilter.IsVisible = true;
			togglefilterFrame.IsVisible = true;
			iscapturemode = false;
			isuploadmode = true;

			assembly = this.GetType().GetTypeInfo().Assembly;
			var resources = assembly.GetManifestResourceNames();
			var resourceName = resources.Single(r => r.EndsWith("upload-image-default.png", StringComparison.OrdinalIgnoreCase));
			var stream = assembly.GetManifestResourceStream(resourceName);

			bitmap = SKBitmap.Decode(stream);

			await MediaPicker.PickPhotoAsync()
				.ContinueWith(async t => { 
					if (t.IsCanceled)
						return;
					var result = t.Result;
					MemoryStream ms = new MemoryStream();
					Stream st = await result.OpenReadAsync();
					debugimage.IsVisible = true;
					debugimage.Source = ImageSource.FromStream(() => st);

					st.CopyTo(ms);
					bitmap = SKBitmap.Decode(ms);
					//StartUploadMode();

					Console.WriteLine(st.CanRead);
					Console.WriteLine(result.FullPath);

					ms.Close();
					st.Close();
				});
		}
		private void togglefilter_Toggled(object sender, ToggledEventArgs e) {
			if (e.Value)
				cbmode = "";
			else
				cbmode = cbmodeorig;
		}
		#endregion imagemode

		#region threading
		private void StartCaptureMode() {
			Device.StartTimer(TimeSpan.FromMilliseconds(1000 / 60), CaptureAndApplyFilter);
		}
		private bool CaptureAndApplyFilter() {
			if (!isbusy) { 
				isbusy = true;
				cameraView.Shutter();
				canvasView.InvalidateSurface();
				isbusy = false;
			}
			return iscapturemode;
		}
		private void StartUploadMode() {
			Device.StartTimer(TimeSpan.FromMilliseconds(1000 / 60), UploadAndApplyFilter);
		}
		private bool UploadAndApplyFilter() {
			if (!isbusy) {
				isbusy = true;
				canvasView.InvalidateSurface();
				isbusy = false;
			}
			return isuploadmode;
		}
		#endregion threading

		#region camera
		private void CameraView_MediaCaptured(object sender, MediaCapturedEventArgs e) {
			MemoryStream ms = new MemoryStream(e.ImageData);
			bitmap = SKBitmap.Decode(ms);
		}

		private void CameraView_OnAvailable(object sender, bool e) {

		}

		private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e) {
			SKImageInfo info = e.Info;
			SKSurface surface = e.Surface;
			SKCanvas canvas = surface.Canvas;

			canvas.Clear();

			if (bitmap != null) {
				SKBitmap rotatedBitmap;
				if (cameraView.CameraOptions == CameraOptions.Back)
					rotatedBitmap = Rotate2(bitmap, 90);
				else
					rotatedBitmap = Rotate2(bitmap, -90);


				switch (cbmode) {
					case "Protanopia":
						canvas.DrawBitmap(rotatedBitmap, info.Rect, BitmapStretch.UniformToFill, paint: paintProtanopia);
						break;
					case "Deuteranopia":
						canvas.DrawBitmap(rotatedBitmap, info.Rect, BitmapStretch.UniformToFill, paint: paintDeuteranopia);
						break;
					case "Tritanopia":
						canvas.DrawBitmap(rotatedBitmap, info.Rect, BitmapStretch.UniformToFill, paint: paintTritanopia);
						break;
					default:
						canvas.DrawBitmap(rotatedBitmap, info.Rect, BitmapStretch.UniformToFill);
						break;
				}
			}
		}

		public static SKBitmap Rotate(SKBitmap bitmap, double angle) {
			double radians = Math.PI * angle / 180;
			float sine = (float) Math.Abs(Math.Sin(radians));
			float cosine = (float) Math.Abs(Math.Cos(radians));
			int originalWidth = bitmap.Width;
			int originalHeight = bitmap.Height;
			int rotatedWidth = (int) (cosine * originalWidth + sine * originalHeight);
			int rotatedHeight = (int) (cosine * originalHeight + sine * originalWidth);

			var rotatedBitmap = new SKBitmap(rotatedWidth, rotatedHeight);

			using (var surface = new SKCanvas(rotatedBitmap)) {
				surface.Translate(rotatedWidth / 2, rotatedHeight / 2);
				surface.RotateDegrees((float) angle);
				surface.Translate(-originalWidth / 2, -originalHeight / 2);
				surface.DrawBitmap(bitmap, new SKPoint());
			}
			return rotatedBitmap;
		}

		public static SKBitmap Rotate2(SKBitmap bitmap, double angle) {
			var rotated = new SKBitmap(bitmap.Height, bitmap.Width);

			using (var surface = new SKCanvas(rotated)) {
				surface.Translate(rotated.Width, 0);
				surface.RotateDegrees((float) angle);
				surface.DrawBitmap(bitmap, 0, 0);
			}

			return rotated;
		}
		#endregion camera
	}
}