namespace chatbot_ludo.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using chatbot_ludo.Web.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Web.Data;
    using Web.Data.Entities;
    using Web.Helpers;

    public class ConsejosController : Controller
    {

        //Vamos a quitar la inyeccion que hace el controlador para hacerlo desde la interface anteriormente hecha.
        //Vamos a inyectar la interface para inyectar el repositorio de consejo.

        private readonly IConsejoRepository consejoRepository;
        private readonly IUserHelper userHelper;

        public ConsejosController(IConsejoRepository consejoRepository, IUserHelper userHelper) 
        {
            this.consejoRepository = consejoRepository;
            this.userHelper = userHelper;
        }

        // GET: Consejos
        public IActionResult Index()
        {
            return View(this.consejoRepository.GetAll()); //Obtenemos todos los consejos. Ahora de esta manera debido a que estamos usando el repositorio generico.
        }

        // GET: Consejos/Details/5
        public async Task <IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consejo = await this.consejoRepository.GetByIdAsync(id.Value); //Modificamos conforme el nombre de los metodos en el repositorio generico <t>
            if (consejo == null)
            {
                return NotFound();
            }

            return View(consejo);
        }

        // GET: Consejos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Consejos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConsejoViewModel view) //Recordemos que ahora ya no es con Entitie, si no con el ViewModel.
        {
            //Convertimos la vista a consejo.
            var consejo = this.ToConsejo(view);
            try
            {
                // Como no hay usuario autenticado, usamos el correo estático.
                var user = await this.userHelper.GetUserByEmailAsync("alexis.hernandez074@gmail.com");

                // Verificamos que el usuario no sea null
                if (user == null)
                {
                    ModelState.AddModelError("", "No se pudo encontrar el usuario con el correo estático proporcionado.");
                    return View(consejo); // Regresamos la vista con el error.
                }

                // Asignar el usuario y la clave foránea UserId al consejo
                consejo.User = user;
                consejo.UserId = user.Id;  // Asignar el UserId del usuario

                // Guardar el consejo usando el repositorio genérico
                await this.consejoRepository.CreateAsync(consejo);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ocurrió un error al crear el consejo: {ex.Message}");
                return View(consejo); // Regresa la vista con el error mostrado
            }
            //TODO: Validar la opción del Modelo porque no está obteniendo el usuario antes de valdiar.
            //if (ModelState.IsValid)
            //{

            //}

            // Si el modelo no es válido, mostramos los errores
            //var errors = ModelState.Values.SelectMany(v => v.Errors);
            //foreach (var error in errors)
            //{
            //    Console.WriteLine(error.ErrorMessage); // Mostrar errores en la consola para ver qué está fallando
            //}

            //return View(consejo); // Devuelve la vista con el modelo y los errores de validación
        }

        private Consejo ToConsejo(ConsejoViewModel view)
        {
            //Transformamos.
            return new Consejo
            {
                ID_Consejo = view.ID_Consejo,
                Texto_Consejo = view.Texto_Consejo,
                Categoria = view.Categoria,
                Grado_Recomendacion = view.Grado_Recomendacion,
                Fecha_Creacion = view.Fecha_Creacion,
                User = view.User,
                UserId = view.UserId,
            };
        }




        // GET: Consejos/Edit/5
        public async Task <IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consejo = await this.consejoRepository.GetByIdAsync(id.Value);
            if (consejo == null)
            {
                return NotFound();
            }
            return View(consejo);
        }

        // POST: Consejos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Consejo consejo)
        {
            //TODO: Misma situacion que crear. 
            //if (ModelState.IsValid)
            //{
                
            //    return RedirectToAction(nameof(Index));
            //}
            try
            {
                //TODO: Cambiar para usuarios logeados, estamos haciendo uno para pruebas unitarias.
                //Asignamos el usuario.
                consejo.User = await this.userHelper.GetUserByEmailAsync("alexis.hernandez074@gmail.com");
                //Para editar es actualizacion
                await this.consejoRepository.UpdateAsync(consejo);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await this.consejoRepository.ExistAsync(consejo.ID_Consejo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return View(consejo);
        }

        // GET: Consejos/Delete/5
        public async Task <IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consejo = await this.consejoRepository.GetByIdAsync(id.Value);
            if (consejo == null)
            {
                return NotFound();
            }

            return View(consejo);
        }

        // POST: Consejos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consejo = await this.consejoRepository.GetByIdAsync(id);
            await this.consejoRepository.DeleteAsync(consejo); //Borramos. Con eso hicimos todas las modificaciones.
            return RedirectToAction(nameof(Index));

        }
    }
}
