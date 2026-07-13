using Supabase;
using Proyecto.Models;
using Proyecto.SupabaseClient;

namespace Proyecto.Services
{
    public static class DepartmentService
    {
        public static async Task<List<Department>> GetAll()
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client
                .From<Department>()
                .Get();

            return result.Models;
        }


        public static async Task<Department?> GetById(int id)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client
                .From<Department>()
                .Where(x => x.Id == id)
                .Get();

            return result.Model;
        }


        public static async Task<Department?> GetByName(string nombre)
        {
            try
            {
                Client client = SupabClient.getSupabaseClient();

                await client.InitializeAsync();

                var result = await client
                    .From<Department>()
                    .Where(x => x.Nombre == nombre)
                    .Single();

                return result;
            }
            catch
            {
                return null;
            }
        }


        public static async Task Create(Department department)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            await client
                .From<Department>()
                .Insert(department);
        }


        public static async Task Edit(Department department)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            await client
                .From<Department>()
                .Update(department);
        }


        public static async Task ChangeStatus(Department department)
        {
            department.Activo = !department.Activo;

            await Edit(department);
        }
    }
}
