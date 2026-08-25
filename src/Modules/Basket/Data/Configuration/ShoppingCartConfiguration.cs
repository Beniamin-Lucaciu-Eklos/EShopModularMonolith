using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Basket.Data.Configuration
{
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.HasKey(sc => sc.Id);

            builder.HasIndex(sc => sc.UserName)
                   .IsUnique();

            builder.Property(sc => sc.UserName)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(sc => sc.Items)
                .WithOne()
                .HasForeignKey(sci => sci.ShoppingCartId);
        }
    }
}
