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


            var tickets = (await client
                .From<Ticket>()
                .Get()).Models;


            var departments = (await client
                .From<Department>()
                .Get()).Models;


            var users = (await client
                .From<User>()
                .Get()).Models;



            foreach (var ticket in tickets)
            {
                // Departamento

                var department = departments
    .FirstOrDefault(x => x.Id == ticket.DepartmentId);

                ticket.DepartmentName =
                    department?.Name ?? "Not found";
                



                // Usuario creador

                var creator = users
    .FirstOrDefault(x => x.Id == ticket.CreatedBy);

                ticket.CreatedByName = creator != null
                    ? $"{creator.FirstName} {creator.LastName}"
                    : "Unknown";



                // Técnico asignado

                if (ticket.AssignedTo.HasValue)
                {
                    var assignedUser = users
                        .FirstOrDefault(x => x.Id == ticket.AssignedTo.Value);

                    ticket.AssignedToName = assignedUser != null
                        ? $"{assignedUser.FirstName} {assignedUser.LastName}"
                        : "Unassigned";
                }
                else
                {
                    ticket.AssignedToName = "Unassigned";
                }
            }


            return tickets;
        }

        public static async Task UpdateTicket(
    long ticketId,
    string status,
    string priority,
    string risk,
    string category,
    int departmentId,
    int? assignedTo)
{
    Supabase.Client client = SupabClient.getSupabaseClient();

    await client.InitializeAsync();

    TicketUpdate update = new TicketUpdate
    {
        Id = ticketId,
        Status = status,
        Priority = priority,
        Risk = risk,
        Category = category,
        DepartmentId = departmentId,
        AssignedTo = assignedTo,
        UpdatedAt = DateTime.UtcNow
    };

    await client
        .From<TicketUpdate>()
        .Update(update);
}



        public static async Task<Ticket?> GetByTicketId(long id)
        {
            Supabase.Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();


            var result = await client
                .From<Ticket>()
                .Where(x => x.Id == id)
                .Get();


            return result.Model;
        }



        public static async Task Create(Ticket ticket)
        {
            Supabase.Client client = SupabClient.getSupabaseClient();

            await client.InitializeAsync();


            await client
                .From<Ticket>()
                .Insert(ticket);
        }



        public static async Task UpdateStatus(long ticketId, string newStatus)
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