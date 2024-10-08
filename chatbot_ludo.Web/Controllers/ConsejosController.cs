namespace chatbot_ludo.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using chatbot_ludo.Web.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Web.Data;
    using Web.Data.Entities;
    using Web.Helpers;

    [Authorize] //Para que solo tengan acceso los usuarios logeados.
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
            return View(this.consejoRepository.GetAll().OrderBy(c => c.Texto_Consejo)); //Ordenamos por orden alfabetico.
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
        public async Task<IActionResult> Create(ConsejoViewModel view)
        {
            if (!ModelState.IsValid)
            {
                return View(view);
            }

            // Convertimos el ViewModel a entidad Consejo, pero sin User ni UserId
            var consejo = this.ToConsejo(view);

            // Asignar el usuario actual logueado
            var user = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            if (user == null)
            {
                ModelState.AddModelError("", "No se pudo encontrar el usuario.");
                return View(view);
            }

            // Asignar el usuario y el UserId al consejo
            consejo.User = user;
            consejo.UserId = user.Id;

            try
            {
                await this.consejoRepository.CreateAsync(consejo);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ocurrió un error al crear el consejo: {ex.Message}");
                return View(view);
            }
        }



        private Consejo ToConsejo(ConsejoViewModel view)
        {
            return new Consejo
            {
                ID_Consejo = view.ID_Consejo,
                Texto_Consejo = view.Texto_Consejo,
                Categoria = view.Categoria,
                Grado_Recomendacion = view.Grado_Recomendacion,
                Fecha_Creacion = view.Fecha_Creacion
                // Ya no necesitas incluir User ni UserId aquí, ya que serán asignados en el controlador
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
            //Vamos aquí a hacer lo contrario a crear, osea, convertir el producto a vista, no a producto.
            var view = this.ToConsejoViewModel(consejo);
            return View(view);
        }

        private ConsejoViewModel ToConsejoViewModel(Consejo consejo)
        {
            //Se hace lo mismo, solo que en lugar de convertir a objeto consejo convertimos a vista.
            return new ConsejoViewModel
            {
                ID_Consejo = consejo.ID_Consejo,
                Texto_Consejo = consejo.Texto_Consejo,
                Categoria = consejo.Categoria,
                Grado_Recomendacion = consejo.Grado_Recomendacion,
                Fecha_Creacion = consejo.Fecha_Creacion,
            };
        }

        // POST: Consejos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ConsejoViewModel view)
        {
            // Validamos el estado del modelo antes de proceder
            if (!ModelState.IsValid)
            {
                // Si el estado del modelo no es válido, regresamos a la vista
                return View(view);
            }

            try
            {
                // Convertimos la vista a consejo
                var consejo = this.ToConsejo(view);

                // Asignar el usuario logueado actual
                var user = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user == null)
                {
                    ModelState.AddModelError("", "No se pudo encontrar el usuario.");
                    return View(view);
                }

                // Asignar el usuario y el UserId al consejo
                consejo.User = user;
                consejo.UserId = user.Id;

                // Actualizamos el consejo en la base de datos
                await this.consejoRepository.UpdateAsync(consejo);

                // Redirigir al Index después de actualizar
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await this.consejoRepository.ExistAsync(view.ID_Consejo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Si hay algún error, regresa a la vista de edición con el modelo actual
            return View(view);
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
