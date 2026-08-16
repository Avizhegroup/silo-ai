using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace SiloAI.Identity.Server.Utilities;

public static class AiCryptoTools
{
    public static SigningCredentials GetJwtCredential(string key)
    {
        return new(GetSymmetricKey(key), SecurityAlgorithms.HmacSha256Signature);
    }

    public static SymmetricSecurityKey GetSymmetricKey(string passKey)
    {
        var key = Encoding.UTF8.GetBytes(passKey);

        return new(key);
    }

    public static string GetHashedStringSha256(string data)
    {
        using var sha256 = SHA256.Create();

        var byteHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));

        var sb = new StringBuilder();

        for (int i = 0; i < byteHash.Length; i++)
        {
            sb.Append(byteHash[i].ToString("x2"));
        }

        return sb.ToString();
    }

    public static bool ValidatePasswordSha256(string storedHash, string inputPassword)
    {
        var computed = GetHashedStringSha256(inputPassword);

        return string.Equals(computed, storedHash, StringComparison.Ordinal);
    }
}
