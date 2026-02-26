using HabitTracker.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Infrastructure.Data;

public class HabitDbContext : DbContext
{
    public HabitDbContext(DbContextOptions<HabitDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Habit> Habits => Set<Habit>();
    public DbSet<HabitCompletion> HabitCompletions => Set<HabitCompletion>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // ── USERS
        mb.Entity<User>(e =>
        {
            e.ToTable("Users");
            e.HasKey(u => u.UserId);
            e.Property(u => u.UserId).HasColumnName("user_id");
            e.Property(u => u.Email).HasColumnName("email");
            e.Property(u => u.Username).HasColumnName("username");
            e.Property(u => u.PasswordHash).HasColumnName("password_hash");
            e.Property(u => u.CreatedAt).HasColumnName("created_at");
        });

        // ── HABITS
        mb.Entity<Habit>(e =>
        {
            e.ToTable("Habits");
            e.HasKey(h => h.HabitId);
            e.Property(h => h.HabitId).HasColumnName("habit_id");
            e.Property(h => h.UserId).HasColumnName("user_id");
            e.Property(h => h.Name).HasColumnName("name");
            e.Property(h => h.Icon).HasColumnName("icon");
            e.Property(h => h.Frequency).HasColumnName("frequency");
            e.Property(h => h.CreatedAt).HasColumnName("created_at");
            e.Property(h => h.IsDeleted).HasColumnName("is_deleted");
            e.HasQueryFilter(h => !h.IsDeleted);
        });

        // ── HABIT COMPLETIONS
        mb.Entity<HabitCompletion>(e =>
        {
            e.ToTable("HabitCompletions");
            e.HasKey(c => c.CompletionId);
            e.Property(c => c.CompletionId).HasColumnName("completion_id");
            e.Property(c => c.HabitId).HasColumnName("habit_id");
            e.Property(c => c.CompletedDate).HasColumnName("completed_date");
            e.HasIndex(c => new { c.HabitId, c.CompletedDate }).IsUnique();
        });
    }
}