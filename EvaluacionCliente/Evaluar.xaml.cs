using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EvaluacionCliente.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;

namespace EvaluacionCliente
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	
	public partial class Evaluar : ContentPage
	{
		Dispositivo o_dispositivo = new Dispositivo();
		List<Dispositivo> listaDispositivos;

		public Evaluar()
		{
			InitializeComponent();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			var lista = new List<Evaluacion>();
			lista = await App.Database.ObtenerEvaluaciones().ConfigureAwait(true);
			listaDispositivos = await App.Database.ObtenerDispositivo().ConfigureAwait(true);
			if (listaDispositivos.Count > 0)
			{
				o_dispositivo = (from tab in listaDispositivos
								 select tab).FirstOrDefault();
			}
		}

		async void BtnBien_OnClick(object sender, EventArgs args)
		{
			await App.Database.GuardarEvaluacion(new Evaluacion
			{
				evaluacion = 1,
				fecha_evaluacion = DateTime.Now,
				device_name = o_dispositivo.nombre,
			}).ConfigureAwait(true);
			var loadingPage = new CustomGIFLoader();
			await PopupNavigation.PushAsync(loadingPage).ConfigureAwait(true);
			await Task.Delay(1000).ConfigureAwait(true);
			await PopupNavigation.RemovePageAsync(loadingPage).ConfigureAwait(true);
		}

		async void BtnMedio_OnClick(object sender, EventArgs args)
		{
			await App.Database.GuardarEvaluacion(new Evaluacion
			{
				evaluacion = 2,
				fecha_evaluacion = DateTime.Now,
				device_name = o_dispositivo.nombre,
			}).ConfigureAwait(true);
			var loadingPage = new CustomGIFLoader();
			await PopupNavigation.PushAsync(loadingPage).ConfigureAwait(true);
			await Task.Delay(1000).ConfigureAwait(true);
			await PopupNavigation.RemovePageAsync(loadingPage).ConfigureAwait(true);
		}

		async void BtnMalo_OnClick(object sender, EventArgs args)
		{
			await App.Database.GuardarEvaluacion(new Evaluacion
			{
				evaluacion = 3,
				fecha_evaluacion = DateTime.Now,
				device_name = o_dispositivo.nombre,
			}).ConfigureAwait(true);
			var loadingPage = new CustomGIFLoader();
			await PopupNavigation.PushAsync(loadingPage).ConfigureAwait(true);
			await Task.Delay(1000).ConfigureAwait(true);
			await PopupNavigation.RemovePageAsync(loadingPage).ConfigureAwait(true);
		}
	}
}	