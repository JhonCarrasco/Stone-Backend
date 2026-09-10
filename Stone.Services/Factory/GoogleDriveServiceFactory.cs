using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;

namespace Stone.Services.Factory
{
    public class GoogleDriveServiceFactory
    {
        private readonly IConfiguration _configuration;

        public GoogleDriveServiceFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DriveService CreateDriveService()
        {
            var section = _configuration.GetSection("GoogleDrive");
            string appName = section["ApplicationName"];

            // 1. Initialize Authentication (Service Account Flow)
            var credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(section["ServiceAccountEmail"])
                {
                    Scopes = new[] { DriveService.Scope.DriveFile } // Adjust scope based on needs
                }.FromPrivateKey(section["PrivateKey"].Replace("\\n", "\n"))
            );




            // 2. Build and configuration the DriveService
            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = appName
            });
        }
    }
}
