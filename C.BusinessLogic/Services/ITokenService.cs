using A.Contracts.Entities;

namespace C.BusinessLogic.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
