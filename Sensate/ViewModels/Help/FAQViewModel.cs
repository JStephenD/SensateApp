using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Sensate.Models;
using Sensate.Views;
using System.Linq;
using System.Text;
using Sensate;
using System.Reflection;

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
		public ImageSource StateIcon{
			//get { return Expanded ? "../Assets/arrow-down.png" : "../Assets/arrow-up.png"; }
			get {
				return Expanded ?
					ImageSource.FromResource("Sensate.Assets.arrow-down.png", typeof(ImageResourceExtension).GetTypeInfo().Assembly) :
					ImageSource.FromResource("Sensate.Assets.arrow-up.png", typeof(ImageResourceExtension).GetTypeInfo().Assembly);
			}
		}

		public int FAQItems { get; set; }
		public FAQViewModel(string title, bool expanded=false){
			Title = title;
			Expanded = expanded;
		}
		public static ObservableCollection<FAQViewModel> Contents { private set; get; }
		static FAQViewModel() {
			ObservableCollection<FAQViewModel> Items = new ObservableCollection<FAQViewModel>{
				new FAQViewModel("What are the features of Sensate?"){
					new FAQClassModel { Description = "Sensate has five main features which are: Sensory-Oriented UI, Recognition Mode, Color Enhancement, Account Management and Personalization. With Sensory-Oriented UI, users can maximize the utility of their other senses aside form vision, such as hearing and touch, through audio and vibrational feedback, and gesture-based shortcuts or navigation. The Recognition Mode allows low vision users to capture an image and recognize it for clarification purposes. Color Enhancement provides color filtering through the camera or image upload to guide colorblind users. The Account Management allows users to create an account and save their preferences for device migrations in the future. Finally, the Personalization feature provides configurations to be customized such as on the feedback, display and navigation preferences."}
				},
				new FAQViewModel("How to manage an account?"){
					new FAQClassModel { Description = "The first step of account management is to create an account through  sign-up. If you're a new user, you are most likely recommended to create an account. Just fill up the information needed (make sure to complete them) and you're good to go, just make sure to remember your sign-in credentials. If you logged out, and used another device, just go to the sign-in page of Sensate and enter your credentials again. To view your profile, just go to the menu bar by clicking the menu button on the upper left side. You can also edit your profile if you wish, just click the edit profile button, make some changes and save them with the confirm button. For account deletion, there is a delete account option provided on the profile section, just click it and confirm your actions."}
				},
				new FAQViewModel("How to configure the settings?"){
					new FAQClassModel { Description = "To configure the settings, just navigate to the menu section and three categories of settings will be shown. Click which settings you would like to configure and make your desired preferences by either toggling or clicking some buttons. Your changes will be updated and the next time you logged in again, they will be updated in your account so that you don't need to configure them again, unless it's necessary. Visit the tutorial section if you want to know the steps further. "}
				},
				new FAQViewModel("How can the low vision users use Sensate?"){
					new FAQClassModel { Description = "After categorizing the user type and configuring the settings, low vision users are recommended to utilize the recognition mode, while the color enhancement becomes a supplementary only. With the recognition mode, users are provided a camera display or interface in which they have to snap (use the capture button) on a certain object, text, product, or face which they feel like they need some clarifications for or they can't see well. Sensate will then feedback what the item is through audio, therefore, users need to pay attention.  "}
				},
				new FAQViewModel("How can the colorblind users use Sensate?"){
					new FAQClassModel { Description = "After categorizing the user type and configuring the settings, colorblind users are recommended to utilize the color enhancement feature, while the recognition mode becomes a supplementary only. With color enhancement, users are provided a camera display or interface in which they have to look around and focus on a certain scene which they feel like they need some clarifications for when it comes to color recognition. The camera is provided with a filter based on the colorblindness type of the user upon categorization, that enhances the color of a certain scene.  "}
				},
				new FAQViewModel("Can normal vision users use Sensate too?"){
					new FAQClassModel { Description = "Users with normal vision are welcomed to use Sensate for the purposes of simulating the application, understanding the struggles of the visually-impaired users or to check the features of the application before recommending it to someone they know who struggles with low vision or color blindness."}
				},
				new FAQViewModel("What do you mean by sensory-oriented UI?"){
					new FAQClassModel { Description = "Sensory-oriented UI means that Sensate's user interfaces do not only focus on visual or display. Since our target users are people with low vision and colorblindness, we also integrate audio and tactile interfaces. Examples are the audio and vibrational feedback catered in this application. Also, with shortcuts, navigation is more efficient. To learn more about the shortcuts, just visit the tutorial section of Sensate.  "}
				},
				new FAQViewModel("What do you mean by synchronization?"){
					new FAQClassModel { Description = "Synchronization happens when the user made an update or changes on the configuration settings, and these updates are saved under the user's account. There might be instances that the users will migrate to a new mobile device, so instead of manually setting up again, they could just sign-in their account and all changes made on the previous will be also reflected on the new device. "}
				},
				new FAQViewModel("Is it necessary to use internet?"){
					new FAQClassModel { Description = "Internet has been a necessity nowadays and it is true that Sensate relies on it. Sensate utilizes Google Vision API for the recognition mode and an online cloud for account management and configurations synchronization. Without the internet, Sensate's functionalities may not be maximized.   "}
				
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