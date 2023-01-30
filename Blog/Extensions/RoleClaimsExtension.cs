using Blog.Domain.Entities;
using System.Security.Claims;

namespace Blog.Extensions
{
    public static class RoleClaimsExtension
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var result = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email), //User.Identity.Name
            };
            result.AddRange(
                user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Slug))); //User.IsInRole

            return result;
        }
    }
}
