using System;
using System.Collections.Generic;
using TechChallenge.Domain.Enumerations;
using TechChallenge.Domain.ValueObjects;
using TechChallenge.Domain.Core.Primitives;

namespace TechChallenge.Domain.Entities
{
    public sealed class Order : AggregateRoot
    {
        #region Read-Only Fields

        private readonly List<OrderItem> _orderItems = new();

        #endregion

        #region Properties

        public Email CustomerEmail { get; private set; }
        public OrderStatus Status { get; private set; }
        public IReadOnlyCollection<OrderItem> Items => _orderItems.AsReadOnly();
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastUpdatedAt { get; private set; }

        #endregion

        #region Constructors

        private Order()
        { }

        private Order(Email email)
        {
            CustomerEmail = email;
            Status = OrderStatus.New;
            CreatedAt = DateTime.UtcNow;
        }

        #endregion

        #region Private Methods

        private void AddItem(int productId, decimal price, int quantity)
        {
            _orderItems.Add(new OrderItem(productId, price, quantity));
        }

        #endregion

        #region Factory Methods

        public static Order Create(string email, IEnumerable<(int ProductId, decimal Price, int Quantity)> items)
        {
            var order = new Order(Email.Create(email));

            foreach (var item in items)
            {
                order.AddItem(item.ProductId, item.Price, item.Quantity);
            }

            return order;
        }

        #endregion
    }
}
