﻿using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Auth;
using Firebase.Database;
using Xamarin.Essentials;
using System.Linq;
using Firebase.Database.Query;
using System.Threading.Tasks;
using Sensate.Models;

namespace Sensate.Views {
	class SyncHelper {

		public static string FirebaseAPIKey = "AIzaSyCCVSTuyUOF9KVv6fZJywVHckk4PttsUw8";
		public static FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAPIKey));

		private static string firestore_url = @"https://sensatefirebase-a7ef8-default-rtdb.asia-southeast1.firebasedatabase.app/";
		private static FirebaseClient firebase = new FirebaseClient(firestore_url);

		private static string UID = Preferences.Get("UID", "");

		public static async Task UploadSettings() {
			var toupdateuser = (await firebase
				.Child("Users")
				.OnceAsync<Users>()).Where(a => a.Object.UID == UID).FirstOrDefault();

			var toupdatesettings = (await firebase
				.Child("Settings")
				.OnceAsync<Settings>()).Where(a => a.Object.UID == UID).FirstOrDefault();

			await firebase
			  .Child("Settings")
			  .Child(toupdateuser.Key)
			  .PutAsync(new Users {
				  UID = Preferences.Get("UID", ""),
				  Name = Preferences.Get("AccountName", "", "UserAccount"),
				  Birthdate = Preferences.Get("AccountBirthdate", "", "UserAccount"),
				  Sex = Preferences.Get("AccountGender", "", "UserAccount"),
			  });

			await firebase
			  .Child("Settings")
			  .Child(toupdatesettings.Key)
			  .PutAsync(new Settings {
				  UID = Preferences.Get("UID", ""),

				  UserCategory = Preferences.Get("UserCategory", "Default", "GeneralSettings"),

				  CBType = Preferences.Get("CBType", "Default", "CBSettings"),

				  LVCause = Preferences.Get("LVCause", "Default", "LVSettings"),
				  LVSeverity = Preferences.Get("LVSeverity", "Default", "LVSettings"),

				  AssistanceLevel = Preferences.Get("AssistanceLevel", "Default", "GeneralSettings"),

				  AudioFeedback = Preferences.Get("AudioFeedback", false, "GeneralSettings"),
				  VibrationFeedback = Preferences.Get("VibrationFeedback", false, "GeneralSettings"),

				  BoldText = Preferences.Get("BoldText", false, "GeneralSettings"),
				  NightMode = Preferences.Get("NightMode", false, "GeneralSettings"),

				  Shortcuts = Preferences.Get("Shortcuts", false, "GeneralSettings"),
				  HardwareButtons = Preferences.Get("HardwareButtons", false, "GeneralSettings"),
				  Gesture = Preferences.Get("Gesture", false, "GeneralSettings"),
			  });
		}

		public static async Task LoadSettings() {
			Console.WriteLine("loadsettings");
			Console.WriteLine(Preferences.Get("UID", ""));
			var uid = Preferences.Get("UID", "");

			// load user configs
			var user = await GetUsers(UID);
			Preferences.Set("AccountName", user.Name, "UserAccount");
			Preferences.Set("AccountGender", user.Sex, "UserAccount");
			Preferences.Set("AccountBirthdate", user.Birthdate, "UserAccount");

			// load application settings
			var settings = (await firebase
				.Child("Settings")
				.OnceAsync<Settings>()).FirstOrDefault(a => a.Object.UID == UID);
			var so = settings.Object;

			Console.WriteLine(new object[]{so.UserCategory, so.CBType, so.AudioFeedback, so.Shortcuts});

			// Category 
			Preferences.Set("UserCategory", so.UserCategory, "GeneralSettings");

			// CB Settings
			Preferences.Set("CBType", so.CBType, "CBSettings");

			// LV Settings
			Preferences.Set("LVCause", so.LVCause, "LVSettings");
			Preferences.Set("LVSeverity", so.LVSeverity, "LVSettings");

			// Assistance
			Preferences.Set("AssistanceLevel", so.AssistanceLevel, "GeneralSettings");

			// Display Settings
			Preferences.Set("BoldText", so.BoldText, "GeneralSettings");
			Preferences.Set("NightMode", so.NightMode, "GeneralSettings");

			// Feedback Settings
			Preferences.Set("AudioFeedback", so.AudioFeedback, "GeneralSettings");
			Preferences.Set("VibrationFeedback", so.VibrationFeedback, "GeneralSettings");

			// Navigation
			Preferences.Set("Shortcuts", so.Shortcuts, "GeneralSettings");
			Preferences.Set("HardwareButtons", so.HardwareButtons, "GeneralSettings");
			Preferences.Set("Gesture", so.Gesture, "GeneralSettings");
		}

		public static async Task<List<Users>> GetAllUsers() {
			return (await firebase
				.Child("Users")
				.OnceAsync<Users>())
					.Select(item => new Users{ 
						UID = item.Object.UID,
						Name = item.Object.Name,
						Birthdate = item.Object.Birthdate,
						Age = item.Object.Age,
						Sex = item.Object.Sex,
					}).ToList();
		}

		public static async Task<Users> GetUsers(string UID) {
			var user = (await firebase
				.Child("Users")
				.OnceAsync<Users>()).Where(a => a.Object.UID == UID).FirstOrDefault();
			var uo = user.Object;

			return new Users { 
				UID = uo.UID,
				Age = uo.Age,
				Birthdate = uo.Birthdate,
				Name = uo.Name,
				Sex = uo.Sex,
			};
		}

		public static async Task<List<Settings>> GetAllSettings() {
			return (await firebase
				.Child("Users")
				.OnceAsync<Settings>())
					.Select(item => new Settings {
						UID = item.Object.UID,

						UserCategory = item.Object.UserCategory,

						CBType = item.Object.CBType,

						LVCause = item.Object.LVCause,
						LVSeverity = item.Object.LVSeverity,

						AssistanceLevel = item.Object.AssistanceLevel,

						AudioFeedback = item.Object.AudioFeedback,
						VibrationFeedback = item.Object.VibrationFeedback,

						Shortcuts = item.Object.Shortcuts,
						HardwareButtons = item.Object.HardwareButtons,
						Gesture = item.Object.Gesture,
					}).ToList();
		}

		public static async Task<Settings> GetSettings(string UID) {
			var allsettings = await GetAllSettings();
			return allsettings.Where(a => a.UID == UID).FirstOrDefault();
		}

		public class Settings { 
			public string UID { get; set; }

			public string UserCategory { get; set; }
			public string CBType { get; set; }

			public string LVCause { get; set; }
			public string LVSeverity { get; set; }

			public string AssistanceLevel { get; set; }

			public bool AudioFeedback { get; set; }
			public bool VibrationFeedback { get; set; }

			public bool BoldText { get; set; }
			public bool NightMode { get; set; }

			public bool Shortcuts { get; set; }
			public bool HardwareButtons { get; set; }
			public bool Gesture { get; set; }
		}
	}	
}
