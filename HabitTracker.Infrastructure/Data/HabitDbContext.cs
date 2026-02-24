using HabitTracker.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Infrastructure.Data;

public class HabitDbContext : DbContext
{
    public HabitDbContext(DbContextOptions<HabitDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Habit> Habits => Set<Habit>();
    public DbSet<HabitCompletion> HabitCompletions => Set<HabitCompletion>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<User>()
            .ToTable("Users")
            .HasKey(x => x.UserId);

        mb.Entity<Habit>()
            .ToTable("Habits")
            .HasKey(x => x.HabitId);

        mb.Entity<HabitCompletion>()
            .ToTable("HabitCompletions")
            .HasKey(x => x.CompletionId);

        mb.Entity<Habit>()
            .HasMany(h => h.Completions)
            .WithOne(c => c.Habit)
            .HasForeignKey(c => c.HabitId);

        mb.Entity<User>()
            .HasMany(u => u.Habits)
            .WithOne(h => h.User)
            .HasForeignKey(h => h.UserId);
    }
}