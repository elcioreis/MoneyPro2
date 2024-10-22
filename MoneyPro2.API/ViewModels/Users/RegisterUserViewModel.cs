﻿using MoneyPro2.Shared.ViewModels;

namespace MoneyPro2.API.ViewModels.Users;

public class RegisterUserViewModel : ViewModel
{
    public string Username { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}
