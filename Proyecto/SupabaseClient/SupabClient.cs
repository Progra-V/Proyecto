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

        private static Client? _client;
        private static readonly SemaphoreSlim _lock = new(1, 1);

        public static async Task<Client> GetSupabaseClientAsync()
        {
            if (_client != null)
                return _client;

            await _lock.WaitAsync();
            try
            {
                if (_client == null)
                {
                    var options = new SupabaseOptions { AutoConnectRealtime = false };
                    _client = new Client(url, key, options);
                    await _client.InitializeAsync();
                }
            }
            finally
            {
                _lock.Release();
            }

            return _client;
        }
    }
}