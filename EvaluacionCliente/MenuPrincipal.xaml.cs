using EvaluacionCliente.Models;
using RestSharp;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EvaluacionCliente
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPrincipal : ContentPage
	{
		List<Acceso> lista = new List<Acceso>();
		public MenuPrincipal()
		{
			InitializeComponent();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			
			lista = await App.Database.ObtenerAccesos().ConfigureAwait(true);
		}

		private void Evaluacion_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Evaluar());
		}

		private async void Datos_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new Login()).ConfigureAwait(true);
			//string usuario = await DisplayPromptAsync("Acceso", "Ingrese nombre de usuario").ConfigureAwait(true);
			//if (usuario != null)
			//{
			//	string clave = await DisplayPromptAsync("Acceso", "Ingrese contraseña").ConfigureAwait(true);
			//	var cliente = new RestClient("http://192.168.11.91:8000");
			//	var peticion = new RestRequest("accesos/", Method.GET);
			//	peticion.AddParameter("usuario", usuario);
			//	peticion.AddParameter("clave", clave);
			//	IRestResponse<List<Acceso>> respuestaServer = cliente.Execute<List<Acceso>>(peticion);
			//	if (respuestaServer.Data.Count == 1)
			//	{
			//		await Navigation.PushAsync(new DatosMenu()).ConfigureAwait(true);
			//	}
			//	else if (respuestaServer.Data.Count == 0)
			//	{
			//		await DisplayAlert("Datos de acceso", "No hay datos de acceso, revise si el usuario existe o si el usuario y contraseña ingresado son correctos", "Aceptar").ConfigureAwait(true);
			//	}
			//}
		}
	}
}