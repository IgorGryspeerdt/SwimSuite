using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SwimSuite.Models;

namespace SwimSuite.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Club> Clubs => Set<Club>();

        public DbSet<TrainingGroup> TrainingGroups => Set<TrainingGroup>();

        public DbSet<TrainingBlock> TrainingBlocks => Set<TrainingBlock>();

        public DbSet<Trainer> Trainers => Set<Trainer>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TrainingGroup>()
                .HasOne(group => group.Club)
                .WithMany(club => club.TrainingGroups)
                .HasForeignKey(group => group.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TrainingBlock>()
                .HasOne(block => block.Club)
                .WithMany(club => club.TrainingBlocks)
                .HasForeignKey(block => block.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TrainingBlock>()
                .HasOne(block => block.TrainingGroup)
                .WithMany(group => group.TrainingBlocks)
                .HasForeignKey(block => block.TrainingGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Trainer>()
                .HasOne(trainer => trainer.Club)
                .WithMany(club => club.Trainers)
                .HasForeignKey(trainer => trainer.ClubId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
