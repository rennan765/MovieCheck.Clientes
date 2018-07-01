using MovieCheck.Api.Infra;

namespace MovieCheck.Api.Models.ViewModel
{
    public class UsuarioViewModel
    {
        #region Propriedades
        public string Email { get; set; }
        public string Senha { get; set; }
        #endregion

        #region Métodos
        public string HashPassword()
        {
            return Util.HashPassword(this.Senha);
        }
        #endregion
    }
}
