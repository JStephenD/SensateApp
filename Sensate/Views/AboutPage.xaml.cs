using System;
using System.ComponentModel;
using Sensate.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sensate.Views {
	public partial class AboutPage : ContentPage {
		public AboutPage() {
			InitializeComponent();
			this.BindingContext = new AboutViewModel();
		}
	}
}