using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Sensate.Models;
using Sensate.Views;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Sensate.ViewModels {
	public class FAQViewModel : ObservableCollection<FAQClassModel>, INotifyPropertyChanged {
		private bool _expanded;
		public string Title { get; set; }
		public string Name { get; set; }

		public bool Expanded{
			get { return _expanded; }
			set {
				if (_expanded != value){
					_expanded = value;
					OnPropertyChanged("Expanded");
					OnPropertyChanged("StateIcon");
				}
			}
		}
		public string StateIcon{
			get { return Expanded ? "../Assets/arrow-down.png" : "../Assets/arrow-up.png"; }
		}

		public int FAQItems { get; set; }
		public FAQViewModel(string title, bool expanded=false){
			Title = title;
			Expanded = expanded;
		}
		public static ObservableCollection<FAQViewModel> Contents { private set; get; }
		static FAQViewModel() {
			ObservableCollection<FAQViewModel> Items = new ObservableCollection<FAQViewModel>{
				new FAQViewModel("What is Sensate?"){
					new FAQClassModel { Description = "Sensate is an AI driven application which utilizes object recognition technology to aid those who are low in vision or color blind, providing assistance."}
				},
				new FAQViewModel("How to Delete My Account? "){
					new FAQClassModel { Description = "Click the menu button and head to account profile. The delete account link is displayed and it must be tapped. The user must then confirm the deletion of account.  "}
				},
				new FAQViewModel("How to Change My Visual Impairment Category? "){
					new FAQClassModel { Description = "Click the menu button and head to account profile. The edit profile button is displayed and it must be clicked. On the category field, the user can reselect the type of category and cofirm the changes. "}
				},
				new FAQViewModel("How to Reconfigure the Interactve Settings? "){
					new FAQClassModel { Description = "Click the menu button and head to interactive settings. The user can now navigate on what needs to updated on the settings. Also, to save the reconfugarations, make sure to click the sync button on the account profile. "}
				}
			};
			Contents = Items;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName){
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}