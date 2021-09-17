using System;
using System.Collections.Generic;
using System.Text;

namespace Sensate.Views {
	public class RecognitionResults {
		public List<RecognitionResult> results { get; set; } = new List<RecognitionResult>();
	}

	public class RecognitionResult { 
		public string fname { get; set; }
		public string generalLabel { get; set; }
		public List<Result> results { get; set; } = new List<Result>();
	}

	public class Result {
		public string desc { get; set; }
		public double score { get; set; }
	}
}
