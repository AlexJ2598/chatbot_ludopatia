namespace chatbot_ludo.UIMAUI.ViewModels
{
    using System.Windows.Input;
    using chatbot_ludo.UIMAUI.Views;
    using CommunityToolkit.Mvvm.ComponentModel; //CommunityToolkit.Mvvm instalar en lugar del Mvvm.Light.Libs.Std
    using CommunityToolkit.Mvvm.Input;

    public class LoginViewModel : ObservableObject
    {
        private string email;
        private string password;

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value); // Asegura que se notifiquen los cambios de propiedad.
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value); // Asegura que se notifiquen los cambios de propiedad.
        }

        public ICommand LoginCommand => new RelayCommand(Login); //El Relay más bonito.

        public LoginViewModel()
        {
            Email = "alexis.hernandez074@gmail.com"; // Valores predeterminados para prueba.
            Password = "123456";
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter an email...", "Accept");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You must enter a password...", "Accept");
                return;
            }
            if(!this.Email.Equals("alexis.hernandez074@gmail.com") || !this.Password.Equals("123456"))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email or Password Incorrect.", "Accept");
                return;
            }
            //await Application.Current.MainPage.DisplayAlert("Ok", "Todo correcto", "Accept");
            //Cada que tiremos la Pagina instanciamos la ViewModel, así como ligarla. Se modifica MainViewModel para poder instanciar.
            MainViewModel.GetInstance().Consejos = new ConsejosViewModel(); //Con esto cargamos los objetos en memoria
            await Application.Current.MainPage.Navigation.PushAsync(new ConsejosPage()); //Apilamos la Pagina. 
        }
    }
}
