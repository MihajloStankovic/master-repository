namespace MovieStore.Models
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public List<string> UserRoles { get; set; }

        public UserRolesViewModel()
        {
            UserRoles = new List<string>();
        }
    }
}
