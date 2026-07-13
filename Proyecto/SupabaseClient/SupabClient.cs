using Supabase;

namespace Proyecto.SupabaseClient
{
    public static class SupabClient
    {
        private static readonly IConfigurationRoot _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        private static readonly string url = _config["Supabase:Url"]!;
        private static readonly string key = _config["Supabase:Key"]!;

        public static Client getSupabaseClient()
        {
            return new Client(url, key);
        }
    }
}