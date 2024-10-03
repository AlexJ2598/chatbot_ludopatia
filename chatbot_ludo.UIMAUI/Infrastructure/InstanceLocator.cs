namespace chatbot_ludo.UIMAUI.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UIMAUI.ViewModels;
    public class InstanceLocator
    {
        public MainViewModel Main {  get; set; }

        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
    }
}
