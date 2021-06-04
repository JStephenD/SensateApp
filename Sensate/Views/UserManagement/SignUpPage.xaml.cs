using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sensate.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using Sensate.Models;
using Firebase.Auth;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignupPage : ContentPage {

		public SignupPage() {
			InitializeComponent();
			this.BindingContext = new SignupViewModel();
		}

	}
}
