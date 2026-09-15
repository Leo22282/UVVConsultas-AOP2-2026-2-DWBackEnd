using System.ComponentModel.DataAnnotations;
namespace UVVConsultas.Models
{
    public class Consulta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome do Paciente")]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; } // permite nulo para ser possível preencher via forms. Não conseguimos preencher via forms o elemento Usuario. Só o ID.

        [StringLength(100)]
        public string? Especialidade { get; set; }

        [Required]
        [Display(Name = "Data e Hora da Consulta")]
        [Range(typeof(DateTime), "09/01/2026", "12/31/2100")]
        public DateTime DataHora { get; set; }

        public string? Descricao { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
