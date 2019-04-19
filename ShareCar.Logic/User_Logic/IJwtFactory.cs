using System.Security.Claims;
using System.Threading.Tasks;

namespace ShareCar.Logic.User_Logic
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}