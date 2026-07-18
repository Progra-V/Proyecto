using Proyecto.Models;
using Proyecto.Models.ViewModels;
using Proyecto.SupabaseClient;
using Supabase;

namespace Proyecto.Services
{
    public static class CategoryService
    {
        // Obtiene todas las categorías.
        public static async Task<List<CategoryViewModel>> GetAll()
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var categories = (await client
                .From<Category>()
                .Get()).Models;

            var departments = (await client
                .From<Department>()
                .Get()).Models;

            var result = new List<CategoryViewModel>();

            foreach (var category in categories)
            {
                var department = departments
                    .FirstOrDefault(x => x.Id == category.DepartmentId);

                result.Add(new CategoryViewModel
                {
                    Id = category.Id,
                    DepartmentId = category.DepartmentId,
                    DepartmentName = department?.Name ?? "No encontrado",
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt
                });
            }

            return result;
        }

        // Obtiene las categorías activas.
        public static async Task<List<CategoryViewModel>> GetActive()
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var categories = (await client
                .From<Category>()
                .Where(x => x.IsActive == true)
                .Get()).Models;

            var departments = (await client
                .From<Department>()
                .Get()).Models;

            var result = new List<CategoryViewModel>();

            foreach (var category in categories)
            {
                var department = departments
                    .FirstOrDefault(x => x.Id == category.DepartmentId);

                result.Add(new CategoryViewModel
                {
                    Id = category.Id,
                    DepartmentId = category.DepartmentId,
                    DepartmentName = department?.Name ?? "No encontrado",
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt
                });
            }

            return result;
        }

        // Obtiene una categoría por su identificador.
        public static async Task<Category?> GetById(long id)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var category = (await client
                .From<Category>()
                .Where(x => x.Id == id)
                .Get()).Model;

            if (category == null)
                return null;

            var department = (await client
                .From<Department>()
                .Where(x => x.Id == category.DepartmentId)
                .Get()).Model;


            return category;
        }

        // Obtiene las categorías de un departamento.
        public static async Task<List<CategoryViewModel>> GetByDepartment(long departmentId)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var categories = (await client
                .From<Category>()
                .Where(x => x.DepartmentId == departmentId)
                .Where(x => x.IsActive == true)
                .Get()).Models;

            var department = (await client
                .From<Department>()
                .Where(x => x.Id == departmentId)
                .Get()).Model;

            List<CategoryViewModel> result = new();

            foreach (var category in categories)
            {
                result.Add(new CategoryViewModel
                {
                    Id = category.Id,
                    DepartmentId = category.DepartmentId,
                    DepartmentName = department?.Name ?? "No encontrado",
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt
                });
            }

            return result;
        }

        // Crea una categoría.
        public static async Task Create(Category category)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = null;
            category.IsActive = true;

            await client.From<Category>().Insert(category);
        }

        // Actualiza una categoría.
        public static async Task Update(Category category)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            category.UpdatedAt = DateTime.UtcNow;

            await client.From<Category>().Update(category);
        }

        // Desactiva una categoría.
        public static async Task Disable(long id)
        {
            var category = await GetById(id);

            if (category == null)
                return;

            category.IsActive = false;
            category.UpdatedAt = DateTime.UtcNow;

            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            await client.From<Category>().Update(category);
        }

        // Verifica si una categoría ya existe.
        public static async Task<bool> Exists(string name, long departmentId)
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