namespace chatbot_ludo.UIMAUI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MainViewModel // Para controlar el resto de las ViewModels.
    {
        private static MainViewModel instance; //Creamos un apuntador.
        public LoginViewModel Login { get; set; }
        public ConsejosViewModel Consejos { get; set; }

        // Instanciamos.
        public MainViewModel()
        {
            instance = this;
        }

        //Devolvemos la instancia.
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }
    }
}

