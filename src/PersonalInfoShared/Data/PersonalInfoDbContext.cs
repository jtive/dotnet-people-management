using Microsoft.EntityFrameworkCore;
using PersonalInfoShared.Models;

namespace PersonalInfoShared.Data;

public class PersonalInfoDbContext : DbContext
{
    public PersonalInfoDbContext(DbContextOptions<PersonalInfoDbContext> options) : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<CreditCard> CreditCards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Person entity
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.SSN).HasMaxLength(11);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");

            // Configure table with check constraint for valid SSN format if provided
            entity.ToTable(t => t.HasCheckConstraint("chk_ssn_format", 
                "(\"SSN\" IS NULL) OR (LENGTH(\"SSN\") = 9 AND \"SSN\" ~ '^[0-9]{9}$') OR (LENGTH(\"SSN\") = 11 AND \"SSN\" ~ '^[0-9]{3}-[0-9]{2}-[0-9]{4}$')"));
        });

        // Configure Address entity
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AddressType).IsRequired().HasMaxLength(20);
            entity.Property(e => e.StreetAddress).IsRequired().HasMaxLength(200);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.Property(e => e.State).IsRequired().HasMaxLength(2);
            entity.Property(e => e.ZipCode).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Country).HasMaxLength(2).HasDefaultValue("US");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");

            // Configure relationship with Person
            entity.HasOne(e => e.Person)
                  .WithMany(p => p.Addresses)
                  .HasForeignKey(e => e.PersonId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure CreditCard entity
        modelBuilder.Entity<CreditCard>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CardType).IsRequired().HasMaxLength(20);
            entity.Property(e => e.LastFourDigits).IsRequired().HasMaxLength(4);
            entity.Property(e => e.ExpirationMonth).IsRequired();
            entity.Property(e => e.ExpirationYear).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");

            // Configure relationship with Person
            entity.HasOne(e => e.Person)
                  .WithMany(p => p.CreditCards)
                  .HasForeignKey(e => e.PersonId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure indexes for better performance
        modelBuilder.Entity<Address>()
            .HasIndex(e => e.PersonId)
            .HasDatabaseName("IX_Addresses_PersonId");

        modelBuilder.Entity<CreditCard>()
            .HasIndex(e => e.PersonId)
            .HasDatabaseName("IX_CreditCards_PersonId");
    }
}
