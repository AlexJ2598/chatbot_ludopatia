namespace chatbot_ludo.UIForms.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using chatbot_ludo.UIForms.ViewModels;
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }
        public InstanceLocator()
        {
            this.Main = new MainViewModel(); //Unico punto donde usamos MainViewModel. Por aquí manejamos cualquier punto o prop en la solución. Vindamos todo en el diccionario de recursos.
        }
    }
}
