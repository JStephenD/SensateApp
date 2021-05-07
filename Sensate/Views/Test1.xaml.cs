using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Threading.Tasks;

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

		public void CameraView_MediaCaptured(object sender, MediaCapturedEventArgs e) {
			if (permissionGranted) {
				switch (cameraView.CaptureMode) {
					default:
					case CameraCaptureMode.Default:
					case CameraCaptureMode.Photo:
						previewPicture.IsVisible = true;
						previewPicture.Rotation = e.Rotation;
						previewPicture.Source = e.Image;
						doCameraThings.Text = "Snap Picture";
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