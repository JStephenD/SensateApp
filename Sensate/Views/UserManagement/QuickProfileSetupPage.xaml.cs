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
			DatePicker.MaximumDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
		}

		
		public void Birthdate_Clicked(object sender, EventArgs e) {
			var currentDay = DateTime.Now.Year;
			var dateOfBirth = DatePicker.Date.Year;
			int calculatedAge = currentDay - dateOfBirth;
			
		
		}

	}
}