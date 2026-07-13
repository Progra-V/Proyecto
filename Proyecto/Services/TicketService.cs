using Proyecto.Models;
using Proyecto.SupabaseClient;
using Supabase;
using Proyecto.Models;

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

        public static async Task UpdateStatus(int ticketId, string newStatus)
        {
            Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var ticket = (await client
                .From<TicketDb>()
                .Where(x => x.Id == ticketId)
                .Get()).Model;

            if (ticket == null)
                return;

            ticket.Status = newStatus;
            ticket.UpdatedAt = DateTime.UtcNow;

            await ticket.Update<TicketDb>();
        }
    }
}