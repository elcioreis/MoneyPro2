using MoneyPro2.Domain.Entities;
using System.Security.Claims;

namespace MoneyPro2.API.Extensions;

public static class RoleClaimsExtension
{
    public static IEnumerable<Claim> GetClaims(this Login login)
    {
        var result = new List<Claim>
        {
            new Claim("userid", login.UserId.ToString()),
            new Claim(ClaimTypes.Name, login.Username),
            new Claim("fullname", login.Nome),
            new Claim(ClaimTypes.Email, login.Email?.ToString() ?? "")
        };
        return result;
    }

    //public static string GetUserName(this ClaimsPrincipal user)
    //{
    //    //const string _userName = "username";
    //    if (user != null) // && user.HasClaim(c => c.Type.ToLower().EndsWith(_userName)))
    //    {
    //        return user.Identity?.Name ?? "";
    //    }

    //    return string.Empty;
    //}

    public static int GetUserId(this ClaimsPrincipal user)
    {
        const string _userid = "userid";
        if (user != null && user.HasClaim(c => c.Type.ToLower().EndsWith(_userid)))
        {
#pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
            string strID =
                user.Claims
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

    public static string GetUserEmail(this ClaimsPrincipal user)
    {
        const string _emailaddress = "emailaddress";
        if (user != null && user.HasClaim(c => c.Type.ToLower().EndsWith(_emailaddress)))
        {
#pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
            return user.Claims
                    .FirstOrDefault(x => x.Type.ToLowerInvariant().EndsWith(_emailaddress))
                    .Value ?? string.Empty;
#pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
        }

        return string.Empty;
    }
}
