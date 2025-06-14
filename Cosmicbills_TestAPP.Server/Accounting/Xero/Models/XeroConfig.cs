namespace Cosmicbills_TestAPP.Server.Accounting.Xero.Models
{
    public class XeroConfig
    {
        private static IConfiguration? _configuration;

        public static void Init()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();

            ClientId = _configuration["XeroConfiguration:ClientId"];
            ClientSecret = _configuration["XeroConfiguration:ClientSecret"];
            CallbackUrl = _configuration["XeroConfiguration:CallbackUrl"];
            State = _configuration["XeroConfiguration:State"];
            Scope = _configuration["XeroConfiguration:Scope"];
            AuthUrl = _configuration["XeroConfiguration:AuthUrl"];
            TokenUrl = _configuration["XeroConfiguration:TokenUrl"];
            ApiUrl = _configuration["XeroConfiguration:ApiUrl"];
            TenantId = _configuration["XeroConfiguration:TenantId"];
        }

        public static string? ClientId { get; set; }

        public static string? ClientSecret { get; set; }

        public static string? CallbackUrl { get; set; }

        public static string? State { get; set; }

        public static string? Scope { get; set; }

        public static string? AuthUrl { get; set; }

        public static string? TokenUrl { get; set; }

        public static string? ApiUrl { get; set; }

        public static string? TenantId { get; set; }

        public static string? AccessToken { get; set; }

        public static string? RefreshToken { get; set; }
    }
}
