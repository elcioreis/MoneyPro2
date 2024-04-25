using Flunt.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyPro2.API.Data;
using MoneyPro2.API.ViewModels;
using MoneyPro2.API.ViewModels.Users;
using MoneyPro2.Domain.Entities;
using MoneyPro2.Domain.Functions;

namespace MoneyPro2.API.Controllers;

[ApiController]
public class PasswordController : ControllerBase
{
    [HttpPost("v1/changepassword/")]
    public async Task<IActionResult> ChangePasswordAsync(
        [FromBody] ChangePasswordViewModel model,
        [FromServices] MoneyPro2DataContext context
    )
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email.Address == model.Email);

        if (user == null)
        {
            return StatusCode(500, new ResultViewModel<string>("02x01 - e-mail não encontrado"));
        }

        var newpass = new ChangePassword
        (
          user.Username,
          model.SenhaAntiga,
          model.SenhaNova
        );

        if (!newpass.IsValid)
        {
            return BadRequest(new ResultViewModel<List<Notification>>(newpass.Notifications.ToList()));
        }

        var cripto = Tools.GenerateMD5(user.Username, model.SenhaAntiga);

        return StatusCode(500, new ResultViewModel<string>("Desenvolvimento"));

    }
}
