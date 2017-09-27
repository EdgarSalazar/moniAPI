namespace Moni.Extensions
{
    public static class StringExtensions
    {
        public static string ToPascalCase(this string value)
        {
            return value.Substring(0,1).ToUpper() + value.Substring(1, value.Length - 1);
        }
    }
}
