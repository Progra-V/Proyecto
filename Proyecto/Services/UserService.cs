using Supabase;
using Proyecto.Models;
using Proyecto.SupabaseClient;



namespace Proyecto.Services
{
    public static class UserService
    {


        private const int AdminRole = 1;
        private const int TechnicianRole = 2;
        private const int EmployeeRole = 3;
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


        public static async Task<List<User>> GetAssignableUsers(User currentUser)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client
                .From<User>()
                .Where(x => x.IsActive == true)
                .Get();

            List<User> users = result.Models;

            if (currentUser.RoleId == EmployeeRole)
            {
                return new List<User>();
            }

            if (currentUser.RoleId == TechnicianRole)
            {
                return users
                    .Where(x => x.RoleId == TechnicianRole)
                    .ToList();
            }

            return users
                .Where(x => x.RoleId == AdminRole || x.RoleId == TechnicianRole)
                .ToList();
        }
    }
}