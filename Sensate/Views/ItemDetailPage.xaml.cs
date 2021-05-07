using System.ComponentModel;
using Sensate.ViewModels;
using Xamarin.Forms;

namespace Sensate.Views {
	public partial class ItemDetailPage : ContentPage {
		public ItemDetailPage() {
			InitializeComponent();
			BindingContext = new ItemDetailViewModel();
		}
	}
}