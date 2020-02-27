using EvaluacionCliente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Plugin.DeviceInfo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using RestSharp;
using EvaluacionCliente.Forms.Servidores;

namespace EvaluacionCliente
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DatosMenu : ContentPage
	{
		Dispositivo o_dispositivo = new Dispositivo();
		List<Dispositivo> listaDispositivos;
		public DatosMenu()
		{
			InitializeComponent();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			o_dispositivo.id = 0;
			var lista = new List<Evaluacion>();
			lista = await App.Database.ObtenerEvaluaciones().ConfigureAwait(true);
			texto.Text += " " + lista.Count;
			CargarDatos();
		}

		private async void SubirDatos_Clicked(object sender, EventArgs e)
		{
			if (o_dispositivo.id == 0 || o_dispositivo.nombre.Length == 0)
			{
				await DisplayAlert("Mensaje", "El dispositivo no tiene nombre asignado", "OK").ConfigureAwait(true);
				return;
			}
			List<Evaluacion> lista = new List<Evaluacion>();
			lista = await App.Database.ObtenerEvaluaciones().ConfigureAwait(false);
			var jsondatos = JsonConvert.SerializeObject(new { datos = lista});
			if (lista.Count > 0)
			{
				var cliente = new RestClient("http://192.168.11.91:8004/sincronizar/");
				//var cliente = new RestClient("http://it01.local:8004/sincronizar/");
				var request = new RestRequest();
				request.AddParameter("datos", jsondatos);
				var response = cliente.Post(request);
				if (response.IsSuccessful)
				{
					await DisplayAlert("Mensaje", "Se subieron los datos al servidor", "OK").ConfigureAwait(true);
				}
				else
				{
					await DisplayAlert("Mensaje", "El proceso no finalizó correctamente", "OK").ConfigureAwait(true);
				}
			}
			else
			{
				await DisplayAlert("Mensaje", "No hay datos para sincronizar", "OK").ConfigureAwait(true);
			}
		}

		private async void GuardarNombre_Clicked(object sender, EventArgs e)
		{
			try
			{
				var nombreDispositivo = await DisplayPromptAsync("Datos", "Ingrese nombre de dispositivo").ConfigureAwait(true);
				if (nombreDispositivo != null)
				{
					if (nombreDispositivo.Length > 0)
					{
						o_dispositivo.nombre = nombreDispositivo;
						if (o_dispositivo.id > 0)
						{
							await App.Database.ActualizarDispositivo(o_dispositivo).ConfigureAwait(true);
						}
						else
						{
							await App.Database.GuardarDispositivo(o_dispositivo).ConfigureAwait(true);
						}
						await DisplayAlert("Mensaje", "Se han guardardo los datos", "OK").ConfigureAwait(true);
						CargarDatos();
					}
					else
					{
						await DisplayAlert("Mensaje", "El nombre de dispositivo no puede quedar vacío", "OK").ConfigureAwait(true);
					}
				}
				else
				{
					await DisplayAlert("Mensaje", "El nombre de dispositivo no puede quedar vacío", "OK").ConfigureAwait(true);
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Mensaje", ex.Message, "OK").ConfigureAwait(true);
			}
		}

		private async void BorrarDatos_Clicked(object sender, EventArgs e)
		{
			try
			{
				await App.Database.EliminarEvaluaciones().ConfigureAwait(true);
				var lista = new List<Evaluacion>();
				lista = await App.Database.ObtenerEvaluaciones().ConfigureAwait(true);
				texto.Text = "Total registros: " + lista.Count;
				await DisplayAlert("Mensaje", "Se han limpiado los datos", "OK").ConfigureAwait(true);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Mensaje", ex.Message, "OK").ConfigureAwait(true);
			}
		}

		private async void CargarDatos()
		{
			listaDispositivos = await App.Database.ObtenerDispositivo().ConfigureAwait(true);
			if (listaDispositivos.Count > 0)
			{
				o_dispositivo = (from tab in listaDispositivos
								 select tab).FirstOrDefault();
				lblNombreDispositivo.Text = o_dispositivo.nombre;
			}
		}

		private async void btnServidores_Clicked(object sender, EventArgs e)
		{
			try
			{
				await Navigation.PushAsync(new LVPServidores()).ConfigureAwait(true);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", ex.Message, "OK").ConfigureAwait(true);
			}
		}
	}
}