using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Sensate.Views;
using Xamarin.Forms;
using Sensate.Models;
using Firebase.Database;
using Firebase.Database.Query;

namespace Sensate.ViewModels {
	public class QuickProfileSetupViewModel : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;
		public QuickProfileSetupViewModel() {}

		private string name;
		private string sex;
		private int age;

		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				PropertyChanged(this, new PropertyChangedEventArgs("Name"));
			}
		}

		public string Sex
		{
			get { return sex; }
			set
			{
				sex = value;
				PropertyChanged(this, new PropertyChangedEventArgs("Sex"));
			}
		}

		public int Age
		{
			get { return age; }
			set
			{
				age = value;
				PropertyChanged(this, new PropertyChangedEventArgs("Age"));
			}
		}


		public Command ConfirmCommand
		{
			get { return new Command(OnConfirmCommand); }
		}
		private async void OnConfirmCommand()
		{
			//-----insert to database-----
			FirebaseClient firebaseClient = new FirebaseClient("https://sensatefirebase-a7ef8-default-rtdb.asia-southeast1.firebasedatabase.app/");
			await firebaseClient.Child("Users").PostAsync(new Users { Name = name, Age = age, Sex = sex });
			
			//go to personalization 
		}
	}
}