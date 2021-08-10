
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
		private byte[] imagebytearray;
		private string cbmode, cbmodeorig;
		private bool iscapturemode = true;
		private bool isuploadmode = false;
		private SyncHelper.Settings _settings;
		private bool isbusy = false;
		private readonly bool isVibration;
		private bool isBackCam = true;
		private Assembly assembly;

		private static readonly float[] Protanopia = {
			0.567F, 0.433F,     0F,		0F,		0,
			0.558F, 0.442F,     0F,		0F,		0,
			0F,		0.242F,		0.758F, 0F,		0,
			0F,     0F,			0F,		1F,		0,
		};

		private static readonly float[] Deuteranopia = {
			0.625f,	0.375f,	0,		0,		0,
			0.7f,	0.3f,	0,		0,		0,
			0,		0.3f,	0.7f,	0,		0,
			0,		0,		0,		1,		0
		};

		private static readonly float[] Tritanopia = {
			0.95f,	0.05f,		0,		0,		0,
			0,		0.433f,		0.567f,	0,		0,
			0,		0.475f,		0.525f,	0,		0,
			0,		0,			0,		1,		0
		};

		private readonly SKPaint paintProtanopia = new SKPaint {
			ColorFilter = SKColorFilter.CreateColorMatrix(Protanopia),
			Style = SKPaintStyle.Fill
		};

		private readonly SKPaint paintDeuteranopia = new SKPaint {
			ColorFilter = SKColorFilter.CreateColorMatrix(Deuteranopia),
			Style = SKPaintStyle.Fill
		};
		private readonly SKPaint paintTritanopia = new SKPaint {
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

			var twotaprotate = new TapGestureRecognizer();
			twotaprotate.NumberOfTapsRequired = 2;
			twotaprotate.Tapped += RotateCamFrameClick;
			canvasView.GestureRecognizers.Add(twotaprotate);

			var flashframeclick = new TapGestureRecognizer();
			flashframeclick.Tapped += FlashFrameClick;
			flashFrame.GestureRecognizers.Add(flashframeclick);

			var colormodeframeclick = new TapGestureRecognizer();
			colormodeframeclick.Tapped += ColorModeFrameClick;
			colormodeFrame.GestureRecognizers.Add(colormodeframeclick);

			var testclicktap = new TapGestureRecognizer();
			testclicktap.Tapped += testclick;
			canvasView.GestureRecognizers.Add(testclicktap);
			cameraView.GestureRecognizers.Add(testclicktap);
			#endregion gesturerecognizers

			cameraView.CaptureMode = CameraCaptureMode.Photo;
			cameraView.IsVisible = true;
			canvasView.IsVisible = true;

			togglefilter.IsVisible = false;
			togglefilterFrame.IsVisible = false;
			toggleFilterStackFrame.IsVisible = false;

			debugimage.IsVisible = false;

			cbmode = cbmodeorig = Preferences.Get("CBType", "Protanopia", "CBSettings");

			if (Preferences.Get("UserCategory", "Normal", "GeneralSettings") == "Normal") {
				//canvasView.IsVisible = false;
			} else {
				StartCaptureMode();
			}

			isVibration = Preferences.Get("VibrationFeedback", false, "GeneralSettings");
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			_settings = SyncHelper.GetCurrentSettings();

			if (_settings.Gesture) {
				Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
				Accelerometer.Start(SensorSpeed.Game);
			}
		}

		protected override void OnDisappearing() {
			base.OnDisappearing();

			if (_settings.Gesture) {
				Accelerometer.Stop();
				Accelerometer.ShakeDetected -= Accelerometer_ShakeDetected;
			}
		}

		public void testclick(object s, EventArgs e) {
			Console.WriteLine(s.ToString());
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
			isbusy = true;
			try {
				cameraView.CameraOptions = (cameraView.CameraOptions == CameraOptions.Front) ?
					CameraOptions.Back : CameraOptions.Front;
			} catch {
				cameraView.CameraOptions = (cameraView.CameraOptions == CameraOptions.Front) ?
					CameraOptions.Back : CameraOptions.Front;
			}
			isBackCam = !isBackCam;
			isbusy = false;
		}
		public void FlashFrameClick(object s, EventArgs e) {
			if (cameraView.FlashMode == CameraFlashMode.Off)
				cameraView.FlashMode = CameraFlashMode.Torch;
			else
				cameraView.FlashMode = CameraFlashMode.Off;
		}
		public void ColorModeFrameClick(object s, EventArgs e) {
			colormode.Focus();
		}
		public void ColorModeChange(object s, EventArgs e) {
			cbmode = cbmodeorig = colormode.SelectedItem.ToString();
		}
		void Accelerometer_ShakeDetected(object sender, EventArgs e) {
			var imode = colormode.SelectedIndex;
			if (imode == 0) {
				colormode.SelectedIndex = 1;
				cbmode = cbmodeorig = "Deuteranopia";
			} else if (imode == 1) {
				colormode.SelectedIndex = 2;
				cbmode = cbmodeorig = "Tritanopia";
			} else if (imode == 2) {
				colormode.SelectedIndex = 0;
				cbmode = cbmodeorig = "Protanopia";
			}
			if (isVibration) Vibration.Vibrate();
		}
		#endregion navigation

		#region imagemode
		public void CameraFrameClick(object s, EventArgs e) {
			cameraFrame.BackgroundColor = Color.FromHex("#FF881A");
			uploadFrame.BackgroundColor = Color.FromHex("#EFEFEF");
			IconTintColorEffect.SetTintColor(cameraImage, Color.White);
			IconTintColorEffect.SetTintColor(uploadImage, Color.FromHex("#00384F"));

			if (isVibration) Vibration.Vibrate();

			togglefilter.IsVisible = false;
			togglefilterFrame.IsVisible = false;
			toggleFilterStackFrame.IsVisible = false;
			iscapturemode = true;
			isuploadmode = false;
			StartCaptureMode();

			debugimage.IsVisible = false;
		}
		public async void UploadFrameClick(object s, EventArgs e) {
			uploadFrame.BackgroundColor = Color.FromHex("#FF881A");
			cameraFrame.BackgroundColor = Color.FromHex("#EFEFEF");
			IconTintColorEffect.SetTintColor(uploadImage, Color.White);
			IconTintColorEffect.SetTintColor(cameraImage, Color.FromHex("#00384F"));

			if (isVibration) Vibration.Vibrate();

			togglefilter.IsVisible = true;
			togglefilterFrame.IsVisible = true;
			toggleFilterStackFrame.IsVisible = true;
			iscapturemode = false;
			isuploadmode = true;
			cbmode = "";

			assembly = this.GetType().GetTypeInfo().Assembly;
			var resources = assembly.GetManifestResourceNames();
			var resourceName = resources.Single(r => r.EndsWith("upload-image-default.png", StringComparison.OrdinalIgnoreCase));

			if (isVibration) Vibration.Vibrate();

			Console.WriteLine("uploading");

			await MediaPicker.PickPhotoAsync()
				.ContinueWith(async t => {
					if (t.IsCanceled) {
						Console.WriteLine("cancelled picking photo");
						CameraFrameClick(null, null);
						if (isVibration) Vibration.Vibrate();
						return;
					}
					var result = t.Result;
					if (result == null) {
						Console.WriteLine("null picking photo");
						CameraFrameClick(null, null);
						if (isVibration) Vibration.Vibrate();
						return;
					}

					using (MemoryStream ms = new MemoryStream()) {
						using (Stream stream = await result.OpenReadAsync()) {
							stream.CopyTo(ms);
							imagebytearray = ms.ToArray();
							bitmap = SKBitmap.Decode(imagebytearray);
							debugimage.IsVisible = false;
							Console.WriteLine("uploaded");
							StartUploadMode();
							if (isVibration) Vibration.Vibrate();
						}
					}
				});
		}
		private void togglefilter_Toggled(object sender, ToggledEventArgs e) {
			if (e.Value)
				cbmode = cbmodeorig;
			else
				cbmode = "";
		}
		#endregion imagemode

		#region threading
		private void StartCaptureMode() {
			Device.StartTimer(TimeSpan.FromMilliseconds(1000 / 60), CaptureAndApplyFilter);
		}

		System.Diagnostics.Stopwatch watch;

		private bool CaptureAndApplyFilter() {
			if (!isbusy) {
				isbusy = true;
				//watch = System.Diagnostics.Stopwatch.StartNew();
				cameraView.Shutter();
				canvasView.InvalidateSurface();
				isbusy = false;
			}
			return iscapturemode;
		}
		private void StartUploadMode() {
			Device.StartTimer(TimeSpan.FromMilliseconds(1000 / 30), UploadAndApplyFilter);
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
		public void CameraView_MediaCaptured(object sender, MediaCapturedEventArgs e) {
			if (!isbusy) {
				Console.WriteLine("captured");
				bitmap = SKBitmap.Decode(e.ImageData);
			}
		}

		public void CameraView_OnAvailable(object sender, bool e) {

		}

		private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e) {
			SKImageInfo info = e.Info;
			SKSurface surface = e.Surface;
			SKCanvas canvas = surface.Canvas;

			canvas.Clear(SKColors.White);

			if (bitmap != null) {
				if (isuploadmode) {
					Console.WriteLine(cbmode);
					canvas.DrawBitmap(bitmap, info.Rect, BitmapStretch.AspectFit,
						paint: (cbmode == "Protanopia") ? paintProtanopia :
								(cbmode == "Deuteranopia") ? paintDeuteranopia :
								(cbmode == "Tritanopia") ? paintTritanopia :
								null);
				} else {
					//Console.WriteLine($"is back cam {isBackCam}");
					SKBitmap rotatedBitmap;
					if (isBackCam) {
						rotatedBitmap = Rotate2(bitmap, 90);
						canvas.DrawBitmap(rotatedBitmap, info.Rect, BitmapStretch.AspectFill,
						paint: (cbmode == "Protanopia") ? paintProtanopia :
								(cbmode == "Deuteranopia") ? paintDeuteranopia :
								(cbmode == "Tritanopia") ? paintTritanopia :
								null);
					} else {
						rotatedBitmap = Rotate(bitmap, 270);
						//rotatedBitmap = Rotate2(rotatedBitmap, 180);
						canvas.DrawBitmap(rotatedBitmap, info.Rect, BitmapStretch.AspectFill,
						paint: (cbmode == "Protanopia") ? paintProtanopia :
								(cbmode == "Deuteranopia") ? paintDeuteranopia :
								(cbmode == "Tritanopia") ? paintTritanopia :
								null);
					}
				}
			}
			//if (watch != null) {
			//	watch.Stop();
			//	Console.WriteLine($"ellapsed time ce mode capture {watch.ElapsedMilliseconds}");
			//}
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