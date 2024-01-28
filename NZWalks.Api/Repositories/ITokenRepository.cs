using Microsoft.AspNetCore.Identity;

namespace NZWalks.Api.Repositories
{
    public interface ITokenRepository
    {
        string CreateToken(IdentityUser user, List<string> roles);
    }
}
