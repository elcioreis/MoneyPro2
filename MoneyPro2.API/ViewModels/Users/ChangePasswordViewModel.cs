using MoneyPro2.Shared.ViewModels;

namespace MoneyPro2.API.ViewModels.Users;

public class ChangePasswordViewModel : ViewModel
{
    public string Email { get; set; } = string.Empty;
    public string SenhaAntiga { get; set; } = string.Empty;
    public string SenhaNova { get; set; } = string.Empty;
}
