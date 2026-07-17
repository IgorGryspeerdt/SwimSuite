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

        public DbSet<TrainerAttendance> TrainerAttendances => Set<TrainerAttendance>();

        public DbSet<Official> Officials => Set<Official>();

        public DbSet<OfficialDuty> OfficialDuties => Set<OfficialDuty>();

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

            builder.Entity<TrainerAttendance>()
                .HasIndex(attendance => new { attendance.TrainingBlockId, attendance.TrainerId })
                .IsUnique();

            builder.Entity<TrainerAttendance>()
                .HasOne(attendance => attendance.Club)
                .WithMany()
                .HasForeignKey(attendance => attendance.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TrainerAttendance>()
                .HasOne(attendance => attendance.TrainingBlock)
                .WithMany(block => block.TrainerAttendances)
                .HasForeignKey(attendance => attendance.TrainingBlockId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TrainerAttendance>()
                .HasOne(attendance => attendance.Trainer)
                .WithMany(trainer => trainer.Attendances)
                .HasForeignKey(attendance => attendance.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Official>()
                .HasOne(official => official.Club)
                .WithMany(club => club.Officials)
                .HasForeignKey(official => official.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OfficialDuty>()
                .HasOne(duty => duty.Club)
                .WithMany(club => club.OfficialDuties)
                .HasForeignKey(duty => duty.ClubId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OfficialDuty>()
                .HasOne(duty => duty.Official)
                .WithMany(official => official.Duties)
                .HasForeignKey(duty => duty.OfficialId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
