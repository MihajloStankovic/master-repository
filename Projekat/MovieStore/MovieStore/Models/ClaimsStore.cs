using System.Security.Claims;

namespace MovieStore.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> TotalClaims = new List<Claim>()
        {
            new Claim("Add Role", "Add Role"),
            new Claim("Delete Role", "Delete Role")
        };
    }
}
