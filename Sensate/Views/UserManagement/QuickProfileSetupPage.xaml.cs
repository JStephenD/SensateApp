using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Sensate.ViewModels;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QuickProfileSetupPage : ContentPage {
		public QuickProfileSetupPage() {
			InitializeComponent();
			this.BindingContext = new QuickProfileSetupViewModel();
		}

		
	}
}