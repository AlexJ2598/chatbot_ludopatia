namespace chatbot_ludo.UIForms.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    //Esta tiene como objetivo controlar las demás view models-
    public class MainViewModel
    {
        public LoginViewModel Login { get; set; }

        //Instanciamos de momento, luego cambiamos, mala practica.
        public MainViewModel()
        {
            this.Login = new LoginViewModel();
        }
    }
}
