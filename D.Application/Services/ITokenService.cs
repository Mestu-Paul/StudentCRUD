using A.Contracts.Models;

namespace D.Application.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
