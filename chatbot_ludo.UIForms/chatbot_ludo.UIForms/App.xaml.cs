namespace chatbot_ludo.UIForms
{
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using chatbot_ludo.UIForms.Views;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage()); //Ahora arrancamos en el LoginPage
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
