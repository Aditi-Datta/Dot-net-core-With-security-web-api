using webapisolution.Models;

namespace webapisolution.Repositories
{
    public interface IUserRepository
    {
        User ValidateUser(string username, string password);
    }
}
