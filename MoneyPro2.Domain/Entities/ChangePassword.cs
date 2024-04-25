using Flunt.Notifications;
using Flunt.Validations;
using System.Text.RegularExpressions;

namespace MoneyPro2.Domain.Entities;
public partial class ChangePassword : Notifiable<Notification>
{
    private readonly Regex _strongPassword = StrongPassword();
    public ChangePassword(string email, string antiga, string nova)
    {
        Username = email;
        SenhaAntiga = antiga;
        SenhaNova = nova;

        UserContracts();
    }

    public string Username { get; private set; } = new("");
    public string SenhaAntiga { get; private set; } = string.Empty;
    public string SenhaNova { get; private set; } = string.Empty;

    private void UserContracts()
    {
        Clear();
        AddNotifications(
            new Contract<Notification>()
                .Requires()
                .IsTrue(
                _strongPassword.IsMatch(SenhaNova ?? ""),
                "Nova Senha", "A nova senha deve ter minúsculas, maiúsculas, números, caracteres especiais e ao menos 08 caracteres"
                )
        );
    }

    // A senha deve ter ao menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial
    // A senha deve conter de 8 até 30 caracteres
    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,30}$")]
    private static partial Regex StrongPassword();
}
