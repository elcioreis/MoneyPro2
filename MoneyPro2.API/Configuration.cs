namespace MoneyPro2.API;

public static class Configuration
{
    public static string JwtKey = "<carregado de appsettings.json>";
    public static string ApiKeyName = "<carregado de appsettings.json>";
    public static string ApiKey = "<carregado de appsettings.json>";

    public static SmtpConfiguration Smtp { get; set; } = new();

    public class SmtpConfiguration
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; } = 25;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
