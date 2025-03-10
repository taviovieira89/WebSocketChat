using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Define a chave primária
        builder.HasKey(c => c.IdUser).HasName("IdUser");

        // Configura a propriedade email
        builder.Property(u => u.Email)
           .IsRequired()
           .HasMaxLength(200);

        // Configura a propriedade Nascimento como Value Object
        builder.Property(u => u.Password)
          .IsRequired()
          .HasMaxLength(200);

        builder.HasIndex(u => u.Email)
        .IsUnique();
    }
}