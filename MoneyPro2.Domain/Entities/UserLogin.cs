namespace MoneyPro2.Domain.Entities;
public class UserLogin
{
    public long Id { get; set; }
    public int UserId { get; set; }
    public DateTime LoginTime { get; set; }
    public User User { get; set; } = null!;
}
