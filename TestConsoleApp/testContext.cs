using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TestConsoleApp
{
    public partial class testContext : DbContext
    {
        public virtual DbSet<Persion> Persion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql(@"Host=localhost;Database=test;Username=testuser;Password=123654");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Persion>(entity =>
            {
                entity.ToTable("persion", "business");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v1()");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Hiredate)
                    .HasColumnName("hiredate")
                    .HasColumnType("date");

                entity.Property(e => e.Lastupdatetime)
                    .HasColumnName("lastupdatetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Seq)
                    .HasColumnName("seq")
                    .HasDefaultValueSql("nextval('business.persion_seq_seq'::regclass)");

                entity.Property(e => e.Wage)
                    .HasColumnName("wage")
                    .HasColumnType("money")
                    .HasDefaultValueSql("0.00");
            });
        }
    }
}
