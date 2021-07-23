using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sensate.ViewModels;
using Sensate.Models;
using Sensate.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FAQPage : ContentPage {

		private readonly ObservableCollection<FAQViewModel> getContents;
		private ObservableCollection<FAQViewModel> _expandedContent;
		private readonly SyncHelper.Settings _settings;

		public FAQPage() {
			InitializeComponent();
			//this.BindingContext = new FAQViewModel();
			getContents = FAQViewModel.Contents;
			UpdateListContent();

			_settings = SyncHelper.GetCurrentSettings();
		}

		protected override void OnAppearing() {
			base.OnAppearing();

			if (_settings.TextSize == 0) {
				textTitle.FontSize = 22;
				textSubtitle.FontSize = 18;
			} else if (_settings.TextSize == 0) {
				textTitle.FontSize = 24;
				textSubtitle.FontSize = 20;
			} else {
				textTitle.FontSize = 26;
				textSubtitle.FontSize = 22;
			}
		}

		private void HeaderTapped(object sender, EventArgs args){
			int ListSelectedIndex = _expandedContent.IndexOf(((FAQViewModel)((Button)sender).CommandParameter));
			getContents[ListSelectedIndex].Expanded = !getContents[ListSelectedIndex].Expanded;
			UpdateListContent();
		}
		private void UpdateListContent(){
			_expandedContent = new ObservableCollection<FAQViewModel>();
			foreach (FAQViewModel group in getContents){
				FAQViewModel faqs = new FAQViewModel(group.Title, group.Expanded);
				faqs.FAQItems = group.Count;
				if (group.Expanded){
					foreach(FAQClassModel faq in group){
						faqs.Add(faq);
					}
				}
				_expandedContent.Add(faqs);
			}
			MyListView.ItemsSource = _expandedContent;
		}
	}
}