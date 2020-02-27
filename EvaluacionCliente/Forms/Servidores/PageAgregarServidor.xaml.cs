using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EvaluacionCliente.Forms.Servidores
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageAgregarServidor : ContentPage
	{
		public PageAgregarServidor()
		{
			InitializeComponent();
		}

		public void BtnGuardarServidorClicked(object sender, EventArgs e)
		{
			try
			{
				DisplayAlert("Datos de acceso", AppResources.GuardaRegistro, "Aceptar");
			}
			catch (Exception ex)
			{
				DisplayAlert("Datos de acceso", ex.Message, "Aceptar");
			}
		}
	}
}