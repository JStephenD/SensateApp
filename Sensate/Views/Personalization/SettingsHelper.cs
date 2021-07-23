using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using Sensate.Models;

namespace Sensate.Views {
	class SettingsHelper {

		public static void ApplyDisplaySettings(Label[] labels = null) {
			var isBold = Preferences.Get("BoldText", false, "GeneralSettings");
			var textSize = Preferences.Get("TextSize", 1, "GeneralSettings");

			if (labels != null) { 
				foreach (var lab in labels) {
					if (isBold) {
						lab.FontAttributes = FontAttributes.Bold;
					} else {
						lab.FontAttributes = FontAttributes.None;
					}

					if (textSize == 0) {
						lab.Scale = 0.8;
						lab.Margin = new Thickness(0);
						//lab.FontSize = lab.FontSize - 2;
					} else if (textSize == 1) {
						lab.Scale = 1;
						lab.Margin = new Thickness(0);
						//lab.FontSize = lab.FontSize;
					} else {
						lab.Scale = 1.2;
						lab.Margin = new Thickness(10);
						//lab.FontSize = lab.FontSize + 2;
					}
				}
			}
		}
	}	
}
