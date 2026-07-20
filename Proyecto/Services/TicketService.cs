using Proyecto.Models;
using Proyecto.SupabaseClient;

namespace Proyecto.Services
{
    public static class TicketService
    {
        public static readonly List<string> StatusCatalog =
        [
            "Pendiente",
            "En Revisión",
            "En Progreso",
            "Cancelado",
            "Finalizado"
        ];

        public static readonly List<string> PriorityCatalog =
        [
            "Alta",
            "Media",
            "Baja"
        ];

        public static readonly List<string> RiskCatalog =
        [
            "Bajo",
            "Medio",
            "Alto"
        ];

        private const int TechnicianRole = 2;


        public static async Task<List<Ticket>> GetAll()
        {
            Supabase.Client client =
                await SupabClient.GetSupabaseClientAsync();

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
                var department = departments
                    .FirstOrDefault(x =>
                        x.Id == ticket.DepartmentId);

                ticket.DepartmentName =
                    department?.Name ?? "No encontrado";

                var creator = users
                    .FirstOrDefault(x =>
                        x.Id == ticket.CreatedBy);

                ticket.CreatedByName = creator != null
                    ? $"{creator.FirstName} {creator.LastName}"
                    : "Desconocido";

                if (ticket.AssignedTo.HasValue)
                {
                    var assignedUser = users
                        .FirstOrDefault(x =>
                            x.Id == ticket.AssignedTo.Value);

                    ticket.AssignedToName =
                        assignedUser != null
                            ? $"{assignedUser.FirstName} {assignedUser.LastName}"
                            : "No asignado";
                }
                else
                {
                    ticket.AssignedToName =
                        "No asignado";
                }
            }

            return tickets;
        }


        public static async Task UpdateTicket(
    long ticketId,
    string status,
    string priority,
    string risk,
    long categoryId,
    int departmentId,
    int? assignedTo,
    DateTime? dueDate,
    User currentUser)
        {
            Supabase.Client client =
                await SupabClient.GetSupabaseClientAsync();

            Ticket? currentTicket =
                await GetByTicketId(ticketId);

            if (currentTicket == null)
            {
                throw new InvalidOperationException(
                    "El ticket no fue encontrado."
                );
            }


            // EMPLEADO
            if (currentUser.RoleId == 3)
            {
                // Solo puede editar sus propios tickets
                if (currentTicket.CreatedBy != currentUser.Id)
                {
                    throw new InvalidOperationException(
                        "No tiene permiso para editar este ticket."
                    );
                }

                // Solo puede editar mientras esté pendiente
                if (currentTicket.Status != "Pendiente")
                {
                    throw new InvalidOperationException(
                        "El ticket solo puede editarse mientras esté pendiente."
                    );
                }

                // El empleado no puede cambiar campos operativos
                status = currentTicket.Status;
                priority = currentTicket.Priority;
                assignedTo = currentTicket.AssignedTo;
            }

            // ADMINISTRADOR
            if (currentUser.RoleId != 1 &&
                currentUser.RoleId != 2 &&
                currentUser.RoleId != 3)
            {
                throw new InvalidOperationException(
                    "El usuario no tiene un rol válido."
                );
            }


            TicketUpdate update = new TicketUpdate
            {
                Id = ticketId,
                Status = status,
                Priority = priority,
                Risk = risk,
                CategoryId = categoryId,
                DepartmentId = departmentId,
                AssignedTo = assignedTo,
                DueDate = dueDate,
                UpdatedAt = DateTime.UtcNow
            };

            await client
                .From<TicketUpdate>()
                .Update(update);
        }


        public static async Task<Ticket?> GetByTicketId(
            long id)
        {
            Supabase.Client client =
                await SupabClient.GetSupabaseClientAsync();

            var result = await client
                .From<Ticket>()
                .Where(x => x.Id == id)
                .Get();

            return result.Model;
        }


        public static async Task UpdateStatus(
    long ticketId,
    string newStatus)
        {
            Supabase.Client client =
                await SupabClient.GetSupabaseClientAsync();

            Ticket? ticket =
                await GetByTicketId(ticketId);

            if (ticket == null)
            {
                throw new InvalidOperationException(
                    "El ticket no fue encontrado."
                );
            }

            // Validar que el estado exista en el catálogo
            if (!StatusCatalog.Contains(newStatus))
            {
                throw new InvalidOperationException(
                    "El estado seleccionado no es válido."
                );
            }

            // Validar transición
            bool validTransition = ticket.Status switch
            {
                "Pendiente" =>
                    newStatus == "En Revisión" ||
                    newStatus == "Cancelado",

                "En Revisión" =>
                    newStatus == "En Progreso" ||
                    newStatus == "Cancelado",

                "En Progreso" =>
                    newStatus == "Finalizado" ||
                    newStatus == "Cancelado",

                "Cancelado" => false,

                "Finalizado" => false,

                _ => false
            };

            if (!validTransition)
            {
                throw new InvalidOperationException(
                    $"No se puede cambiar el estado de " +
                    $"{ticket.Status} a {newStatus}."
                );
            }

            TicketStatusUpdate update =
                new TicketStatusUpdate
                {
                    Id = ticketId,
                    Status = newStatus,
                    UpdatedAt = DateTime.UtcNow
                };

            await client
                .From<TicketStatusUpdate>()
                .Where(x => x.Id == ticketId)
                .Update(update);
        }


        public static async Task<string> GenerateTicketCode(
            int departmentId)
        {
            Supabase.Client client =
                await SupabClient.GetSupabaseClientAsync();

            var department = (await client
                .From<Department>()
                .Where(x => x.Id == departmentId)
                .Get()).Model;

            if (department == null)
            {
                throw new InvalidOperationException(
                    "Departamento no encontrado.");
            }

            var ticketsInDepartment = (await client
                .From<Ticket>()
                .Where(x =>
                    x.DepartmentId == departmentId)
                .Get()).Models;

            int nextNumber = 1;

            if (ticketsInDepartment.Count > 0)
            {
                var numbers = ticketsInDepartment
                    .Select(ticket =>
                    {
                        var parts =
                            ticket.TicketCode.Split('-');

                        return parts.Length == 2 &&
                               int.TryParse(
                                   parts[1],
                                   out int number)
                            ? number
                            : 0;
                    })
                    .ToList();

                nextNumber =
                    numbers.Max() + 1;
            }

            return
                $"{department.Code}-{nextNumber:D4}";
        }


        public static async Task<int?> AssignTechnician()
        {
            Supabase.Client client =
                await SupabClient.GetSupabaseClientAsync();

            var technicians = (await client
                .From<User>()
                .Where(x =>
                    x.RoleId == TechnicianRole)
                .Where(x =>
                    x.IsActive == true)
                .Get()).Models;

            if (technicians.Count == 0)
                return null;

            var activeTickets = (await client
                .From<Ticket>()
                .Get()).Models
                .Where(ticket =>
                    ticket.Status == "Pendiente" ||
                    ticket.Status == "En Revisión" ||
                    ticket.Status == "En Progreso")
                .ToList();

            var loads = technicians
                .Select(technician => new
                {
                    Technician = technician,

                    ActiveCount =
                        activeTickets.Count(ticket =>
                            ticket.AssignedTo ==
                            technician.Id)
                })
                .ToList();

            int minCount =
                loads.Min(x => x.ActiveCount);

            var candidates = loads
                .Where(x =>
                    x.ActiveCount == minCount)
                .ToList();

            var random = new Random();

            var chosen =
                candidates[
                    random.Next(candidates.Count)
                ];

            return chosen.Technician.Id;
        }


        public static async Task<Ticket> CreateTicket(
            Ticket ticket)
        {
            ticket.TicketCode =
                await GenerateTicketCode(
                    ticket.DepartmentId);

            ticket.AssignedTo =
                await AssignTechnician();

            ticket.CreatedAt =
                DateTime.UtcNow;

            ticket.UpdatedAt =
                DateTime.UtcNow;

            Supabase.Client client =
                await SupabClient.GetSupabaseClientAsync();

            var result = await client
                .From<Ticket>()
                .Insert(ticket);

            return result.Models.First();
        }
    }
}