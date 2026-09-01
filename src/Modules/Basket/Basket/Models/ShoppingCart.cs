using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Basket.Basket.Models
{
    public class ShoppingCart : Aggregate<Guid>
    {
        public string UserName { get; private set; } = default!;

        private readonly List<ShoppingCartItem> _items = new();

        public IReadOnlyCollection<ShoppingCartItem> Items => _items.AsReadOnly();

        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

        public static ShoppingCart Create(Guid id, string userName)
        {
            ArgumentException.ThrowIfNullOrEmpty(userName);

            var shoppingCart = new ShoppingCart
            {
                Id = id,
                UserName = userName
            };
            return shoppingCart;
        }

        public void AddItem(Guid productId,
            int quantity,
            string color,
            decimal price,
            string productName)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var item = Items.FirstOrDefault(x => x.ProductId == productId);
            if (item is not null)
            {
                item.Quantity += quantity;
                return;
            }

            var addedItem = new ShoppingCartItem(Id, productId, quantity, color, price, productName);
            _items.Add(addedItem);
        }

        public void RemoveItem(Guid productId)
        {
            var item = Items.FirstOrDefault(x => x.ProductId == productId);
            if (item is not null)
            {
                _items.Remove(item);
            }
        }

        //TODO: refactor
        public void AddItemsFromJson(IEnumerable<ShoppingCartItem> items)
        {
            _items.Clear();
            _items.AddRange(items);
        }
    }
}
