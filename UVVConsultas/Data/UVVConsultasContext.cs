using UVVConsultas.Models;
using Microsoft.EntityFrameworkCore;

namespace UVVConsultas.Data
{
    public class UVVConsultasContext :DbContext
    {
        public UVVConsultasContext(DbContextOptions<UVVConsultasContext> options):base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Consulta> Consultas { get; set; }


    }
}
