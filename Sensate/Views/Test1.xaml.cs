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
		string mode;

		public Test1() {
			var cameraPermissionsStatus = Permissions.CheckStatusAsync<Permissions.Camera>();
			if (cameraPermissionsStatus.Result != PermissionStatus.Granted) {
				Permissions.RequestAsync<Permissions.Camera>();
			}
			permissionGranted = cameraPermissionsStatus.Result == PermissionStatus.Granted;

			InitializeComponent();
		}

		public void ZoomSlider_ValueChanged(object sender, ValueChangedEventArgs e) {
			if (permissionGranted) {
				cameraView.Zoom = zoomSlider.Value;
			}
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
						mode = "General Object Detection";

						await TextToSpeech.SpeakAsync("Captured Image");

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
								new Feature {
									Type = (mode == "General Object Detection") ?
										Feature.Types.Type.ObjectLocalization :
										Feature.Types.Type.LabelDetection
								}
							}
						};
						AnnotateImageResponse response = await client.AnnotateAsync(request);
						if (mode == "General Object Detection") {
							foreach (LocalizedObjectAnnotation annotation in response.LocalizedObjectAnnotations) {
								// string poly = string.Join(" - ", annotation.BoundingPoly.NormalizedVertices.Select(v => $"({v.X}, {v.Y})"));
								//string output = $"Object Identified: {annotation.Name}; ID: {annotation.Mid}; Score: {annotation.Score}; Bounding poly: ";
								string output = $"Object Identified: {annotation.Name} with a certainty of: {annotation.Score * 100:0.00} percent";
								Console.WriteLine(output);
								await TextToSpeech.SpeakAsync(output);
							}
						} else {
							var limit = 5;
							foreach (EntityAnnotation annotation in response.LabelAnnotations) {
								// string poly = string.Join(" - ", annotation.BoundingPoly.NormalizedVertices.Select(v => $"({v.X}, {v.Y})"));
								//string output = $"Object Identified: {annotation.Name}; ID: {annotation.Mid}; Score: {annotation.Score}; Bounding poly: ";
								string output = $"Object Identified: {annotation.Description} with a certainty of: {annotation.Score * 100:0.00} percent";
								Console.WriteLine(output);
								await TextToSpeech.SpeakAsync(output);
								limit --;
								if (limit <= 0)
									break;
							}
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