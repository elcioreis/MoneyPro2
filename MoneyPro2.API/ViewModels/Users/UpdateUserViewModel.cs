using MoneyPro2.Shared.ViewModels;

namespace MoneyPro2.API.ViewModels.Users;

public class UpdateUserViewModel : ViewModel
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
}
