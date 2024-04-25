using Flunt.Notifications;
using Flunt.Validations;
using MoneyPro2.Domain.Functions;
using MoneyPro2.Domain.ValueObjects;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MoneyPro2.Domain.Entities;
public partial class User : Notifiable<Notification>
{
    private readonly Regex _allowedChars = AllowedChars();
    private readonly Regex _strongPassword = StrongPassword();

    public User() { }

    public User(string? username, string? nome, string? email, string? cpf, string? senha)
    {
        UserId = 0;
        Username = username ?? "";
        Nome = nome ?? "";
        Email = new Email(email ?? "");
        CPF = new CPF(cpf ?? "");
        Senha = senha ?? "";
        Criptografada = Tools.GenerateMD5(Username, Senha);

        UserContracts();
    }

    public int UserId { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string Nome { get; private set; } = string.Empty;
    public Email Email { get; private set; } = new("");
    public bool EmailVerificado { get; private set; } = false;
    public CPF CPF { get; private set; } = new("");
    [JsonIgnore]
    public string Senha { get; private set; } = string.Empty;
    [JsonIgnore]
    public string Criptografada { get; private set; } = string.Empty;
    public ICollection<UserLogin> UserLogins { get; private set; } = new List<UserLogin>();

    private void UserContracts()
    {
        Clear();
        AddNotifications(
            new Contract<Notification>()
                .Requires()
                .IsTrue(
                    Username?.Length >= 1 && Username?.Length <= 20,
                    "Username",
                    "O username deve ter entre 1 e 20 caracteres"
                )
                .IsTrue(
                    _allowedChars.IsMatch(Username ?? ""),
                    "Username",
                    "O username só pode ter letras, números, arroba ou ponto"
                )
                .IsTrue(Nome?.Length >= 3 && Nome?.Length <= 50,
                Nome, "O nome deve ter de 3 a 50 caracteres"
                )
                .IsTrue(
                _strongPassword.IsMatch(Senha ?? ""),
                "Senha", "A senha deve ter minúsculas, maiúsculas, números, caracteres especiais e ao menos 08 caracteres"
                )
        );
        AddNotifications(Email?.Notifications);
        AddNotifications(CPF?.Notifications);
    }

    // Caracteres permitidos para o username
    // O username deve conter entre 1 e 20 caracteres
    [GeneratedRegex("^([a-z0-9@.]){1,20}$")]
    private static partial Regex AllowedChars();

    // A senha deve ter ao menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial
    // A senha deve conter de 8 até 30 caracteres
    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,30}$")]
    private static partial Regex StrongPassword();
}
