using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UVVConsultas.Data;
using UVVConsultas.Models;

namespace UVVConsultas.Controllers
{
    public class ConsultasController : Controller
    {
        private readonly UVVConsultasContext _context;

        public ConsultasController(UVVConsultasContext context)
        {
            _context = context;
        }

        // GET: CONSULTAS
        public async Task<IActionResult> Index()
        {
            var consultas = await _context.Consultas
            .Include(c => c.Usuario) // traz o usuário relacionado
            .ToListAsync();

            return View(consultas);
            //return View(await _context.Consultas.ToListAsync());
        }

        // GET: CONSULTAS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consulta == null) return NotFound();

            return View(consulta);
        }

        // GET: CONSULTAS/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome");
            return View();
        }

        // POST: CONSULTAS/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioId,Especialidade,DataHora,Descricao,CreatedAt")] Consulta consulta)
        {
            if (ModelState.IsValid)
            {
                consulta.CreatedAt = DateTime.Now;
                _context.Add(consulta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", consulta.UsuarioId);
            return View(consulta);
        }

        // GET: CONSULTAS/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null) return NotFound();

            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", consulta.UsuarioId);
            return View(consulta);
        }

        // POST: CONSULTAS/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,UsuarioId,Especialidade,DataHora,Descricao,CreatedAt")] Consulta consulta)
        {
            if (id != consulta.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consulta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultaExists(consulta.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", consulta.UsuarioId);
            return View(consulta);
        }

        // GET: CONSULTAS/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consulta == null) return NotFound();

            return View(consulta);
        }

        // POST: CONSULTAS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta != null) _context.Consultas.Remove(consulta);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultaExists(int? id)
        {
            return _context.Consultas.Any(e => e.Id == id);
        }
    }
}
