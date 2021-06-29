using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Reflection;
using System.IO;
using Google.Cloud.Vision.V1;
using System.Linq;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecognitionModePage : ContentPage {
		string mode;
		Assembly assembly;
		string[] resources;
		string vision_authfile;
		string json_creds;
		Xamarin.Forms.ImageSource icon_general, icon_text, icon_face, icon_product;

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

			#region defaults
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
			#endregion defaults
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
		public void FlashFrameClick(object s, EventArgs e) {
			cameraView.FlashMode = (cameraView.FlashMode == CameraFlashMode.Off) ?
				CameraFlashMode.Torch : CameraFlashMode.Off;
		}
		public void DetectionModeSelectClick(object s, EventArgs e) {
			detectionmode.Focus();
		}
		public void DetectionModeChange(object s, EventArgs e) { 
			mode = detectionmode.SelectedItem.ToString();
			if (mode == "General Object Detection") {
				detectionmodeimage.Source = icon_general;
			} else if (mode == "Text Detection") { 
				detectionmodeimage.Source = icon_text;
			} else if (mode == "Face Detection") {
				detectionmodeimage.Source = icon_face;
			} else if (mode == "Product Detection") {
				detectionmodeimage.Source = icon_product;
			}
		}
		#endregion navigation

		#region camera
		private void DoCameraThingsClicked(object sender, EventArgs e) {
			cameraView.Shutter();
		}

		public void CameraView_OnAvailable(object sender, bool e) {
			
		}

		public async void CameraView_MediaCaptured(object sender, MediaCapturedEventArgs e) {
			mode = detectionmode.SelectedItem.ToString();

			await TextToSpeech.SpeakAsync("Captured Image");

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
							(mode == "Product Detection") ? Feature.Types.Type.LogoDetection :
								Feature.Types.Type.LabelDetection
					}
				}
			};

			AnnotateImageResponse response = await client.AnnotateAsync(request);
			if (mode == "General Object Detection") {
				Console.WriteLine(response.LabelAnnotations);
				foreach (EntityAnnotation annotation in response.LabelAnnotations) {
					string output = $"Object Identified: {annotation.Description}";
					Console.WriteLine(output);
					if (annotation.Score >= .80)
						await TextToSpeech.SpeakAsync(output);
				}
			} 
			if (mode == "Text Detection") {
				Console.WriteLine(response.TextAnnotations);
				foreach (EntityAnnotation text in response.TextAnnotations) {
					Console.WriteLine($"Description: {text.Description}");
					await TextToSpeech.SpeakAsync(text.Description);
					break;
				}
			} 
			if (mode == "Face Detection") {
				Console.WriteLine(response.FaceAnnotations);
				foreach (FaceAnnotation face in response.FaceAnnotations) {
					if (face.JoyLikelihood >= Likelihood.Possible)
						await TextToSpeech.SpeakAsync("It looks like a joyful person");
					if (face.AngerLikelihood >= Likelihood.Possible)
						await TextToSpeech.SpeakAsync("It looks like a mad person");
					if (face.SorrowLikelihood >= Likelihood.Possible)
						await TextToSpeech.SpeakAsync("It looks like a sad person");
					if (face.SurpriseLikelihood >= Likelihood.Possible)
						await TextToSpeech.SpeakAsync("It looks like a surprised person");

					if (face.HeadwearLikelihood >= Likelihood.Possible)
						await TextToSpeech.SpeakAsync("It also seems like the person is wearing a headgear");
				}
			}
			if (mode == "Product Detection") {
				Console.WriteLine(response.LogoAnnotations);
				foreach (EntityAnnotation logo in response.LogoAnnotations) {
					Console.WriteLine($"Description: {logo.Description}");
					await TextToSpeech.SpeakAsync($"Possible Brand: {logo.Description}");
				}
			} 
		}
		#endregion camera
	}
}