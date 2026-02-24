using BCrypt.Net;
using HabitTracker.Core.Interfaces;
using HabitTracker.Core.Models;
using HabitTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HabitDbContext _db;

        public AuthRepository(HabitDbContext db)
        {
            _db = db;
        }

        public async Task<bool> EmailExists(string email)
            => await _db.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());

        public async Task<User> Register(User user, string password)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<User?> Login(string email, string password)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

            if (user == null) return null;

            var valid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            return valid ? user : null;
        }
    }
}
