namespace chatbot_ludo.UIForms.ViewModels
{
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command; //MvvmLightLibsStd10 importar desde los nuget
    using Xamarin.Forms;

    public class LoginViewModel
    {
        //Propiedades para la View de Login
        public string Email { get; set; }
        public string Password { get; set; }
        //Comando para el login Page
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login); //Instanciamos al metodo donde validamos la info de inicio de sesión.
            }
        }

        public LoginViewModel()
        {
            this.Email = "alexis.hernandez074@gmail.com"; //Inicializamos las propiedades de lectura y escritura.
            this.Password = "123456";
        }

        private async void Login() //Inicio de sesion.
        {
            if(string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Ingresa un email","Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Ingresa un Password", "Aceptar");
                return;
            }
            //Condicones para corroborar correo.
            if(!this.Email.Equals("alexis.hernandez074@gmail.com") || !this.Password.Equals("123456"))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Usuario o contraseña incorrecta", "Aceptar");
                return;
            }
            //Salio todo bien.
            await Application.Current.MainPage.DisplayAlert("Ok", "Todo bien", "Aceptar");
        }
    }
}
