namespace chatbot_ludo.UIMAUI
{
    using chatbot_ludo.UIMAUI.Views;
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
