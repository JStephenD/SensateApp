using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Sensate.Views;
using Xamarin.Forms.Platform.Android;
using Sensate.Droid;

[assembly: ExportRenderer(typeof(RecognitionModePage), typeof(RecognitionModePageRenderer))]
namespace Sensate.Droid {
	class RecognitionModePageRenderer : PageRenderer {
		public RecognitionModePageRenderer(Context context) : base(context) {
			Console.WriteLine("hello from android");
		}



		public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e) {
			Console.WriteLine("keypress down");
			switch (keyCode) {
				case Keycode.VolumeUp:
					Console.WriteLine("onkeydown volume up");
					break;
				case Keycode.VolumeDown:
					Console.WriteLine("onkeydown volume down");
					break;
			}

			return base.OnKeyDown(keyCode, e);
		}

		public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e) {
			Console.WriteLine("keypress up");


			return base.OnKeyUp(keyCode, e);
		}
	}
}