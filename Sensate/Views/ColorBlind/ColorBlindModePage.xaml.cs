using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using SkiaSharp.Views.Forms;

using Sensate.ViewModels;
using SkiaSharp;
using System.IO;
using System.Collections.Generic;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ColorBlindModePage : ContentPage {
		
		private SKBitmap bitmap;
		private string cbmode;

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


		public ColorBlindModePage() {
			InitializeComponent();

			cameraView.CaptureMode = CameraCaptureMode.Photo;
			cameraView.IsVisible = true;
			canvasView.IsVisible = true;

			cbmode = Preferences.Get("CBType", "Protanopia", "CBSettings");
			
			if (Preferences.Get("UserCategory", "Normal", "GeneralSettings") == "Normal") {
				canvasView.IsVisible = false;
			} else {
				Device.StartTimer(TimeSpan.FromSeconds(1f / 30), () => {
					canvasView.InvalidateSurface();
					cameraView.Shutter();
					return true;
				});
			}
		}

		private void CameraView_MediaCaptured(object sender, MediaCapturedEventArgs e) {
			MemoryStream ms = new MemoryStream(e.ImageData);
			bitmap = SKBitmap.Decode(ms);
		}

		private void CameraView_OnAvailable(object sender, bool e) {
			captureButton.IsEnabled = e;
		}

		private void captureButtonClicked(object sender, EventArgs e) {
			cameraView.Shutter();
		}

		private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e) {
			SKImageInfo info = e.Info;
			SKSurface surface = e.Surface;
			SKCanvas canvas = surface.Canvas;

			canvas.Clear(SKColors.White);
			
			if (bitmap != null) {
				SKBitmap rotatedBitmap = Rotate2(bitmap, 90);
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
				surface.RotateDegrees((float)angle);
				surface.DrawBitmap(bitmap, 0, 0);
			}

			return rotated;
		}
	}
}