using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Basket.Data.Configuration
{
    public class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {
            builder.HasKey(sci => sci.Id);

            builder.Property(sci => sci.ProductId)
                .IsRequired();

            builder.Property(sci => sci.Quantity)
                .IsRequired();

            builder.Property(sci => sci.Color);

            builder.Property(sci => sci.Price)
                .IsRequired();

            builder.Property(sci => sci.ProductName)
                .IsRequired();
        }
    }
}
