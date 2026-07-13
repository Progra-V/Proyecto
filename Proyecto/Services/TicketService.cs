using Proyecto.Models;
using Proyecto.SupabaseClient;
using Supabase;

namespace Proyecto.Services
{
    public static class TicketService
    {
        public static async Task<List<Ticket>> GetAll()
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client.From<Ticket>().Get();
            return result.Models;
        }

        public static async Task<Ticket> GetByTicketId(int id)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client.From<Ticket>().Where(x => x.Id == id).Get();

            var comments = await client.From<Comment>().Where(x => x.TicketId == id).Get();

            result.Model?.Comments = comments.Models;

            return result.Model;
        }

        public static async Task CreateComment(Comment comment)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            await client.From<Comment>().Insert(comment);
        }

        public static async Task DeleteComment(int commentId)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            await client.From<Comment>().Where(x => x.Id == commentId).Delete();
        }
    }
}
