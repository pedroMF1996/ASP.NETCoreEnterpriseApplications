using System.Globalization;

namespace NSE.Core.Utils
{
    public static class DataHora
    {
        public static string ObterFormatado()
        {
            return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}
