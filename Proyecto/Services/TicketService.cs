using Proyecto.Models;
using Proyecto.SupabaseClient;

namespace Proyecto.Services
{
    public static class TicketService
    {
        public static async Task<List<Ticket>> GetAll()
        {
            Supabase.Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client
                .From<Ticket>()
                .Get();

            return result.Models;
        }


        public static async Task<Ticket?> GetByTicketId(int id)
        {
            Supabase.Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var result = await client
                .From<Ticket>()
                .Where(x => x.Id == id)
                .Get();

            return result.Model;
        }


        public static async Task UpdateStatus(int ticketId, string newStatus)
        {
            Supabase.Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();

            var ticket = (await client
                .From<Ticket>()
                .Where(x => x.Id == ticketId)
                .Get()).Model;

            if (ticket == null)
                return;

            ticket.Status = newStatus;
            ticket.UpdatedAt = DateTime.UtcNow;

            await client
                .From<Ticket>()
                .Update(ticket);
        }
    }
}