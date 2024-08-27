namespace App.Extensions
{
    public static class StringExtensions
    {
        public static string PrimeiraPalavra(this string texto)
        {
            return texto.Trim().Substring(0, texto.IndexOf(" "));
        }
    }
}