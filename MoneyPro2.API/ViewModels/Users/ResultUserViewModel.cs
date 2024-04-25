using MoneyPro2.Shared.ViewModels;

namespace MoneyPro2.API.ViewModels.Users;

public class ResultUserViewModel : ViewModel
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
