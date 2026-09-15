namespace UVVConsultas.Models
{
    public class Consulta
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; } // permite nulo para ser possível preencher via forms. Não conseguimos preencher via forms o elemento Usuario. Só o ID.
        public string Especialidade { get; set; }
        public DateTime DataHora { get; set; }
        public string Descricao { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
