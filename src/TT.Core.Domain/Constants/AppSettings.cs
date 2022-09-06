namespace TT.Core.Domain.Constants;

public class AppSettings
{
    public static AppSettings Instance { get; private set; }

    public string SecretKey { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public int RefreshTokenExpiration { get; set; }

    public string MailEmail { get; set; }

    public string MailPassword { get; set; }

    public static void Initialize(AppSettings appSettings)
    {
        Instance = appSettings;
    }
}
