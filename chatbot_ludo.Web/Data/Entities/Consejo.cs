namespace chatbot_ludo.Web.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Consejo
    {
        public int ID_Consejo {  get; set; } //Lave primaria
        [MaxLength(250, ErrorMessage = "The field {0} only can contain {1} characters length.")] //Anotaciones para limitar el consejo a 150 caracteres max
        [Required] //Requerido. No puede haber consejos vacios.
        public string Texto_Consejo { get; set;}
        [MaxLength(50)]
        public string Categoria {  get; set; } //prevesion, sintomas, apoyo, etc.
        public int Grado_Recomendacion { get; set; } //1 bajo, 2 medium, 3 hight
        public DateTime Fecha_Creacion {  get; set; }

        //Relacionamosla tabla de consejos con la de usuarios.
        public User User { get; set; } //Configuramos de uno a varios.

        //Todo consejo lo crea un usuario. Un usuario tiene muchos consejos. -> Migramos. 
        //Agregar una migración:
            
        //dotnet ef migrations add UpdateDataContextWithIdentity
        //Actualizar la base de datos:
        //dotnet ef database update

        //Revisar base de datos, se añadieron las tablas de usuarios, roles, etc. Borrar tabla de consejos porque tiene llave foranea con User.


        //Para hacer los cambios en DB vamos a la consola, dir direccion del producto web 
        //Corremos acorde a lo que modificamos: dotnet ef migrations add ModifyConsejos dotnet ef database update -- Porque modificamos consejos.
        //Solo corremos cuando, por ejemplo, modificamos longitud (son migraciones)
        //Nos equivocamos en la fecha, hacemos la migracion: dotnet ef migrations add RenameFechaCreacionColumn. Cada que hagamos una migracion nombramos,
        //acorde a lo que modificamos, ver carpeta Migrations para más contexto.
        //Cada que hagamos esto actualizamos: dotnet ef database update
    }
}
