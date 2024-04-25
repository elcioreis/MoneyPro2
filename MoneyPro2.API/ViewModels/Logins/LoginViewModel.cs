using MoneyPro2.Shared.ViewModels;

namespace MoneyPro2.API.ViewModels.Logins;

public class LoginViewModel : ViewModel
{
    public string Username { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;
}
