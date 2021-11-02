using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EvaluacionCliente
{
	public partial class App : Application
	{
		static Database database;
		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new MenuPrincipal());
		}

		public static Database Database
		{
			get
			{
				if (database == null)
				{
					database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Evaluaciones.db3"));
				}
				return database;
			}
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
