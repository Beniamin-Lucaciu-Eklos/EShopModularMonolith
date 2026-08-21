using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Catalog.Data.Configurations;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Categories).IsRequired();

        builder.Property(p => p.Description).HasMaxLength(200);

        builder.Property(p => p.ImageFile).HasMaxLength(200);

        builder.Property(p => p.Price).IsRequired();
    }

}
