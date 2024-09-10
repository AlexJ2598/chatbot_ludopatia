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
        private readonly DataContext _context;

        public ConsejosController(DataContext context)
        {
            _context = context;
        }

        // GET: Consejos
        public async Task<IActionResult> Index()
        {
              return _context.Consejos != null ? 
                          View(await _context.Consejos.ToListAsync()) :
                          Problem("Entity set 'DataContext.Consejos'  is null.");
        }

        // GET: Consejos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Consejos == null)
            {
                return NotFound();
            }

            var consejo = await _context.Consejos
                .FirstOrDefaultAsync(m => m.ID_Consejo == id);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Consejo,Texto_Consejo,Categoria,Grado_Recomendacion,Fecha_Creacios")] Consejo consejo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consejo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(consejo);
        }

        // GET: Consejos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Consejos == null)
            {
                return NotFound();
            }

            var consejo = await _context.Consejos.FindAsync(id);
            if (consejo == null)
            {
                return NotFound();
            }
            return View(consejo);
        }

        // POST: Consejos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Consejo,Texto_Consejo,Categoria,Grado_Recomendacion,Fecha_Creacios")] Consejo consejo)
        {
            if (id != consejo.ID_Consejo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consejo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsejoExists(consejo.ID_Consejo))
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Consejos == null)
            {
                return NotFound();
            }

            var consejo = await _context.Consejos
                .FirstOrDefaultAsync(m => m.ID_Consejo == id);
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
            if (_context.Consejos == null)
            {
                return Problem("Entity set 'DataContext.Consejos'  is null.");
            }
            var consejo = await _context.Consejos.FindAsync(id);
            if (consejo != null)
            {
                _context.Consejos.Remove(consejo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsejoExists(int id)
        {
          return (_context.Consejos?.Any(e => e.ID_Consejo == id)).GetValueOrDefault();
        }
    }
}
