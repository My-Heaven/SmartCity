using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SmartCity.Entities
{
    public partial class SmartCityContext : DbContext
    {
        public SmartCityContext()
        {
        }

        public SmartCityContext(DbContextOptions<SmartCityContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAccount> TblAccounts { get; set; } = null!;
        public virtual DbSet<TblBooking> TblBookings { get; set; } = null!;
        public virtual DbSet<TblBookingDetail> TblBookingDetails { get; set; } = null!;
        public virtual DbSet<TblBookingEmp> TblBookingEmps { get; set; } = null!;
        public virtual DbSet<TblCommit> TblCommits { get; set; } = null!;
        public virtual DbSet<TblEmpSkill> TblEmpSkills { get; set; } = null!;
        public virtual DbSet<TblEmployee> TblEmployees { get; set; } = null!;
        public virtual DbSet<TblRole> TblRoles { get; set; } = null!;
        public virtual DbSet<TblService> TblServices { get; set; } = null!;
        public virtual DbSet<TblSkill> TblSkills { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=SmartCity");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAccount>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK__tblAccou__349DA58655B0256E");

                entity.ToTable("tblAccounts");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AccountEmail).HasMaxLength(100);

                entity.Property(e => e.AccountName).HasMaxLength(50);

                entity.Property(e => e.AccountPhone)
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblAccounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__tblAccoun__RoleI__3B75D760");
            });

            modelBuilder.Entity<TblBooking>(entity =>
            {
                entity.HasKey(e => e.BookingId)
                    .HasName("PK__tblBooki__73951ACD55367850");

                entity.ToTable("tblBooking");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.DateOfBooking).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TblBookings)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__tblBookin__Accou__47DBAE45");
            });

            modelBuilder.Entity<TblBookingDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId)
                    .HasName("PK__tblBooki__D3B9D30C1ECB07D5");

                entity.ToTable("tblBookingDetail");

                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.TblBookingDetails)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__tblBookin__Booki__4D94879B");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.TblBookingDetails)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK__tblBookin__Servi__4E88ABD4");
            });

            modelBuilder.Entity<TblBookingEmp>(entity =>
            {
                entity.ToTable("tblBookingEmp");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TblBookingEmps)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBookingEmp_tblAccounts");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.TblBookingEmps)
                    .HasForeignKey(d => d.BookingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBookingEmp_tblBooking");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.TblBookingEmps)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBookingEmp_tblEmployee");
            });

            modelBuilder.Entity<TblCommit>(entity =>
            {
                entity.HasKey(e => e.CommitId)
                    .HasName("PK__tblCommi__73748B526EB13B3B");

                entity.ToTable("tblCommits");

                entity.Property(e => e.CommitId).HasColumnName("CommitID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.CommitName).HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TblCommits)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__tblCommit__Accou__44FF419A");
            });

            modelBuilder.Entity<TblEmpSkill>(entity =>
            {
                entity.HasKey(e => e.EmpSkillId)
                    .HasName("PK__tblEmpSk__726FF0B248FB952C");

                entity.ToTable("tblEmpSkill");

                entity.Property(e => e.EmpSkillId).HasColumnName("EmpSkillID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.SkillId).HasColumnName("SkillID");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.TblEmpSkills)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__tblEmpSki__Emplo__412EB0B6");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.TblEmpSkills)
                    .HasForeignKey(d => d.SkillId)
                    .HasConstraintName("FK__tblEmpSki__Skill__4222D4EF");
            });

            modelBuilder.Entity<TblEmployee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK__tblEmplo__7AD04FF115A9046A");

                entity.ToTable("tblEmployee");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.EmployeeEmail).HasMaxLength(100);

                entity.Property(e => e.EmployeeName).HasMaxLength(50);

                entity.Property(e => e.EmployeePhone)
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.TblEmployees)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__tblEmploy__Accou__3E52440B");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__tblRoles__8AFACE3A2DD556B3");

                entity.ToTable("tblRoles");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblService>(entity =>
            {
                entity.HasKey(e => e.ServiceId)
                    .HasName("PK__tblServi__C51BB0EA9446BF74");

                entity.ToTable("tblService");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.Image).IsUnicode(false);

                entity.Property(e => e.ServiceName).HasMaxLength(50);

                entity.Property(e => e.SkillId).HasColumnName("SkillID");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.TblServices)
                    .HasForeignKey(d => d.SkillId)
                    .HasConstraintName("FK__tblServic__Skill__4AB81AF0");
            });

            modelBuilder.Entity<TblSkill>(entity =>
            {
                entity.HasKey(e => e.SkillId)
                    .HasName("PK__tblSkill__DFA091E71F750F4A");

                entity.ToTable("tblSkills");

                entity.Property(e => e.SkillId).HasColumnName("SkillID");

                entity.Property(e => e.SkillName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
