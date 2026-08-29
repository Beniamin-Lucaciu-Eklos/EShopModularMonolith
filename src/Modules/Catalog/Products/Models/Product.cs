
namespace Eshop.Modules.Catalog.Products.Models;

public class Product : Aggregate<Guid>
{
    public string Name { get; private set; } = null!;

    public List<string> Categories { get; private set; } = [];

    public string Description { get; private set; } = null!;

    public string ImageFile { get; private set; } = null!;

    public decimal Price { get; private set; }

    public static Product Create(
        Guid id,
        string name,
        List<string> categories,
        string description,
        string imageFile,
        decimal price)
    {
        Validate(name, price);

        Product product = new Product
        {
            Id = id,
            Name = name,
            Categories = categories,
            Description = description,
            ImageFile = imageFile,
            Price = price
        };

        product.AddDomainEvent(new ProductCreatedEvent(product));
        return product;
    }

    public void Update(
        string name,
        List<string> categories,
        string description,
        string imageFile,
        decimal price)
    {
        Validate(name, price);

        Name = name;
        Categories = categories;
        Description = description;
        ImageFile = imageFile;
        Price = price;

        if (Price != price)
        {
            Price = price;
            AddDomainEvent(new ProductPriceChangedEvent(this));
        }
    }

    private static void Validate(string name, decimal price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
    }
}
