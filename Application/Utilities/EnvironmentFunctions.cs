namespace Application.Utilities
{
    public static class EnvironmentFunctions
    {
        public static readonly string CONNECTION_STRING = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? "host=localhost;port=5433;database=db;username=user;password=password;";
        public static readonly string ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        public static readonly string AUTHORIZATION_TOKEN = Environment.GetEnvironmentVariable("AUTHORIZATION_TOKEN") ?? "ThisismenttobetheAuthorizationtoken";

        public static bool isEnv(string env = "Production")
        {
            return ASPNETCORE_ENVIRONMENT == env;
        }
    }
}

