﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sensate.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IntroPage : ContentPage {
		public IntroPage() {
			InitializeComponent();

			this.BindingContext = new IntroViewModel();
		}
	}
}