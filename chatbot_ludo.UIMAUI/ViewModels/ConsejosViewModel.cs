namespace chatbot_ludo.UIMAUI.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using chatbot_ludo.Common.Models;
    using chatbot_ludo.Common.Services;
    using CommunityToolkit.Mvvm.Input;

    public class ConsejosViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private ObservableCollection<Consejo> consejos;
        private bool isRefreshing; //Es para refrescar

        public ObservableCollection<Consejo> Consejos
        {
            get { return this.consejos; }
            set { this.SetValue(ref this.consejos, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public ICommand RefreshCommand { get; }

        public ConsejosViewModel()
        {
            this.apiService = new ApiService();
            this.RefreshCommand = new RelayCommand(RefreshConsejos);
            this.LoadConsejos(); // Carga inicial de los consejos.
        }

        private async void LoadConsejos()
        {
            this.IsRefreshing = true; // Indica que el refresco está en progreso.
            //Contectamos con el servicio de la API.
            var response = await this.apiService.GetListAsync<Consejo>(
                "https://chatbotludo.azurewebsites.net",
                "/api",
                "/Consejos");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                this.IsRefreshing = false;
                return;
            }

            var myConsejo = (List<Consejo>)response.Result;
            this.Consejos = new ObservableCollection<Consejo>(myConsejo);
            this.IsRefreshing = false; // Detiene el indicador de refresco.
        }

        private void RefreshConsejos()
        {
            this.LoadConsejos(); // Simplemente recargamos los datos.
        }
    }
}

