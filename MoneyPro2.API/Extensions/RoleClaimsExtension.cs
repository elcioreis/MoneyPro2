using MoneyPro2.Domain.Entities;
using System.Security.Claims;

namespace MoneyPro2.API.Extensions;

public static class RoleClaimsExtension
{
    public static IEnumerable<Claim> GetClaims(this Login login)
    {
        var result = new List<Claim>
        {
            new Claim("UserId", login.UserId.ToString()),
            new Claim(ClaimTypes.Name, login.Nome ?? ""),
            new Claim(ClaimTypes.Email, login.Email?.ToString() ?? "")
        };
        return result;
    }

    public static int GetUserId(this ClaimsPrincipal login)
    {
        //const string _primarysid = "primarysid";

        const string _userid = "userid";
        if (login != null && login.HasClaim(c => c.Type.ToLower().EndsWith(_userid)))
        {
#pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
            string strID =
                login.Claims
                    .FirstOrDefault(x => x.Type.ToLowerInvariant().EndsWith(_userid))
                    .Value ?? string.Empty;
#pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.


            if (int.TryParse(strID, out int id))
            {
                return id;
            }
        }

        return -1;
    }

    public static string GetUserEmail(this ClaimsPrincipal login)
    {
        const string _emailaddress = "emailaddress";
        if (login != null && login.HasClaim(c => c.Type.ToLower().EndsWith(_emailaddress)))
        {
#pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
            return login.Claims
                    .FirstOrDefault(x => x.Type.ToLowerInvariant().EndsWith(_emailaddress))
                    .Value ?? string.Empty;
#pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
        }

        return string.Empty;
    }
}
