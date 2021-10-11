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
using Plugin.TextToSpeech;
using System.Threading;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Text.Json;
using Google.Protobuf.Collections;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecognitionModePage : ContentPage {
		string mode;
		Assembly assembly;
		string[] resources;
		string vision_authfile;
		string json_creds;
		bool isVibration;
		bool isGesture;
		Xamarin.Forms.ImageSource icon_general, icon_text, icon_face, icon_product;
		private SyncHelper.Settings _settings;
		float speakRate;
		CancelMe cancelme;
		private bool isFlashlight = false;
		private SKBitmap bitmap;
		private bool isSpeaking = false;
		private bool isBackCam = true;

		private CancellationTokenSource _cts;

		private static string filedir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		private string resultsfile = Path.Combine(filedir, "RecognitionResults.json");
		private string tempgeneralLabel = "";

		public RecognitionModePage() {
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

			var rotatecamframeclick = new TapGestureRecognizer();
			rotatecamframeclick.Tapped += RotateCamFrameClick;
			rotatecamFrame.GestureRecognizers.Add(rotatecamframeclick);

			var twotaprotate = new TapGestureRecognizer();
			twotaprotate.NumberOfTapsRequired = 2;
			twotaprotate.Tapped += RotateCamFrameClick;
			cameraView.GestureRecognizers.Add(twotaprotate);

			var flashframeclick = new TapGestureRecognizer();
			flashframeclick.Tapped += FlashFrameClick;
			flashFrame.GestureRecognizers.Add(flashframeclick);

			doCameraThings.Clicked += DoCameraThingsClicked;

			var detectionmodeselectclick = new TapGestureRecognizer();
			detectionmodeselectclick.Tapped += DetectionModeSelectClick;
			detectionmodeselect.GestureRecognizers.Add(detectionmodeselectclick);
			#endregion gesturerecognizers
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			#region defaults
			_settings = SyncHelper.GetCurrentSettings();
			cancelme = new CancelMe();

			speakRate = (_settings.VoiceSpeed == 0) ? .7f :
						(_settings.VoiceSpeed == 1) ? 1f :
													1.3f;

			assembly = this.GetType().GetTypeInfo().Assembly;
			resources = assembly.GetManifestResourceNames();
			vision_authfile = resources.Single(r => r.EndsWith("Sensate-auth.json", StringComparison.OrdinalIgnoreCase));
			var stream = assembly.GetManifestResourceStream(vision_authfile);
			using (StreamReader sr = new StreamReader(stream)) {
				json_creds = sr.ReadToEnd();
			}
			stream.Close();

			icon_general = Xamarin.Forms.ImageSource.FromResource("Sensate.Assets.recognition-general.png", assembly);
			icon_text = Xamarin.Forms.ImageSource.FromResource("Sensate.Assets.recognition-text.png", assembly);
			icon_face = Xamarin.Forms.ImageSource.FromResource("Sensate.Assets.recognition-face.png", assembly);
			icon_product = Xamarin.Forms.ImageSource.FromResource("Sensate.Assets.recognition-product.png", assembly);

			detectionmode.SelectedIndex = 0;
			mode = detectionmode.SelectedItem.ToString();

			isVibration = Preferences.Get("VibrationFeedback", false, "GeneralSettings");
			isGesture = Preferences.Get("Gesture", false, "GeneralSettings");

			_cts = new CancellationTokenSource();
			#endregion defaults

			if (isGesture) {
				Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
				Accelerometer.Start(SensorSpeed.Game);
			}

			//Debug

			//List<Result> results = new List<Result> {
			//	new Result {
			//		desc = "shoe",
			//		score = 74.9
			//	}
			//};

			//RecognitionResult newresult = new RecognitionResult {
			//	fname = "hello.txt",
			//	generalLabel = "purchased goods",
			//	results = results
			//};

			//RecognitionResults recognitionResults = new RecognitionResults();
			//recognitionResults.results.Add(newresult);
			//Console.WriteLine("add new result");
			//Console.WriteLine(JsonSerializer.Serialize(newresult));
			//Console.WriteLine(JsonSerializer.Serialize(recognitionResults));
		}

		protected override void OnDisappearing() {
			base.OnDisappearing();
			if (isGesture) {
				Accelerometer.Stop();
				Accelerometer.ShakeDetected -= Accelerometer_ShakeDetected;
			}
			cancelme.CancelToken();
		}

		#region zooming
		public void ZoomSlider_ValueChanged(object sender, ValueChangedEventArgs e) {
			cameraView.Zoom = zoomSlider.Value;
		}

		public async void ZoomInClick(object s, EventArgs e) {
			//await Shell.Current.GoToAsync(nameof(RecognitionResultPage));

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
			try {
				cameraView.CameraOptions = (cameraView.CameraOptions == CameraOptions.Front) ?
					CameraOptions.Back : CameraOptions.Front;
			} catch {
				cameraView.CameraOptions = (cameraView.CameraOptions == CameraOptions.Front) ?
					CameraOptions.Back : CameraOptions.Front;
			}
			zoomSlider.Maximum = cameraView.MaxZoom;
			isBackCam = !isBackCam;
		}
		public async void FlashFrameClick(object s, EventArgs e) {
			cameraView.FlashMode = (cameraView.FlashMode == CameraFlashMode.Off) ?
				CameraFlashMode.On : CameraFlashMode.Off;
			Console.WriteLine("hello");
			//if (isFlashlight)
			//	await Flashlight.TurnOffAsync();
			//else
			//	await Flashlight.TurnOnAsync();
			isFlashlight = !isFlashlight;
		}
		public void DetectionModeSelectClick(object s, EventArgs e) {
			detectionmode.Focus();
		}
		public async void DetectionModeChange(object s, EventArgs e) {
			mode = detectionmode.SelectedItem.ToString();
			if (mode == "General Object Detection") {
				detectionmodeimage.Source = icon_general;
			} else if (mode == "Text Detection") {
				detectionmodeimage.Source = icon_text;
			} else if (mode == "Face Detection") {
				detectionmodeimage.Source = icon_face;
			} else if (mode == "Logo Detection") {
				detectionmodeimage.Source = icon_product;
			}

			await Speak($"{mode}");
		}

		#endregion navigation

		#region hardware
		void Accelerometer_ShakeDetected(object sender, EventArgs e) {
			var imode = detectionmode.SelectedIndex;
			if (imode == 0) {
				detectionmode.SelectedIndex = 1;
				detectionmodeimage.Source = icon_text;
			} else if (imode == 1) {
				detectionmode.SelectedIndex = 2;
				detectionmodeimage.Source = icon_face;
			} else if (imode == 2) {
				detectionmode.SelectedIndex = 3;
				detectionmodeimage.Source = icon_product;
			} else if (imode == 3) {
				detectionmode.SelectedIndex = 0;
				detectionmodeimage.Source = icon_general;
			}
			if (isVibration) Vibration.Vibrate();
		}
		#endregion hardware

		#region camera
		private void DoCameraThingsClicked(object sender, EventArgs e) {
			cameraView.Shutter();
		}

		public void CameraView_OnAvailable(object sender, bool e) {

		}

		public async Task ProcessObjectRecognitionResponse(
				RepeatedField<LocalizedObjectAnnotation> objectAnnotations,
				RepeatedField<EntityAnnotation> labelAnnotations) {

			var detectedHand = false;
			var tooGeneralObject = false;

			//[1] hand presence
			foreach (EntityAnnotation annotation in labelAnnotations) {
				if (annotation.Description.ToLower() == "hand") {
					detectedHand = true;
					break;
				}
			}
			await Speak((detectedHand) ? "i can see someone holding" : "i can see");

			//[2] obj loc
			Dictionary<string, int> counts = new Dictionary<string, int>();
			foreach (LocalizedObjectAnnotation annotation in objectAnnotations) {
				var key = annotation.Name;
				if (counts.ContainsKey(key)) {
					counts[key] += 1;
				} else { 
					counts[key] = 1;
				}
			}

			var addAnd = false;
			foreach (var kvp in counts) { 
				if (addAnd) await Speak("and");

				if (kvp.Key == "packaged goods") {
					await Speak("a packaged good which is an item that is inside a container, such as consumer goods or products");
					tooGeneralObject = true;
				} else if (kvp.Key == "food") {
					await Speak("a food which is something that we consume or eat");
					tooGeneralObject = true;
				} else if (kvp.Key == "dishware" || kvp.Key == "tableware") {
					await Speak("an item that we see on a table such as utensils or tools for dining");
					tooGeneralObject = true;
				} else if (kvp.Key == "animal") {
					await Speak("an animal or a living creature");
					tooGeneralObject = true;
				} else if (kvp.Key == "communication device") {
					await Speak("a device that we use for communications");
					tooGeneralObject = true;
				} else if (kvp.Key == "person" && kvp.Key == "top") {
					await Speak("a person wearing a top or t-shirt");
					tooGeneralObject = true;
				} else if (kvp.Key == "bags" || kvp.Key == "luggage") {
					await Speak("a bag");
					tooGeneralObject = true;
				} else {
					if (kvp.Value > 1) {
						await Speak(kvp.Key);
					} else {
						await Speak(kvp.Value.ToString() + " " + kvp.Key);
					}
				}
				addAnd = true;
			}


			//[3] labels
			List<Result> results = new List<Result>();
			foreach (EntityAnnotation annotation in labelAnnotations) {
				results.Add(new Result {
					desc = annotation.Description,
					score = annotation.Score
				}
				);
			}
			results.OrderBy(x => x.score);

			await Speak((tooGeneralObject) 
				? "i am not really about sure this item but the terms related to what i can see are" 
				: "the terms related to what i can see are");

			var limit = 3;
			foreach (var r in results) {
				await Speak(r.desc);
				if (limit-- == 0) break;
			}
		}


		public async void CameraView_MediaCaptured(object sender, MediaCapturedEventArgs e) {
			mode = detectionmode.SelectedItem.ToString();

			//previewImage.Rotation = e.Rotation;
			//previewImage.Source = e.Image;
			//previewImage.VerticalOptions = LayoutOptions.FillAndExpand;
			//previewImage.IsVisible = true;

			var imagedata = e.ImageData;

			bitmap = SKBitmap.Decode(imagedata);
			canvasView.InvalidateSurface();
			canvasView.IsVisible = true;

			if (isVibration) Vibration.Vibrate();

			try {
				await cancelme.Speak("Captured Image", speakRate);

				//var watch = System.Diagnostics.Stopwatch.StartNew();
				ImageAnnotatorClientBuilder builder = new ImageAnnotatorClientBuilder {
					JsonCredentials = json_creds
				};
				ImageAnnotatorClient client = await builder.BuildAsync();
				AnnotateImageRequest request = new AnnotateImageRequest {
					Image = Google.Cloud.Vision.V1.Image.FromBytes(
						imagedata.AsMemory().ToArray()),
					Features = {
						new Feature {
							Type =
								(mode == "General Object Detection") ? Feature.Types.Type.LabelDetection :
								(mode == "Text Detection") ? Feature.Types.Type.TextDetection :
								(mode == "Face Detection") ? Feature.Types.Type.FaceDetection :
								(mode == "Logo Detection") ? Feature.Types.Type.LogoDetection :
									Feature.Types.Type.LabelDetection
						},
						new Feature {
							Type =
								(mode == "General Object Detection") ? Feature.Types.Type.ObjectLocalization :
								Feature.Types.Type.Unspecified
						}
					}
				};

				AnnotateImageResponse response = await client.AnnotateAsync(request);

				if (mode == "General Object Detection") {
					await ProcessObjectRecognitionResponse(
						response.LocalizedObjectAnnotations, response.LabelAnnotations);

					//string fname = $"{Guid.NewGuid()}.jpg";
					//string saveto = Path.Combine(filedir, fname);
					//File.WriteAllBytes(saveto, imagedata);

					//RecognitionResult newresult = new RecognitionResult{
					//	fname = fname,
					//	generalLabel = tempgeneralLabel,
					//	results = finalresults
					//};

					//if (File.Exists(resultsfile)) { 
					//	Console.WriteLine("file exists");
					//	string readfile = File.ReadAllText(resultsfile);
					//	RecognitionResults recognitionResults = JsonSerializer.Deserialize<RecognitionResults>(readfile);
					//	recognitionResults.results.Add(newresult);
					//	string serialized = JsonSerializer.Serialize(recognitionResults);
					//	File.WriteAllText(resultsfile, serialized);
					//} else {
					//	Console.WriteLine("file not exists");
					//	RecognitionResults recognitionResults = new RecognitionResults();
					//	recognitionResults.results.Add(newresult);
					//	string serialized = JsonSerializer.Serialize(recognitionResults);
					//	File.WriteAllText(resultsfile, serialized);
					//}
				}
				if (mode == "Text Detection") {
					var detected = false;
					Console.WriteLine(response.TextAnnotations);
					foreach (EntityAnnotation text in response.TextAnnotations) {
						detected = true;
						Console.WriteLine($"Description: {text.Description}");
						await Speak("i can see some texts, i will read it out loud");
						await Speak(text.Description);
						break;
					}
					if (!detected) await Speak("No text found in the captured image");
				}
				if (mode == "Face Detection") {
					var detected = false;
					var detectedemotion = false;
					Console.WriteLine(response.FaceAnnotations);
					foreach (FaceAnnotation face in response.FaceAnnotations) {
						detected = true;
						if (face.JoyLikelihood >= Likelihood.Possible) {
							await Speak("i can see a face, It looks like a joyful person"); detectedemotion = true;
						}
						if (face.AngerLikelihood >= Likelihood.Possible) {
							await Speak("i can see a face, It looks like a mad person"); detectedemotion = true;
						}
						if (face.SorrowLikelihood >= Likelihood.Possible) {
							await Speak("i can see a face, It looks like a sad person"); detectedemotion = true;
						}
						if (face.SurpriseLikelihood >= Likelihood.Possible) {
							await Speak("i can see a face, It looks like a surprised person"); detectedemotion = true;
						}
						if (face.HeadwearLikelihood >= Likelihood.Possible) {
							await Speak("It also seems like the person is wearing a headgear"); detectedemotion = true;
						}
					}
					if (!detectedemotion) await Speak("Face found but could not identify emotion");
					else if (!detected) await Speak("No face found in the captured image");
				}
				if (mode == "Logo Detection") {
					var detected = false;
					Console.WriteLine(response.LogoAnnotations);
					List<Result> results = new List<Result>();
					foreach (EntityAnnotation logo in response.LogoAnnotations) {
						detected = true;
						Console.WriteLine($"Description: {logo.Description}");
						await Speak($"i can see a logo of a product, it seems to be {logo.Description}");
					}
					if (!detected) await Speak("No logo found in the captured image");
				}
			} catch {
				Console.WriteLine("error");
			}

			previewImage.IsVisible = false;
			canvasView.IsVisible = false;
		}

		private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e) {
			SKImageInfo info = e.Info;
			SKSurface surface = e.Surface;
			SKCanvas canvas = surface.Canvas;

			canvas.Clear();

			if (bitmap != null) {
				var rotatedbitmap = (isBackCam) ? Rotate2(bitmap, 90) : Rotate(bitmap, 270);
				canvas.DrawBitmap(rotatedbitmap, info.Rect, BitmapStretch.AspectFill);
			}
		}

		#endregion camera

		private async Task Speak(string output) {
			await cancelme.Speak(output, speakRate);
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
	}
}