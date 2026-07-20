using Proyecto.Models;
using Proyecto.SupabaseClient;

namespace Proyecto.Services
{
    public static class CommentService
    {
        public static async Task<List<Comment>> GetByTicketId(long id)
        {
            Supabase.Client client =
                await SupabClient.GetSupabaseClientAsync();

            var result = await client
                .From<Comment>()
                .Where(x => x.TicketId == id)
                .Get();

            return result.Models;
        }


        public static async Task<Comment?> GetById(long commentId)
        {
            Supabase.Client client =
                await SupabClient.GetSupabaseClientAsync();

            var result = await client
                .From<Comment>()
                .Where(x => x.Id == commentId)
                .Get();

            return result.Model;
        }


        public static async Task Create(Comment comment)
        {
            Supabase.Client client =
                await SupabClient.GetSupabaseClientAsync();

            await client
                .From<Comment>()
                .Insert(comment);
        }


        public static async Task Delete(long commentId)
        {
            Supabase.Client client =
                await SupabClient.GetSupabaseClientAsync();

            await client
                .From<Comment>()
                .Where(x => x.Id == commentId)
                .Delete();
        }
    }
}