using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AxelMaldonadoS6
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CentroEliminar : ContentPage
    {
        public CentroEliminar(int idCentro, string nombreCentro, string sectorCentro, string direccionCentro, string telefonoCentro, int estadoCentro)
        {
            InitializeComponent();
            //Llenar los campos con los valores actuales
            lblId.Text = idCentro.ToString();
            lblNombre.Text = nombreCentro;
            lblSector.Text = sectorCentro; 
            lblDireccion.Text = direccionCentro;
            lblTelefono.Text = telefonoCentro;

            if (estadoCentro == 1)
            {
                swtEstado.IsToggled = true;
            }
            else
            {
                swtEstado.IsToggled = false;
            }
        }

        private async void btnEliminar_Clicked(object sender, EventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();

                //Eliminar por ID
                var response = client.DeleteAsync(String.Format("http://afgrupodos.eastus2.cloudapp.azure.com/AppFinalGrupoDos/centro.php?idCentro={0}", lblId.Text)).Result;

                if (response.IsSuccessStatusCode)
                {
                    var resultString = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    await DisplayAlert("Mensaje de alerta", "Error al eliminar", "OK");
                }

                //Utilizar TOAST
                var mensaje = "Eliminado correctamente";
                
                DependencyService.Get<Mensaje>().longAlert(mensaje);

                //Poner en blanco los campos
                lblNombre.Text = "";
                lblSector.Text = "";
                lblDireccion.Text = "";
                lblTelefono.Text = "";
                swtEstado.IsToggled = true;

                //Regresar a la pantalla de Vacunas
                await Navigation.PushAsync(new Centro());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Mensaje de alerta", ex.Message, "OK");
            }
        }
    }
}