using Proyecto.Models;
using Proyecto.SupabaseClient;

namespace Proyecto.Services
{
    public static class CommentService
    {
        public static async Task<List<Comment>> GetByTicketId(long id)
        {
            Supabase.Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client
                .From<Comment>()
                .Where(x => x.TicketId == id)
                .Get();

            return result.Models;
        }


        public static async Task Create(Comment comment)
        {
            Supabase.Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            await client
                .From<Comment>()
                .Insert(comment);
        }


        public static async Task Delete(int commentId)
        {
            Supabase.Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            await client
                .From<Comment>()
                .Where(x => x.Id == commentId)
                .Delete();
        }
    }
}