using System.Security.Cryptography;

namespace MoneyPro2.Domain.Functions;
public static partial class Tools
{
    public static string GenerateMD5(string username, string password)
    {
        // Use input string to calculate MD5 hash
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes($"{username}&{password}");
        byte[] hashBytes = MD5.HashData(inputBytes);

        return Convert.ToHexString(hashBytes).ToLower();
        ;
    }

    public static string GenerateMD5(string input)
    {
        // Use input string to calculate MD5 hash
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = MD5.HashData(inputBytes);

        return Convert.ToHexString(hashBytes).ToLower();
        ;
    }
}
