using Microsoft.EntityFrameworkCore;

public class ConsultasController : Controller
{
    private readonly ApplicationDbContext _context;

    public ConsultasController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var consultas = await _context.Consultas
            .Include(c => c.Usuario) // traz o usuário relacionado
            .ToListAsync();

        return View(consultas);
    }
}