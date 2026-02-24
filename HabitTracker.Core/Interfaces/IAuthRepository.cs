using HabitTracker.Core.Models;

namespace HabitTracker.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> EmailExists(string email);
        Task<User> Register(User user, string password);
        Task<User?> Login(string email, string password);
    }
}
