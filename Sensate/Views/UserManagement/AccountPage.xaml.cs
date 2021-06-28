﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sensate.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Sensate.Models;
using System.Windows.Input;
using System.IO;
using Firebase.Auth;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountPage : ContentPage {

		public AccountPage() {
			InitializeComponent();

			#region gesturerecognizers
			var synchronizeframeclick = new TapGestureRecognizer();
			synchronizeframeclick.Tapped += SynchronizeFrameClick;
			var editaccountframeclick = new TapGestureRecognizer();
			editaccountframeclick.Tapped += EditAccountFrameClick;
			var logoutframeclick = new TapGestureRecognizer();
			logoutframeclick.Tapped += LogoutFrameClick;
			synchronizeFrame.GestureRecognizers.Add(synchronizeframeclick);
			editprofileFrame.GestureRecognizers.Add(editaccountframeclick);
			logoutframe.GestureRecognizers.Add(logoutframeclick);

			var hamburgerclick = new TapGestureRecognizer();
			hamburgerclick.Tapped += HamburgerClick;
			hamburger.GestureRecognizers.Add(hamburgerclick);
			#endregion gesturerecognizers
		}

		#region gesturerecognizer functions
		public void SynchronizeFrameClick(object s, EventArgs e) {
			Console.WriteLine("a");
		}
		public void EditAccountFrameClick(object s, EventArgs e) {
			Console.WriteLine("b");
		}
		public void LogoutFrameClick(object s, EventArgs e) {
			Console.WriteLine("c");
		}
		public void HamburgerClick(object s, EventArgs e) {
			Shell.Current.FlyoutIsPresented = true;
		}
		#endregion gesturerecognizer functions
	}
}