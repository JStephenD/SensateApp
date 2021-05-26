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
	public partial class Test1 : ContentPage {
		private bool permissionGranted;

		public Test1() {
			var cameraPermissionsStatus = Permissions.CheckStatusAsync<Permissions.Camera>();
			if (cameraPermissionsStatus.Result != PermissionStatus.Granted) {
				Permissions.RequestAsync<Permissions.Camera>();
			}
			permissionGranted = cameraPermissionsStatus.Result == PermissionStatus.Granted;

			InitializeComponent();

			if (permissionGranted)
				zoomLabel.Text = string.Format("Zoom: {0}", zoomSlider.Value);
		}

		public void ZoomSlider_ValueChanged(object sender, ValueChangedEventArgs e) {
			if (permissionGranted) {
				cameraView.Zoom = zoomSlider.Value;
				zoomLabel.Text = string.Format("Zoom: {0}", Math.Round(zoomSlider.Value));
			}
		}

		public void VideoSwitch_Toggled(object sender, ToggledEventArgs e) {
			if (permissionGranted) {
				var captureVideo = e.Value;

				if (captureVideo)
					cameraView.CaptureMode = CameraCaptureMode.Video;
				else
					cameraView.CaptureMode = CameraCaptureMode.Photo;

				previewPicture.IsVisible = !captureVideo;

				doCameraThings.Text = e.Value ? "Start Recording"
					: "Snap Picture";
			}
		}

		// You can also set it to Default and External
		public void FrontCameraSwitch_Toggled(object sender, ToggledEventArgs e) {
			if (permissionGranted)
				cameraView.CameraOptions = e.Value ? CameraOptions.Front : CameraOptions.Back;
		}

		// You can also set it to Torch (always on) and Auto
		public void FlashSwitch_Toggled(object sender, ToggledEventArgs e) {
			if (permissionGranted)
				cameraView.FlashMode = e.Value ? CameraFlashMode.On : CameraFlashMode.Off;
		}

		public void DoCameraThings_Clicked(object sender, EventArgs e) {
			if (permissionGranted) {
				cameraView.Shutter();
				doCameraThings.Text = cameraView.CaptureMode == CameraCaptureMode.Video
					? "Stop Recording"
					: "Snap Picture";
			}
		}

		public void CameraView_OnAvailable(object sender, bool e) {
			if (permissionGranted) {
				if (e) {
					zoomSlider.Value = cameraView.Zoom;
					var max = cameraView.MaxZoom;
					if (max > zoomSlider.Minimum && max > zoomSlider.Value)
						zoomSlider.Maximum = max;
					else
						zoomSlider.Maximum = zoomSlider.Minimum + 1; // if max == min throws exception
				}

				doCameraThings.IsEnabled = e;
				zoomSlider.IsEnabled = e;
			}
		}

		public async void CameraView_MediaCaptured(object sender, MediaCapturedEventArgs e) {
			if (permissionGranted) {
				switch (cameraView.CaptureMode) {
					default:
					case CameraCaptureMode.Default:
					case CameraCaptureMode.Photo:
						previewPicture.IsVisible = true;
						previewPicture.Rotation = e.Rotation;
						previewPicture.Source = e.Image;
						doCameraThings.Text = "Snap Picture";

						await TextToSpeech.SpeakAsync("Captured Image X D");

						var assembly = this.GetType().GetTypeInfo().Assembly;
						var resources = assembly.GetManifestResourceNames();
						var resourceName = resources.Single(r => r.EndsWith("Sensate-auth.json", StringComparison.OrdinalIgnoreCase));
						var stream = assembly.GetManifestResourceStream(resourceName);

						Console.WriteLine(resourceName);
						string json_creds;
						using (StreamReader sr = new StreamReader(stream)) {
							json_creds = await sr.ReadToEndAsync();
						}
						Console.WriteLine(json_creds);


						ImageAnnotatorClientBuilder builder = new ImageAnnotatorClientBuilder {
							JsonCredentials = json_creds
						};
						ImageAnnotatorClient client = await builder.BuildAsync();
						AnnotateImageRequest request = new AnnotateImageRequest {
							Image = Google.Cloud.Vision.V1.Image.FromBytes(
								e.ImageData.AsMemory().ToArray()),
							Features = {
								new Feature { Type = Feature.Types.Type.ObjectLocalization }
							}
						};
						AnnotateImageResponse response = await client.AnnotateAsync(request);
						foreach (LocalizedObjectAnnotation annotation in response.LocalizedObjectAnnotations) {
							// string poly = string.Join(" - ", annotation.BoundingPoly.NormalizedVertices.Select(v => $"({v.X}, {v.Y})"));
							//string output = $"Object Identified: {annotation.Name}; ID: {annotation.Mid}; Score: {annotation.Score}; Bounding poly: ";
							string output = $"Object Identified: {annotation.Name} with a certainty of: {annotation.Score * 100} percent";
							Console.WriteLine(output);
							await TextToSpeech.SpeakAsync(output);
						}
						
						break;
					case CameraCaptureMode.Video:
						previewPicture.IsVisible = false;
						doCameraThings.Text = "Start Recording";
						break;
				}
			}
		}
	}
}