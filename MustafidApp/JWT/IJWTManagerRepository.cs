using MustafidAppModels.Models;

namespace MustafidApp.JWT
{
    public interface IJWTManagerRepository
    {
        string Authenticate(string Phone, out string RefToken);
    }
}
