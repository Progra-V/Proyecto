using Supabase;
using Proyecto.Models;
using Proyecto.SupabaseClient;



namespace Proyecto.Services
{
    public static class UserService
    {


        private const int AdminRole = 3;
        private const int TechnicianRole = 2;
        private const int EmployeeRole = 1;
        public static async Task<User?> GetByEmail(string email)
        {
            try
            {
                Client client = await SupabClient.GetSupabaseClientAsync();

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
            Client client = await SupabClient.GetSupabaseClientAsync();

            var result = await client
                .From<User>()
                .Get();

            return result.Models;
        }


        public static async Task<User?> GetById(int id)
        {
            Client client = await SupabClient.GetSupabaseClientAsync();

            var result = await client
                .From<User>()
                .Where(x => x.Id == id)
                .Get();

            return result.Model;
        }


        public static async Task Create(User user)
        {
            Client client = await SupabClient.GetSupabaseClientAsync();

            await client
                .From<User>()
                .Insert(user);
        }


        public static async Task Edit(User user)
        {
            Client client = await SupabClient.GetSupabaseClientAsync();

            await client
                .From<User>()
                .Update(user);
        }


        public static async Task ChangeStatus(User user)
        {
            user.IsActive = !user.IsActive;

            await Edit(user);
        }

    }
}