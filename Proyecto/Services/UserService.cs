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


        public static async Task<List<User>> GetAll()
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client
                .From<User>()
                .Get();

            return result.Models;
        }


        public static async Task<User?> GetById(int id)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client
                .From<User>()
                .Where(x => x.Id == id)
                .Get();

            return result.Model;
        }


        public static async Task Create(User user)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            await client
                .From<User>()
                .Insert(user);
        }


        public static async Task Edit(User user)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            await client
                .From<User>()
                .Update(user);
        }


        public static async Task ChangeStatus(User user)
        {
            user.Activo = !user.Activo;

            await Edit(user);
        }
    }
}