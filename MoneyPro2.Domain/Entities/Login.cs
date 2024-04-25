using Flunt.Notifications;

namespace MoneyPro2.Domain.Entities;
public class Login : Notifiable<Notification>
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public Email Email { get; set; } = new("");
}
