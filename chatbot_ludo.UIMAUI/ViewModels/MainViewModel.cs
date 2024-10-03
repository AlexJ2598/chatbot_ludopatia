namespace chatbot_ludo.UIMAUI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MainViewModel // Para controlar el resto de las ViewModels.
    {
        public LoginViewModel Login { get; set; }

        // Instanciamos, luego cambiamos, mala práctica.
        public MainViewModel()
        {
            this.Login = new LoginViewModel(); // Solamente porque arranca. Luego lo cambiamos.
        }
    }
}

