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
using Syncfusion.XlsIO;
using System.IO;
using System.Windows.Input;
using EvaluacionCliente.Services;
using Xamarin.Essentials;

namespace EvaluacionCliente
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DatosMenu : ContentPage
	{
		public ICommand ExportToExcelCommand { private set; get; }
		private ExcelService excelService;


		Dispositivo o_dispositivo = new Dispositivo();
		List<Dispositivo> listaDispositivos;
		Sucursal o_sucursal = new Sucursal();
		List<Sucursal> listaSucursales;

		public DatosMenu()
		{
			InitializeComponent();
			ExportToExcelCommand = new Command(async () => await ExportToExcel());
			excelService = new ExcelService();
		}

		async Task ExportToExcel()
		{
			

		}

		protected async override void OnAppearing()
		{
			texto.Text = "";
			base.OnAppearing();
			o_dispositivo.id = 0;
			o_sucursal.id = 0;
			var lista = new List<Evaluacion>();
			lista = await App.Database.ObtenerEvaluaciones().ConfigureAwait(true);
			texto.Text += " " + lista.Count;
			CargarDatos();
		}

		private async void SubirDatos_Clicked(object sender, EventArgs e)
		{
			try
			{
				if ((o_dispositivo.id == 0 || o_dispositivo.nombre.Length == 0))
				{
					await DisplayAlert("Mensaje", AppResources.SinNombre, "OK").ConfigureAwait(true);
					return;
				}
				if ((o_sucursal.id == 0 || o_sucursal.sucursal.Length == 0))
				{
					await DisplayAlert("Mensaje", AppResources.SinSucursal, "OK").ConfigureAwait(true);
					return;
				}
				List<Evaluacion> lista = new List<Evaluacion>();
				lista = await App.Database.ObtenerEvaluaciones().ConfigureAwait(false);
				var jsondatos = JsonConvert.SerializeObject(new { datos = lista });
				if (lista.Count > 0)
				{
					var cliente = new RestClient(Globales.Servidor + "sincronizar/");
					//var cliente = new RestClient("http://it01.local:8004/sincronizar/");
					var request = new RestRequest();
					request.AddParameter("datos", jsondatos);
					var response = cliente.Post(request);
					if (response.IsSuccessful)
					{
						await DisplayAlert("Mensaje", AppResources.FinalizadoCorrectamente, "OK").ConfigureAwait(true);
					}
					else
					{
						await DisplayAlert("Mensaje", AppResources.NoFinalizaProceso, "OK").ConfigureAwait(true);
					}
				}
				else
				{
					await DisplayAlert("Mensaje", AppResources.NoDatosSincroniza, "OK").ConfigureAwait(true);
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Mensaje", ex.Message, "OK").ConfigureAwait(true);
			}
		}

		private async void GuardarNombre_Clicked(object sender, EventArgs e)
		{
			try
			{
				var nombreDispositivo = await DisplayPromptAsync("Datos", AppResources.NombreDispositivo).ConfigureAwait(true);
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
						await DisplayAlert("Mensaje", AppResources.FinalizadoCorrectamente, "OK").ConfigureAwait(true);
						CargarDatos();
					}
					else
					{
						await DisplayAlert("Mensaje", AppResources.NombreDispositivoVacio, "OK").ConfigureAwait(true);
					}
				}
				else
				{
					await DisplayAlert("Mensaje", AppResources.NombreDispositivoVacio, "OK").ConfigureAwait(true);
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Mensaje", ex.Message, "OK").ConfigureAwait(true);
			}
		}

		private async void GuardarSucursal_Clicked(object sender, EventArgs e)
		{
			try
			{
				var nombreSucursal = await DisplayPromptAsync("Datos", AppResources.Sucursal).ConfigureAwait(true);
				if (nombreSucursal != null)
				{
					if (nombreSucursal.Length > 0)
					{
						o_sucursal.sucursal = nombreSucursal;
						if (o_sucursal.id > 0)
						{
							await App.Database.ActualizarSucursal(o_sucursal).ConfigureAwait(true);
						}
						else
						{
							await App.Database.GuardarSucursal(o_sucursal).ConfigureAwait(true);
						}
						await DisplayAlert("Mensaje", AppResources.FinalizadoCorrectamente, "OK").ConfigureAwait(true);
						CargarDatos();
					}
					else
					{
						await DisplayAlert("Mensaje", AppResources.SucursalVacia, "OK").ConfigureAwait(true);
					}
				}
				else
				{
					await DisplayAlert("Mensaje", AppResources.SucursalVacia, "OK").ConfigureAwait(true);
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
				await DisplayAlert("Mensaje", AppResources.DatosLimpios, "OK").ConfigureAwait(true);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Mensaje", ex.Message, "OK").ConfigureAwait(true);
			}
		}

		private async void btnExportarDatos(object sender, EventArgs e)
		{
			try
			{
				List<Evaluacion> lista = new List<Evaluacion>();
				lista = await App.Database.ObtenerEvaluaciones().ConfigureAwait(false);

				var fileName = $"{Guid.NewGuid()}.xlsx";
				string filePath = excelService.GenerateExcel(fileName);

				var header = new List<string>() { "Id", "Evaluacion", "Fecha_Evaluacion", "Device_Name" };

				var data = new ExcelData();
				data.Headers = header;

				foreach (var item in lista)
				{
					var row = new List<string>()
				{
					item.id.ToString(),
					item.evaluacion.ToString(),
					item.fecha_evaluacion.ToString(),
					item.device_name.ToString(),
				};
					data.Values.Add(row);
				}

				excelService.InsertDataIntoSheet(filePath, "Publications", data);
				await Launcher.OpenAsync(new OpenFileRequest()
				{
					File = new ReadOnlyFile(filePath)
				});
			}
			catch (Exception ex)
			{
				await DisplayAlert("Mensaje", ex.Message, "OK").ConfigureAwait(true);
			}
		}

		private async void CargarDatos()
		{
			listaDispositivos = await App.Database.ObtenerDispositivo().ConfigureAwait(true);
			listaSucursales = await App.Database.ObtenerSucursal().ConfigureAwait(true);
			if (listaDispositivos.Count > 0)
			{
				o_dispositivo = (from tab in listaDispositivos
								 select tab).FirstOrDefault();
				lblNombreDispositivo.Text = o_dispositivo.nombre;
			}

			if (listaSucursales.Count > 0)
			{
				o_sucursal = (from tab in listaSucursales
							  select tab).FirstOrDefault();
				lblSucursal.Text = o_sucursal.sucursal;
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