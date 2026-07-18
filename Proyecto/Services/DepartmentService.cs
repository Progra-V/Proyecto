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

        public static async Task<Department?> GetByCode(string code)
        {
            try
            {
                Client client = SupabClient.getSupabaseClient();

                await client.InitializeAsync();

                var result = await client
                    .From<Department>()
                    .Where(x => x.Code == code)
                    .Single();

                return result;
            }
            catch
            {
                return null;
            }
        }


        public static async Task<Department?> GetByName(string name)
        {
            try
            {
                Client client = SupabClient.getSupabaseClient();

                await client.InitializeAsync();

                var result = await client
                    .From<Department>()
                    .Where(x => x.Name == name)
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
            department.IsActive = !department.IsActive;

            await Edit(department);
        }

        //Generacion del codigo para el ticket, se genera con el codigo del departamento y el numero de ticket siguiente
        public static async Task<string> GenerateNextTicketCode(int departmentId)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var department = (await client
                .From<Department>()
                .Where(x => x.Id == departmentId)
                .Get()).Model;

            if (department == null)
                throw new InvalidOperationException("El departamento no existe.");

            string ticketCode =
                $"{department.Code}-{department.NextTicketNumber:D4}";

            department.NextTicketNumber++;

            await client
                .From<Department>()
                .Update(department);

            return ticketCode;
        }

    }
}