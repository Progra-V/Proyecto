using Supabase;
using Proyecto.Models;
using Proyecto.SupabaseClient;

namespace Proyecto.Services
{
    public static class UserService
    {
        public static async Task<User?> GetByEmail(string email)
        {
            try
            {
                Client client = SupabClient.getSupabaseClient();

                await client.InitializeAsync();

                var result = await client
                    .From<User>()
                    .Where(x => x.Email == email)
                    .Single();

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}