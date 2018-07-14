namespace MovieCheck.Site.Models
{
    public class DiretorFilme
    {
        public int DiretorId { get; set; }
        public Diretor Diretor { get; set; }
        public int FilmeId { get; set; }
        public Filme Filme { get; set; }
    }
}
