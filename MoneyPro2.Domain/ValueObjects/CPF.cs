using Flunt.Notifications;
using Flunt.Validations;
using MoneyPro2.Domain.Functions;
using MoneyPro2.Shared.ValueObjects;

namespace MoneyPro2.Domain.ValueObjects;
public class CPF : ValueObject
{
    public CPF(string numero = "")
    {
        if (!string.IsNullOrEmpty(numero))
        {
            Numero = numero.Trim().Replace(".", "").Replace("-", "");
        }

        AddNotifications(
            new Contract<Notification>()
            .Requires()
            .IsTrue(Tools.CheckCPF(Numero), "CPF", "CPF inválido")
            );
    }

    public string Numero { get; private set; } = string.Empty;
}
