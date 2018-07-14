namespace MovieCheck.Site.Models.ViewModels
{
    public class PendenciaViewModel
    {
        #region Atributos
        private int id;
        private int filmeId;
        private string tituloFilme;
        private string posterFilme;
        private string tipoMidia;
        private string iconeMidia;
        private string caractereSituacao;
        private string descricaoSituacao;
        private string dataFormatada;
        #endregion

        #region Propriedades
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        public int FilmeId
        {
            get { return this.filmeId; }
            set { this.filmeId = value; }
        }
        public string TituloFilme
        {
            get { return this.tituloFilme; }
            set { this.tituloFilme = value; }
        }
        public string PosterFilme
        {
            get { return this.posterFilme; }
            set { this.posterFilme = value; }
        }
        public string TipoMidia
        {
            get { return this.tipoMidia; }
            set { this.tipoMidia = value; }
        }
        public string IconeMidia
        {
            get { return this.iconeMidia; }
            set { this.iconeMidia = value; }
        }
        public string CaractereSituacao
        {
            get { return this.caractereSituacao; }
            set { this.caractereSituacao = value; }
        }
        public string DescricaoSituacao
        {
            get { return this.descricaoSituacao; }
            set { this.descricaoSituacao = value; }
        }
        public string DataFormatada
        {
            get { return this.dataFormatada; }
            set { this.dataFormatada = value; }
        }
        #endregion

        #region Construtores
        public PendenciaViewModel(Pendencia pendencia)
        {
            this.id = pendencia.Id;
            this.filmeId = pendencia.FilmeId;
            this.tituloFilme = pendencia.Filme.Titulo;
            this.posterFilme = pendencia.Filme.Poster;
            this.tipoMidia = pendencia.Filme.ObterTipoMidia();
            this.iconeMidia = pendencia.Filme.ObterIconeMidia();
            this.caractereSituacao = pendencia.Status;
            this.descricaoSituacao = pendencia.RetornarDescricaoReserva();
            this.dataFormatada = $"{pendencia.DataReserva.Day}/{pendencia.DataReserva.Month}/{pendencia.DataReserva.Year}";
        }
        #endregion
    }
}
