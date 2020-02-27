using EvaluacionCliente.Models;
using RestSharp;
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
	public partial class Login : ContentPage
	{
		public Login()
		{
			InitializeComponent();
		}

		private async void BtnAceptar_Clicked(object sender, EventArgs e)
		{
			try
			{
				string usuario = txtusuario.Text;
				string clave = txtclave.Text;
				btnAceptar.IsEnabled = false;
				if (usuario != null)
				{
					var cliente = new RestClient("http://192.168.11.91:8004");
					//var cliente = new RestClient("http://it01:8004");
					var peticion = new RestRequest("accesos/", Method.GET);
					peticion.AddParameter("usuario", usuario);
					peticion.AddParameter("clave", clave);
					IRestResponse<List<Acceso>> respuestaServer = cliente.Execute<List<Acceso>>(peticion);
					if (respuestaServer.Data.Count == 1)
					{
						await Navigation.PushAsync(new DatosMenu()).ConfigureAwait(true);
					}
					else if (respuestaServer.Data.Count == 0)
					{
						await DisplayAlert("Datos de acceso", "No hay datos de acceso, revise si el usuario existe o si el usuario y contraseña ingresado son correctos", "Aceptar").ConfigureAwait(true);
						btnAceptar.IsEnabled = true;
					}
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Datos de acceso", ex.Message, "Aceptar").ConfigureAwait(true);
				btnAceptar.IsEnabled = true;
			}
		}
	}
}