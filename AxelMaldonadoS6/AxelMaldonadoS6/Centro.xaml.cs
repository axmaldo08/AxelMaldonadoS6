using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AxelMaldonadoS6
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Centro : ContentPage
    {
        private const string Uri = "http://afgrupodos.eastus2.cloudapp.azure.com/AppFinalGrupoDos/centro.php";
        private readonly HttpClient client = new HttpClient();
        private ObservableCollection<AxelMaldonadoS6.WS.centro> _post;
        public Centro()
        {
            InitializeComponent();
            // Traer los datos de los Centros
            get();
        }
        public async void get()
        {
            try
            {
                var content = await client.GetStringAsync(Uri);
                List<AxelMaldonadoS6.WS.centro> posts = JsonConvert.DeserializeObject<List<AxelMaldonadoS6.WS.centro>>(content);
                _post = new ObservableCollection<AxelMaldonadoS6.WS.centro>(posts);

                listView.ItemsSource = _post;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Mensaje de alerta", ex.Message, "OK");
            }
        }

        private async void btnInsertar_Clicked(object sender, EventArgs e)
        {

            //Pasa a la pantalla de insertar
            await Navigation.PushAsync(new CentroInsertar());
        }

        private async void MenuItem_Actualizar(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var centro = menuItem.CommandParameter as AxelMaldonadoS6.WS.centro;

            await Navigation.PushAsync(new CentroActualizar(centro.idCentro, centro.nombreCentro, centro.sectorCentro, centro.direccionCentro, centro.telefonoCentro, centro.estadoCentro));
        }

        private async void MenuItem_Eliminar(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var centro = menuItem.CommandParameter as AxelMaldonadoS6.WS.centro;

            await Navigation.PushAsync(new CentroEliminar(centro.idCentro, centro.nombreCentro, centro.sectorCentro, centro.direccionCentro, centro.telefonoCentro, centro.estadoCentro));
        }
    }
}