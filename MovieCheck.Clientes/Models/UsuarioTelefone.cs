namespace MovieCheck.Clientes.Models
{
    public class UsuarioTelefone
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int TelefoneId { get; set; }
        public Telefone Telefone { get; set; }
    }
}
