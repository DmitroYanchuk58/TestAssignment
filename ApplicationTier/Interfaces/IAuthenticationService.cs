using ApplicationTier.Models;

namespace ApplicationTier.Interfaces
{
    public interface IAuthenticationService
    {
        void Register(User user);

        bool Authenticate(string username, string password);
    }
}
