using Shared.DDD;

namespace Basket.Basket.Models
{
    public class ShoppingCart : Aggregate<Guid>
    {
        public string UserName { get; private set; } = default!;
        private readonly List<ShoppingCartItem> _items = new();
        public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();

        public decimal  TotalPrice => Items.Sum(x => x.Price * x.Quantity);


        public static ShoppingCart Create(Guid id , string userName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userName);

            var shoppingCart = new ShoppingCart()
            {
                Id = id,
                UserName = userName,
            };
            return shoppingCart;
        }

        public void AddItem(Guid productId, int quantity, string color, decimal price, string productName)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var existingItme = _items.FirstOrDefault(i=> i.Id == productId);

            if (existingItme != null)
            {
                existingItme.Quantity = quantity;
            }
            else
            {
                var newItem = new ShoppingCartItem(Id, productId, quantity, color, price, productName);
                _items.Add(newItem);
            }
        }
        public void RemoveItem(Guid productId)
        {
            var existingItme = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItme != null)
            {
                _items.Remove(existingItme);
            }
        }
    }
}
