using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodoApi.Models.Configurations
{
    public class CategoriesConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasIndex(category => category.Title);

            builder.Property(category => category.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(category => category.Description)
                   .HasMaxLength(256);

            builder.HasMany(category => category.Todos) // У Category много Todo
                   .WithOne(todo => todo.Category) // У Todo один Category
                   .HasForeignKey(todo => todo.CategoryId)
                   .OnDelete(DeleteBehavior.SetNull); // При удалении категории CategoryId становится NULL
        }
    }
}
