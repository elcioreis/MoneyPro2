using Flunt.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyPro2.API.Data;
using MoneyPro2.API.Extensions;
using MoneyPro2.API.ViewModels;
using MoneyPro2.API.ViewModels.Users;
using MoneyPro2.Domain.Entities;

namespace MoneyPro2.API.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    [HttpPost("v1/users/")]
    public async Task<IActionResult> NewUserAsync(
        [FromBody] RegisterViewModel model,
        [FromServices] MoneyPro2DataContext context
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

        var user = new User(model.Username, model.Nome, model.Email, model.CPF, model.Senha);

        if (!user.IsValid)
        {
            return BadRequest(new ResultViewModel<List<Notification>>(user.Notifications.ToList()));
        }

        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return Ok(
                new ResultViewModel<dynamic>(
                    new
                    {
                        userid = user.UserId,
                        user.Username,
                        email = user.Email?.Address
                    }
                    )
                );
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.ToLower().Contains("ix_user_username"))
            {
                return StatusCode(500, new ResultViewModel<string>($"01x01 - o usuário '{user.Username}' já está em uso"));
            }

            if (ex.InnerException != null && ex.InnerException.Message.ToLower().Contains("ix_user_email"))
            {
                return StatusCode(500, new ResultViewModel<string>($"01x02 - o e-mail '{user.Email?.Address}' já está em uso"));
            }

            if (ex.InnerException != null && ex.InnerException.Message.ToLower().Contains("ix_user_cpf"))
            {
                return StatusCode(500, new ResultViewModel<string>($"01x03 - o CPF '{user.CPF?.Numero}' já está em uso"));
            }

            return StatusCode(500, new ResultViewModel<string>("01x04 - Erro ao cadastrar o usuário"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("01x05 - Falha interna no servidor"));
        }
    }
}
