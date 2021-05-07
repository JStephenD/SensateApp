using System;
using System.Collections.Generic;
using System.ComponentModel;
using Sensate.Models;
using Sensate.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sensate.Views {
	public partial class NewItemPage : ContentPage {
		public Item Item { get; set; }

		public NewItemPage() {
			InitializeComponent();
			BindingContext = new NewItemViewModel();
		}
	}
}