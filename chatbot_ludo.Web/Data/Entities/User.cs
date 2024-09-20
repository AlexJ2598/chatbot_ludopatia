namespace chatbot_ludo.Web.Data.Entities
{
    using System;
    using Microsoft.AspNetCore.Identity;
    public class User : IdentityUser //Se hereda de aquí para los usuarios del sistema, es para el manejo de usuarios que nos brinda el framework.
    {
        //Personalizamos para nombre, Apellidos, etc
        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}
