using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.TextToSpeech;

namespace Sensate.Views {

	public class CancelMe {
		public CancellationTokenSource CurrentToken { get; set; }

		public void CancelToken() { 
			try {
				CurrentToken.Cancel();
			} catch { Console.WriteLine("cancelme error"); }
		}

		public CancellationTokenSource CancelRenewToken() {
			CurrentToken.Cancel();
			return GetNewTokenSource();
		}

		public CancellationTokenSource GetNewTokenSource() {
			CurrentToken = new CancellationTokenSource();
			return CurrentToken;
		}

		public async Task Speak(string output, float speakRate) {
			CancelToken();
			try {
				await CrossTextToSpeech.Current.Speak($"...{output}", speakRate: speakRate, cancelToken: GetNewTokenSource().Token);
			} catch (TaskCanceledException tce) {
				Console.WriteLine("speak cancelled"); Console.WriteLine(tce.Message);
			} catch (Exception) { Console.WriteLine("speak cancelled error"); }
		}
	}
}
