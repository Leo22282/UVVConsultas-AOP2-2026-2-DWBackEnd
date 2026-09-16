using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

        // Função para obter o ID do usuário logado a partir das claims
        private int GetCurrentUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(id, out var v) ? v : 0;
        }

        // GET: CONSULTAS
        public async Task<IActionResult> Index()
        {
            // Obtém o ID do usuário logado a partir das claims
            var usuarioLogado = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var consultas = await _context.Consultas
            .Include(c => c.Usuario) // traz o usuário relacionado
            .Where(c => c.UsuarioId.ToString() == usuarioLogado)
            .ToListAsync();

            return View(consultas);
            //return View(await _context.Consultas.ToListAsync());
        }

        // GET: CONSULTAS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var consulta = await _context.Consultas
                .Include(c => c.Usuario) // <-- garante que Usuario.Nome esteja disponível
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consulta == null) return NotFound();

            return View(consulta);
        }

        // GET: CONSULTAS/Create
        public IActionResult Create()
        {
            // Gera um SelectList contendo apenas o usuário logado
            var currentUserId = GetCurrentUserId();

            var usuarioFiltrado = _context.Usuarios
                .Where(u => u.Id == currentUserId)
                .Select(u => new { u.Id, u.Nome })
                .ToList();

            ViewData["UsuarioId"] = new SelectList(usuarioFiltrado, "Id", "Nome", currentUserId);
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

            // Gera um SelectList contendo apenas o usuário logado
            var currentUserId = GetCurrentUserId();

            var usuarioFiltrado = _context.Usuarios
                .Where(u => u.Id == currentUserId)
                .Select(u => new { u.Id, u.Nome })
                .ToList();

            ViewData["UsuarioId"] = new SelectList(usuarioFiltrado, "Id", "Nome", currentUserId);
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
                .Include(c => c.Usuario) // <-- garante que Usuario.Nome esteja disponível
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
