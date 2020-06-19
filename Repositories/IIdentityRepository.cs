using System.Threading.Tasks;
using NetCoreAPI.Models;

namespace NetCoreAPI.Repositories
{
    public interface IIdentityRepository
    {
        Task<AuthenticationResult> RegisterAsync(string userEmail, string password);

        Task<AuthenticationResult> LoginAsync(string userEmail, string password);

        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}