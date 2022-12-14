using Core.Security.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Persistence.Contexts
{
    public class BaseDbContext : DbContext
    {
        protected IConfiguration Configuration { get; set; }

        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<Technology> Technologies { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<EmailAuthenticator> EmailAuthenticators { get; set; }
        public DbSet<OtpAuthenticator> OtpAuthenticators { get; set; }

        public DbSet<GithubAccount> GithubAccounts { get; set; }

        public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgrammingLanguage>(a =>
            {
                a.ToTable("ProgrammingLanguages").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(a => a.Name).HasColumnName("Name").IsRequired(true);
                a.HasMany(p => p.Technologies);
            });
            ProgrammingLanguage[] programmingLanguagesEntitySeeds = { new(1, "Java"), new(2, "C#") };
            modelBuilder.Entity<ProgrammingLanguage>().HasData(programmingLanguagesEntitySeeds);


            modelBuilder.Entity<Technology>(a =>
              {
                  a.ToTable("Technologies").HasKey(k => k.Id);
                  a.Property(p => p.Id).HasColumnName("Id");
                  a.Property(p => p.ProgrammingLanguageId).HasColumnName("ProgrammingLanguageId");
                  a.Property(p => p.Name).HasColumnName("Name").HasMaxLength(20).IsRequired(true);
                  a.HasOne(p => p.ProgrammingLanguage);
              });

            Technology[] technologiesEntitySeeds = { new(1, 1, "Spring"), new(2, 1, "Hibernate"), new(3, 2, "Entity Framework") };
            modelBuilder.Entity<Technology>().HasData(technologiesEntitySeeds);

            modelBuilder.Entity<User>(a =>
            {
                a.ToTable("Users").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.FirstName).HasColumnName("FirstName").IsRequired(true).HasMaxLength(20);
                a.Property(p => p.LastName).HasColumnName("LastName").IsRequired(true).HasMaxLength(20);
                a.Property(p => p.Email).HasColumnName("Email").IsRequired(true);
                a.Property(p => p.PasswordHash).HasColumnName("PasswordHash");
                a.Property(p => p.PasswordSalt).HasColumnName("PasswordSalt");
                a.Property(p => p.Status).HasColumnName("Status").HasDefaultValue(true);
                a.Property(p => p.AuthenticatorType).HasColumnName("AuthenticatorType");

                a.HasMany(p => p.RefreshTokens);
                a.HasMany(p => p.UserOperationClaims);
            });

            modelBuilder.Entity<OperationClaim>(a =>
            {
                a.ToTable("OperationClaims").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.Name).HasColumnName("Name").IsRequired(true).HasMaxLength(10);

            });

            modelBuilder.Entity<UserOperationClaim>(a =>
            {
                a.ToTable("UserOperationClaims").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.UserId).HasColumnName("UserId");
                a.Property(p => p.OperationClaimId).HasColumnName("OperationClaimId");

                a.HasOne(p => p.User);
                a.HasOne(p => p.OperationClaim);
            });

            modelBuilder.Entity<RefreshToken>(a =>
            {
                a.ToTable("RefreshTokens").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.UserId).HasColumnName("UserId");
                a.Property(p => p.ReplacedByToken).HasColumnName("ReplacedByToken");
                a.Property(p => p.RevokedByIp).HasColumnName("RevokedByIp");
                a.Property(p => p.Created).HasColumnName("Created");
                a.Property(p => p.CreatedByIp).HasColumnName("CreatedByIp");
                a.Property(p => p.Expires).HasColumnName("Expires");
                a.Property(p => p.Token).HasColumnName("Token");
                a.Property(p => p.RevokedByIp).HasColumnName("RevokedByIp");
                a.Property(p => p.ReasonRevoked).HasColumnName("ReasonRevoked");

                a.HasOne(p => p.User);
            });

            modelBuilder.Entity<EmailAuthenticator>(a =>
            {
                a.ToTable("EmailAuthenticators").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.ActivationKey).HasColumnName("ActivationKey");
                a.Property(p => p.IsVerified).HasColumnName("IsVerified");
                a.Property(p => p.UserId).HasColumnName("UserId");

                a.HasOne(p => p.User);
            });

            modelBuilder.Entity<OtpAuthenticator>(a => {
                a.ToTable("OtpAuthenticators").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.UserId).HasColumnName("UserId");
                a.Property(p => p.SecretKey).HasColumnName("SecretKey");
                a.Property(p => p.IsVerified).HasColumnName("IsVerified");

                a.HasOne(p => p.User);
            });


            modelBuilder.Entity<GithubAccount>(a =>
            {
                a.ToTable("GithubAccounts").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.UserId).HasColumnName("UserId").IsRequired(true);
                a.Property(p => p.Url).HasColumnName("Url").IsRequired(true);

                a.HasOne(p => p.User);

            });


        }

    }
}
