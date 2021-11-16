using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AxelMaldonadoS6
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CentroInsertar : ContentPage
    {
        public int estado;
        public CentroInsertar()
        {
            InitializeComponent();
        }

        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (pckSector.SelectedIndex == -1)
                {
                    await DisplayAlert("Mensaje de alerta", "Seleccione un sector", "OK");
                }
                else
                {
                    if (swtEstado.IsToggled == true)
                    {
                        estado = 1;
                    }
                    else
                    {
                        estado = 0;
                    }

                    if (Convert.ToInt32(txtTelefono.Text) >= 0 && txtTelefono.Text.Length >= 7 && txtTelefono.Text.Length <= 10)
                    {
                        WebClient client = new WebClient();

                        var parametros = new System.Collections.Specialized.NameValueCollection();

                        parametros.Add("nombreCentro", txtNombre.Text);
                        parametros.Add("sectorCentro", pckSector.SelectedItem.ToString());
                        parametros.Add("direccionCentro", txtDireccion.Text);
                        parametros.Add("telefonoCentro", txtTelefono.Text);
                        parametros.Add("estadoCentro", estado.ToString());

                        client.UploadValues("http://afgrupodos.eastus2.cloudapp.azure.com/AppFinalGrupoDos/centro.php", "POST", parametros);

                        // Utilizar TOAST
                        var mensaje = "Guardado correctamente";
                        DependencyService.Get<Mensaje>().longAlert(mensaje);

                        //Poner en blanco los campos
                        txtNombre.Text = "";
                        pckSector.SelectedIndex = -1;
                        txtDireccion.Text = "";
                        txtTelefono.Text = "";
                        swtEstado.IsToggled = true;

                        //Regresar a la pantalla de Centros de vacunación
                        await Navigation.PushAsync(new Centro());
                    }
                    else
                    {
                        await DisplayAlert("Mensaje de alerta", "Teléfono inválido", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Mensaje de alerta", ex.Message, "OK");
            }

        }
    }
}