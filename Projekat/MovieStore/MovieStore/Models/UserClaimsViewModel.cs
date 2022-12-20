namespace MovieStore.Models
{
    public class UserClaimsViewModel
    {
        public string UserId { get; set; }
        public List<UserClaim> UserClaims { get; set; }

        public UserClaimsViewModel() 
        {
            UserClaims = new List<UserClaim>();
        }
    }
}
