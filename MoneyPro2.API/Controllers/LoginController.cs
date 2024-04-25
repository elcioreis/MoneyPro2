using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyPro2.API.Data;
using MoneyPro2.API.Services;
using MoneyPro2.API.ViewModels;
using MoneyPro2.API.ViewModels.Logins;
using MoneyPro2.API.ViewModels.Users;
using MoneyPro2.Domain.Entities;
using MoneyPro2.Domain.Functions;

namespace MoneyPro2.API.Controllers;

[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost("v1/login")]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginViewModel model,
        [FromServices] MoneyPro2DataContext context,
        [FromServices] TokenService tokenService
    )
    {
        string cripto = Tools.GenerateMD5(model.Username, model.Senha);

        Login? login = await context.Users
            .AsNoTracking()
            .Where(x => x.Username == model.Username && x.Criptografada == cripto)
            .Select(x => new Login
            {
                UserId = x.UserId,
                Username = x.Username,
                Nome = x.Nome,
                Email = x.Email
            })
            .FirstOrDefaultAsync();

        if (login == null)
        {
            return Unauthorized(new ResultViewModel<string>("00x01 - Usuário ou senha incorretos"));
        }

        try
        {
            await context.UserLogins.AddAsync(new UserLogin { UserId = login.UserId });
            await context.SaveChangesAsync();
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("00x02 - Não foi possível logar"));
        }

        try
        {
            var token = tokenService.GenerateToken(login);
            return Ok(
                new ResultViewModel<ResultUserViewModel>(
                    new ResultUserViewModel
                    {
                        UserId = login.UserId,
                        Username = model.Username,
                        Email = login.Email.Address,
                        Token = token
                    }));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<string>("00x03 - Falha interna no servidor"));
        }
    }
}
