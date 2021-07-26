using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Sensate.Views {
	public class TutorialContentModel {
		public string Title { get; set; }
		public int TitleSize { get; set; }
		public string Subtitle { get; set; }
		public int SubtitleSize { get; set; }
		public ImageSource Image { get; set; }
		public int ImageWidth { get; set; }
		public string Details { get; set; }
		public int DetailsSize { get; set; }
	}
}
