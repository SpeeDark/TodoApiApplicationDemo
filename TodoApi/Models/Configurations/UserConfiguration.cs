using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodoApi.Models.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(user => user.Email).IsUnique();

            builder.HasMany(user => user.Todos)
                   .WithOne(todo => todo.Owner)
                   .HasForeignKey(todo => todo.OwnerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(user => user.Categories)
                   .WithOne(category => category.Owner)
                   .HasForeignKey(todo => todo.OwnerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(user => user.Name).IsRequired()
                   .HasMaxLength(64);
            builder.Property(user => user.Email).IsRequired();
            builder.Property(user => user.Password).IsRequired();
        }
    }
}
