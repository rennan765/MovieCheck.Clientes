using MovieCheck.Site.Models.ViewModels;

namespace MovieCheck.Site.Infra.Factory
{
    public static class DefaultFactory
    {
        public static MensagemViewModel _mensagemViewModel;

        public static bool IsNumeric(string value)
        {
            bool isNumeric = true;
            char[] valueChars = value.ToCharArray();

            foreach (var valueChar in valueChars)
            {
                if (!char.IsDigit(valueChar))
                {
                    isNumeric = false;
                    break;
                }
            }

            return isNumeric;
        }

        public static bool ValidaBotaoLogOff(string titulo)
        {
            return titulo != "Seja Bem Vindo" && titulo != "Novo Usuário" && titulo != "Esqueceu a sua senha?";
        }
    }
}
