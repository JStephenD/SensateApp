using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sensate.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SensateAboutPage : ContentPage {
		public SensateAboutPage() {
			InitializeComponent();
			this.BindingContext = new SensateAboutViewModel();
		}
	}
}