namespace chatbot_ludo.UIMAUI
{
    using chatbot_ludo.UIMAUI.ViewModels;
    using chatbot_ludo.UIMAUI.Views;
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //Instanciamos,
            MainViewModel.GetInstance().Login = new LoginViewModel();

            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
