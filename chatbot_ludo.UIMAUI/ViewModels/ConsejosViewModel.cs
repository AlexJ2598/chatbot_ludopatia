namespace chatbot_ludo.UIMAUI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using chatbot_ludo.Common.Models; //Importamos desde nuestro Common ya que allí tenemos definido nuestro Modelo de Consejo
    using chatbot_ludo.Common.Services;

    public class ConsejosViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private ObservableCollection<Consejo> consejos;
        public ObservableCollection<Consejo> Consejos //Para que los cambios se vean reflejados en la View.
        {
            get { return this.consejos; }
            set { this.SetValue(ref this.consejos, value); }
        }


        public ConsejosViewModel()
        {
            this.apiService = new ApiService(); //Instanciamos con la API service,
            this.LoadConsejos(); //Es async, ya que no cargan de una, dependen del consumo de la API.
        }

        private async void LoadConsejos()
        {
            var response = await this.apiService.GetListAsync<Consejo>(
                "https://chatbotludo.azurewebsites.net",
                "/api",
                "/Consejos");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error",response.Message,"Accept");
                return;
            }
            //Devuelve la lista
            var myConsejo = (List<Consejo>)response.Result; //Devolvemos la lista usando los modelos de la clase Response.
            //Armamos la Observable
            this.Consejos = new ObservableCollection<Consejo>(myConsejo);

        }
    }
}
