namespace chatbot_ludo.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using chatbot_ludo.Web.Data;
    using chatbot_ludo.Web.Data.Entities;
    public class ConsejosController : Controller
    {
        //Vamos a quitar la inyeccion que hace el controlador para hacerlo desde la interface anteriormente hecha.
        //Vamos a inyectar la interface para inyectar el repositorio.
        private readonly IRepository repository; //Para que este disponible en todo el proyecto.

        public ConsejosController(IRepository repository)
        {
            this.repository = repository;
        }

        // GET: Consejos
        public IActionResult Index()
        {
              return repository.GetConsejos() != null ? 
                          //Usamos la interface para el retorno de los elementos.
                          View(this.repository.GetConsejos()) :
                          Problem("Entity set 'DataContext.Consejos'  is null.");
        }

        // GET: Consejos/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consejo = this.repository.GetConsejo(id.Value); //Obtenemos el consejo por medio del metodo de getproducto por id.
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
        public async Task<IActionResult> Create(Consejo consejo) //Quitamos el bind
        {
            if (ModelState.IsValid)
            {
                //Quitamos las lineas de conexion directa.
                this.repository.AddConsejo(consejo);
                //Lo guardamos
                await this.repository.SaveAllAsync(); //Metodo async se va a editar más adelante porque no debemos suponer
                return RedirectToAction(nameof(Index));
            }
            return View(consejo);
        }

        // GET: Consejos/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consejo = this.repository.GetConsejo(id.Value);
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

            if (ModelState.IsValid)
            {
                try
                {
                    this.repository.UpdateConsejo(consejo);
                    await this.repository.SaveAllAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.repository.ConsejoExists(consejo.ID_Consejo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(consejo);
        }

        // GET: Consejos/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consejo = this.repository.GetConsejo(id.Value);
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
            var consejo = this.repository.GetConsejo(id);
            if (consejo != null)
            {
                this.repository.RemoveConsejo(consejo);
            }
            
            await this.repository.SaveAllAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
