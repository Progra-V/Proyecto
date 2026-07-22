namespace Proyecto.Models.ViewModels
{
    public class ProfileViewModel
    {
        public User CurrentUser { get; set; } = new User();
        public string? Phone { get; set; }
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}