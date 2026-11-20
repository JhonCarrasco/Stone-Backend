namespace Stone.Entities
{
    public class AppSettings
    {
        public Jwt Jwt { get; set; } = default!;
        public SmtpConfiguration SmtpConfiguration { get; set; } = default!;
        public GoogleDrive GoogleDrive { get; set; } = default!;
    }

    public class Jwt
    {
        public string JWTKey { get; set; } = string.Empty;
        public int LifetimeInSeconds { get; set; }
    }

    public class SmtpConfiguration
    {
        public string UserName { get; set; } = default!;
        public string Server { get; set; } = default!;
        public string Password { get; set; } = default!;
        public int PortNumber { get; set; }
        public string FromName { get; set; } = default!;
        public bool EnableSsl { get; set; }
    }

    public class Swagger
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ContactName { get; set; }
        public string ContactUrl { get; set; }
        public string ContactEmail { get; set; }
        public string HostPath { get; set; }
        public string DefaultVersion { get; set; }
        public string Scheme { get; set; }
        public bool EnableApiKey { get; set; }
        public string ApiKey { get; set; }
        public string HeaderName { get; set; }
    }

    public class GoogleDrive
    {
        public string ApplicationName { get; set; } = default!;
        public string ServiceAccountEmail { get; set; } = default!;
        public string PrivateKey { get; set; } = default!;
        public string BulgetFolder { get; set; } = default!;
        public string SharedFolderRoot { get; set; } = default!;
        public string ReceptionFolder { get; set; } = default!;
        public string DispatchFolder { get; set; } = default!;
        public string VoucherFolder { get; set; } = default!;
        public string ExpenseFolder { get; set; } = default!;
    }
}
