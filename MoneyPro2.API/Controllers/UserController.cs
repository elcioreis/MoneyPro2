using Flunt.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyPro2.API.Data;
using MoneyPro2.API.Extensions;
using MoneyPro2.API.Services;
using MoneyPro2.API.ViewModels;
using MoneyPro2.API.ViewModels.Users;
using MoneyPro2.Domain.Entities;
using MoneyPro2.Domain.Functions;

namespace MoneyPro2.API.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    [HttpPost("v1/users/")]
    public async Task<IActionResult> NewUserAsync(
        [FromBody] RegisterUserViewModel model,
        [FromServices] EmailService emailService,
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

            emailService.Send(
                user.Nome,
                user.Email.Address,
                "Bem vindo ao MoneyPro2.",
                "Confirme esse e-mail para validar seu endereço.");

            return Ok(
                new ResultViewModel<dynamic>(
                    new
                    {
                        userid = user.UserId,
                        username = user.Username,
                        nome = user.Nome,
                        email = user.Email?.Address,
                        cpf = user.CPF?.Numero
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

    [Authorize]
    [HttpPut("v1/users/")]
    public async Task<IActionResult> UpdateUserAsync(
        [FromBody] UpdateUserViewModel model,
        [FromServices] MoneyPro2DataContext context
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

        var userid = User.GetUserId();

        var user = await context.Users.FirstOrDefaultAsync(x => x.UserId == userid);

        if (user == null)
        {
            return StatusCode(500, new ResultViewModel<string>("01x06 - Usuário não encontrado"));
        }

        user.SetNome(model.Nome);
        user.CPF.SetNumero(model.CPF);

        if (user.Email.ToString() != model.Email)
        {
            user.SetVerificado(false);
            user.Email.SetAddress(model.Email);
        }

        if (!user.IsValid)
        {
            return BadRequest(new ResultViewModel<List<Notification>>(user.Notifications.ToList()));
        }

        try
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();

            return Ok(
                new ResultViewModel<dynamic>(
                    new
                    {
                        username = user.Username,
                        nome = user.Nome,
                        email = user.Email?.Address,
                        cpf = user.CPF?.Numero
                    }
                    )
                );
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.ToLower().Contains("ix_user_email"))
            {
                return StatusCode(500, new ResultViewModel<string>($"01x07 - o e-mail '{user.Email?.Address}' já está em uso"));
            }

            if (ex.InnerException != null && ex.InnerException.Message.ToLower().Contains("ix_user_cpf"))
            {
                return StatusCode(500, new ResultViewModel<string>($"01x08 - o CPF '{user.CPF?.Numero}' já está em uso"));
            }

            return StatusCode(500, new ResultViewModel<string>("01x09 - Erro ao atualizar o usuário"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("01x10 - Falha interna no servidor"));
        }
    }

    [Authorize]
    [HttpPost("v1/changepassword/")]
    public async Task<IActionResult> ChangePasswordAsync(
    [FromBody] ChangePasswordViewModel model,
    [FromServices] MoneyPro2DataContext context
    )
    {
        var newpass = new ChangePassword
        (
          model.SenhaAntiga,
          model.SenhaNova
        );

        if (!newpass.IsValid)
        {
            return BadRequest(new ResultViewModel<List<Notification>>(newpass.Notifications.ToList()));
        }

        string userName = User.Identity?.Name ?? "";

        var oldCripto = Tools.GenerateMD5(userName, model.SenhaAntiga);

        var user = await context.Users
            .FirstOrDefaultAsync(x =>
                x.Username == userName &&
                x.Criptografada == oldCripto);

        if (user == null)
        {
            return Unauthorized(new ResultViewModel<string>("01x06 - usuário ou senha incorretos"));
        }

        user.SetCriptografada(Tools.GenerateMD5(user.Username, model.SenhaNova));

        try
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return Ok(new ResultViewModel<dynamic>(new
            {
                Mensagem = "Senha alterada"
            }));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<string>("01x07 - Falha interna no servidor"));
        }
    }
}
