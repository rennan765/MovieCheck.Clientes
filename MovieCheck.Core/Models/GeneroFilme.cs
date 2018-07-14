namespace MovieCheck.Core.Models
{
    public class GeneroFilme
    {
        public int GeneroId { get; set; }
        public Genero Genero { get; set; }
        public int FilmeId { get; set; }
        public Filme Filme { get; set; }
    }
}
