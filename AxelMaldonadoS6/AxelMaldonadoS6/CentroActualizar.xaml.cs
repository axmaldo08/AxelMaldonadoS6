using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AxelMaldonadoS6
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CentroActualizar : ContentPage
    {
        public int estado;
        public CentroActualizar(int idCentro, string nombreCentro, string sectorCentro, string direccionCentro, string telefonoCentro, int estadoCentro)
        {
            InitializeComponent();
            //Llenar los campos con los valores actuales
            lblId.Text = idCentro.ToString();
            txtNombre.Text = nombreCentro;
            pckSector.SelectedItem = sectorCentro;
            txtDireccion.Text = direccionCentro;
            txtTelefono.Text = telefonoCentro; 

            if (estadoCentro == 1)
            {
                swtEstado.IsToggled = true;
            }
            else
            {
                swtEstado.IsToggled = false;
            }
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
                        HttpClient client = new HttpClient();

                        //Actualizar con PUT
                        var response = client.PutAsync(String.Format("http://afgrupodos.eastus2.cloudapp.azure.com/AppFinalGrupoDos/centro.php?idCentro={0}&nombreCentro={1}&sectorCentro={2}&direccionCentro={3}&telefonoCentro={4}&estadoCentro={5}", lblId.Text, txtNombre.Text, pckSector.SelectedItem, txtDireccion.Text, txtTelefono.Text, estado), null).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var resultString = response.Content.ReadAsStringAsync().Result;
                        }
                        else
                        {
                            await DisplayAlert("Mensaje de alerta", "Error al actualizar", "OK");
                        }

                        // Utilizar TOAST
                        var mensaje = "Actualizado correctamente";
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