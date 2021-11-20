namespace Quark.Infrastructure.DbContexts;

public class LibraryDbContext : AuditableContext
{
    private readonly ICurrentUserService _currentUserService;

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options, ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    //TODO: DbSets Here>>
    public DbSet<Designation> Designations { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Patron> Patrons { get; set; }
    public DbSet<Checkout> Checkouts { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTime.Now;
                    entry.Entity.CreatedByUserId = _currentUserService.UserId;
                    entry.Entity.CreatedBy = _currentUserService.UserName;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTime.Now;
                    entry.Entity.LastModifiedByUserId = _currentUserService.UserId;
                    entry.Entity.LastModifiedBy = _currentUserService.UserName;
                    break;
            }
        }
        if (_currentUserService.UserId == null)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        else
        {
            return await base.SaveChangesAsync(_currentUserService.UserId, cancellationToken);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
        .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,2)");
        }
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable(name: "Users", "Identity");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        builder.Entity<ApplicationRole>(entity =>
        {
            entity.ToTable(name: "Roles", "Identity");
        });
        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("UserRoles", "Identity");
        });

        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("UserClaims", "Identity");
        });

        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("UserLogins", "Identity");
        });

        builder.Entity<ApplicationRoleClaim>(entity =>
        {
            entity.ToTable(name: "RoleClaims", "Identity");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.RoleClaims)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("UserTokens", "Identity");
        });
    }
}