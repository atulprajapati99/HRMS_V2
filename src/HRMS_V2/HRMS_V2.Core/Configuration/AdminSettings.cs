namespace HRMS_V2.Core.Configuration;

public class AdminSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }

    public Tokens Tokens { get; set; }
}

public class Tokens
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    public int Expiry { get; set; }
    public int RefreshExpiry { get; set; }
}

public class ConnectionStrings
{
    public string HRMS_Database { get; set; }
}