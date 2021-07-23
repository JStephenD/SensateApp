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
using Xamarin.Essentials;
using SkiaSharp;
using System.IO;
using Firebase.Database;
using Firebase.Database.Query;

namespace Sensate.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestFirestore : ContentPage {
		private SKBitmap bitmap;
		private byte[] imagebytearray;
		private bool istoggled = false;

		private static readonly string firestore_url = @"https://sensatefirebase-a7ef8-default-rtdb.asia-southeast1.firebasedatabase.app/";
		private readonly FirebaseClient firebase = new FirebaseClient(firestore_url);

		private static readonly float[] Protanopia = {
			0.567F, 0.433F,         0F, 0F, 0,
			0.558F, 0.442F,         0F, 0F, 0,
					0F, 0.242F, 0.758F, 0F, 0,
					0F,         0F,        0F, 1F, 0,
		};

		private readonly SKPaint paintProtanopia = new SKPaint {
			ColorFilter = SKColorFilter.CreateColorMatrix(Protanopia),
			Style = SKPaintStyle.Fill
		};

		public TestFirestore() {
			InitializeComponent();

		}

		protected async override void OnAppearing() {
			base.OnAppearing();
			var allPersons = await GetAllPersons();
			lstPersons.ItemsSource = allPersons;

			//await AddSettings("Colorblind", "John", "06/05/2022");
			var allsettings = await GetAllSettings();
			listSettings.ItemsSource = allsettings;
			foreach (var x in allsettings) {
				Console.WriteLine(x);
				Console.WriteLine(x.UserType);
				Console.WriteLine(x.AccountName);
				Console.WriteLine(x.AccountBirthdate);
			}
		}

		public class Person {
			public int PersonId { get; set; }
			public string Name { get; set; }
		}

		public class Settings {
			public string UID { get; set; }
			public string UserType { get; set; }
			public string AccountName { get; set; }
			public string AccountBirthdate { get; set; }
		}

		public async Task<List<Settings>> GetAllSettings() {
			return (await firebase
			  .Child("Settings")
			  .OnceAsync<Settings>()).Select(item => new Settings {
				  UserType = item.Object.UserType,
				  AccountName = item.Object.AccountName,
				  AccountBirthdate = item.Object.AccountBirthdate
			  }).ToList();
		}

		public async Task AddSettings(string usertype, string accountname, string accountbirthdate) {
			await firebase
			  .Child("Settings")
			  .PostAsync(new Settings() {
				  UserType = usertype,
				  AccountName = accountname,
				  AccountBirthdate = accountbirthdate
			  });
		}

		public async Task<List<Person>> GetAllPersons() {
			return (await firebase
			  .Child("Persons")
			  .OnceAsync<Person>()).Select(item => new Person {
				  Name = item.Object.Name,
				  PersonId = item.Object.PersonId
			  }).ToList();
		}

		public async Task AddPerson(int personId, string name) {
			await firebase
			  .Child("Persons")
			  .PostAsync(new Person() { PersonId = personId, Name = name });
		}

		private async void BtnAdd_Clicked(object sender, EventArgs e) {
			await AddPerson(Convert.ToInt32(txtId.Text), txtName.Text);
			txtId.Text = string.Empty;
			txtName.Text = string.Empty;
			await DisplayAlert("Success", "Person Added Successfully", "OK");

			var allPersons = await GetAllPersons();
			lstPersons.ItemsSource = allPersons;
		}

		private async void ImageButton_Clicked(object sender, EventArgs e) {
			await MediaPicker.PickPhotoAsync()
				.ContinueWith(async t => {
					if (t.IsCanceled)
						return;
					var result = t.Result;

					using (MemoryStream ms = new MemoryStream()) {
						using (Stream stream = await result.OpenReadAsync()) {
							stream.CopyTo(ms);
							imagebytearray = ms.ToArray();
							bitmap = SKBitmap.Decode(imagebytearray);
						}
					}

					canvasView.InvalidateSurface();
				});
		}

		private void canvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e) {
			SKImageInfo info = e.Info;
			SKSurface surface = e.Surface;
			SKCanvas canvas = surface.Canvas;

			canvas.Clear(SKColors.White);

			if (bitmap != null) {
				if (istoggled)
					canvas.DrawBitmap(bitmap, info.Rect, BitmapStretch.AspectFit, paint: paintProtanopia);
				else
					canvas.DrawBitmap(bitmap, info.Rect, BitmapStretch.AspectFit);
			}
		}

		private void Switch_Toggled(object sender, ToggledEventArgs e) {
			istoggled = !istoggled;
			canvasView.InvalidateSurface();
		}
	}
}