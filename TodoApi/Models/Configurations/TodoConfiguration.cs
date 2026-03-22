using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodoApi.Models.Configurations
{
    public class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.Property(todo => todo.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(todo => todo.Description)
                   .HasMaxLength(256);

            // Индексы для производительности
            //builder.HasIndex(t => t.IsComplete);
        }
    }
}
