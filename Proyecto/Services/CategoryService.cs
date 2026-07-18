using Proyecto.Models;
using Proyecto.SupabaseClient;
using Supabase;

namespace Proyecto.Services
{
    public static class CategoryService
    {
        // Obtiene todas las categorías.
        public static async Task<List<Category>> getAll()
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client.From<Category>().Get();

            return result.Models;
        }

        // Obtiene las categorías activas.
        public static async Task<List<Category>> getActive()
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client
                .From<Category>()
                .Where(x => x.IsActive == true)
                .Get();

            return result.Models;
        }

        // Obtiene una categoría por su identificador.
        public static async Task<Category?> getById(long id)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client
                .From<Category>()
                .Where(x => x.Id == id)
                .Get();

            return result.Model;
        }

        // Obtiene las categorías de un departamento.
        public static async Task<List<Category>> getByDepartment(long departmentId)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client
                .From<Category>()
                .Where(x => x.DepartmentId == departmentId)
                .Where(x => x.IsActive == true)
                .Get();

            return result.Models;
        }

        // Crea una categoría.
        public static async Task create(Category category)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = null;
            category.IsActive = true;

            await client.From<Category>().Insert(category);
        }

        // Actualiza una categoría.
        public static async Task update(Category category)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            category.UpdatedAt = DateTime.UtcNow;

            await client.From<Category>().Update(category);
        }

        // Desactiva una categoría.
        public static async Task disable(long id)
        {
            var category = await getById(id);

            if (category == null)
                return;

            category.IsActive = false;
            category.UpdatedAt = DateTime.UtcNow;

            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            await client.From<Category>().Update(category);
        }

        // Verifica si una categoría ya existe.
        public static async Task<bool> exists(string name, long departmentId)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client
                .From<Category>()
                .Where(x => x.DepartmentId == departmentId)
                .Where(x => x.Name == name)
                .Get();

            return result.Models.Any();
        }
    }
}