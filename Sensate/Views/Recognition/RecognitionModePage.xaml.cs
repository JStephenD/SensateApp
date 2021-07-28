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
		private bool isSpeaking = false;

		private CancellationTokenSource _cts;

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
			try {
				cameraView.CameraOptions = (cameraView.CameraOptions == CameraOptions.Front) ?
					CameraOptions.Back : CameraOptions.Front;
			} catch {
				cameraView.CameraOptions = (cameraView.CameraOptions == CameraOptions.Front) ?
					CameraOptions.Back : CameraOptions.Front;
			}
			zoomSlider.Maximum = cameraView.MaxZoom;
		}
		public async void FlashFrameClick(object s, EventArgs e) {
			//cameraView.FlashMode = (cameraView.FlashMode == CameraFlashMode.Off) ?
			//	CameraFlashMode.Torch : CameraFlashMode.Off;
			Console.WriteLine("hello");
			if (isFlashlight)
				await Flashlight.TurnOffAsync();
			else
				await Flashlight.TurnOnAsync();
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
			Vibration.Vibrate();
		}
		#endregion hardware

		#region camera
		private void DoCameraThingsClicked(object sender, EventArgs e) {
			cameraView.Shutter();
		}

		public void CameraView_OnAvailable(object sender, bool e) {

		}

		public async void CameraView_MediaCaptured(object sender, MediaCapturedEventArgs e) {
			mode = detectionmode.SelectedItem.ToString();

			previewImage.Rotation = e.Rotation;
			previewImage.Source = e.Image;
			previewImage.VerticalOptions = LayoutOptions.FillAndExpand;
			previewImage.IsVisible = true;

			if (isVibration) Vibration.Vibrate();

			try {
				await cancelme.Speak("Captured Image", speakRate);

				ImageAnnotatorClientBuilder builder = new ImageAnnotatorClientBuilder {
					JsonCredentials = json_creds
				};
				ImageAnnotatorClient client = await builder.BuildAsync();
				AnnotateImageRequest request = new AnnotateImageRequest {
					Image = Google.Cloud.Vision.V1.Image.FromBytes(
						e.ImageData.AsMemory().ToArray()),
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
					var detectedlabel = false;
					var detectedobject = false;

					Console.WriteLine(response.LocalizedObjectAnnotations);
					foreach (LocalizedObjectAnnotation annotation in response.LocalizedObjectAnnotations) {
						detectedobject = true;
						string output = $"Object Identified: {annotation.Name}";
						Console.WriteLine(output);
						//if (annotation.Score >= .80)
						await Speak(output);
					}

					Console.WriteLine(response.LabelAnnotations);
					List<Result> results = new List<Result>();
					if (detectedobject)
						await Speak("Related terms to Object can be");
					foreach (EntityAnnotation annotation in response.LabelAnnotations) {
						results.Add(new Result {
							desc = annotation.Description,
							score = annotation.Score
						}
						);
						detectedlabel = true;
					}
					results.OrderBy(x => x.score);

					var limit = 3;
					foreach (var r in results) {
						string output;
						if (detectedobject) {
							output = $"{r.desc}";
						} else {
							output = $"Object Identified: {r.desc}";
						}
						await Speak(output);
						//await TextToSpeech.SpeakAsync(output);
						if (limit-- == 0) break;
					}

					if (!detectedlabel && !detectedobject)
						await Speak("No object found in the captured image.");
				}
				if (mode == "Text Detection") {
					var detected = false;
					Console.WriteLine(response.TextAnnotations);
					foreach (EntityAnnotation text in response.TextAnnotations) {
						detected = true;
						Console.WriteLine($"Description: {text.Description}");
						await Speak(text.Description);
						break;
					}
					if (!detected) await Speak("No text found in the captured image.");
				}
				if (mode == "Face Detection") {
					var detected = false;
					var detectedemotion = false;
					Console.WriteLine(response.FaceAnnotations);
					foreach (FaceAnnotation face in response.FaceAnnotations) {
						detected = true;
						if (face.JoyLikelihood >= Likelihood.Possible) {
							await Speak("It looks like a joyful person"); detectedemotion = true;
						}
						if (face.AngerLikelihood >= Likelihood.Possible) {
							await Speak("It looks like a mad person"); detectedemotion = true;
						}
						if (face.SorrowLikelihood >= Likelihood.Possible) {
							await Speak("It looks like a sad person"); detectedemotion = true;
						}
						if (face.SurpriseLikelihood >= Likelihood.Possible) {
							await Speak("It looks like a surprised person"); detectedemotion = true;
						}
						if (face.HeadwearLikelihood >= Likelihood.Possible) {
							await Speak("It also seems like the person is wearing a headgear"); detectedemotion = true;
						}
					}
					if (!detectedemotion) await Speak("Face found but could not identify emotion.");
					else if (!detected) await Speak("No face found in the captured image.");
				}
				if (mode == "Logo Detection") {
					var detected = false;
					Console.WriteLine(response.LogoAnnotations);
					List<Result> results = new List<Result>();
					foreach (EntityAnnotation logo in response.LogoAnnotations) {
						detected = true;
						Console.WriteLine($"Description: {logo.Description}");
						await Speak($"Possible Logo: {logo.Description}");
					}
					if (!detected) await Speak("No logo found in the captured image.");
				}
			} catch {
				Console.WriteLine("error");
			}

			previewImage.IsVisible = false;
		}
		#endregion camera

		private async Task Speak(string output) {
			await cancelme.Speak(output, speakRate);
		}

		public class Result {
			public string desc;
			public double score;
		}
	}
}