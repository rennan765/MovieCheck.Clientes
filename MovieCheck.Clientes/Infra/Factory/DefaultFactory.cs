namespace MovieCheck.Clientes.Infra.Factory
{
    public static class DefaultFactory
    {
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
    }
}
