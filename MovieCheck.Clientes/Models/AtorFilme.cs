namespace MovieCheck.Clientes.Models
{
    public class AtorFilme
    {
        public int AtorId { get; set; }
        public Ator Ator { get; set; }
        public int FilmeId { get; set; }
        public Filme Filme { get; set; }
    }
}
