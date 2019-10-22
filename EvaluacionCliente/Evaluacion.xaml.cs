using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EvaluacionCliente
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Evaluacion : ContentPage
	{
		int clicktotal;
		public Evaluacion()
		{
			InitializeComponent();
		}

		private void BtnBien_OnClick(object sender, EventArgs args)
		{
			texto.Text = "¡Gracias por visitarnos!";
			ThreadStart delegado = new ThreadStart(EnviarDato);
			Thread hilo = new Thread(delegado);
			hilo.Start();

			int valor = 0;

			while (valor == 0)
			{
				if (hilo.IsAlive != true)
				{
					valor += 1;
					delegado = new ThreadStart(LimpiarMensaje);
					Thread hilo2 = new Thread(delegado);
					hilo2.Start();
				}
			}
		}

		private void EnviarDato()
		{
			try
			{
				const string url = "http://192.168.11.71:8000/evaluaciones/";
				HttpClient _client = new HttpClient();
				var formContent = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("evaluacion", "1"),
				});
				//var content = JsonConvert.SerializeObject(post);
				HttpResponseMessage resp = _client.PostAsync(url, formContent).Result;
			}
			catch (Exception ex)
			{
				texto.Text = ex.Message;
			}
		}

		private void LimpiarMensaje()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Thread.Sleep(8000);
				texto.Text = null;
			});
		}
	}
	public class Enviar
	{
		public int evaluacion { get; set; }
	}
}

