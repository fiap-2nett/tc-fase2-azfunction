using System;
using System.Collections.Generic;
using TechChallenge.Domain.Errors;
using TechChallenge.Domain.Events;
using TechChallenge.Domain.Extensions;
using TechChallenge.Domain.Enumerations;
using TechChallenge.Domain.ValueObjects;
using TechChallenge.Domain.Core.Primitives;
using TechChallenge.Domain.Core.Primitives.Result;

namespace TechChallenge.Domain.Entities
{
    public sealed class Order : AggregateRoot
    {
        #region Properties

        public Email CustomerEmail { get; private set; }
        public OrderStatus Status { get; private set; }
        public ICollection<OrderItem> Items { get; private set; }
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
            Items ??= new List<OrderItem>();
            Items.Add(new OrderItem(productId, price, quantity));
        }

        #endregion

        #region Public Methods

        public Result Processing()
        {
            if (Status == OrderStatus.Processing)
                return Result.Failure(DomainErrors.Order.AlreadyProcessing);

            Status = OrderStatus.Processing;
            LastUpdatedAt = DateTime.UtcNow;

            return Result.Success();
        }

        public Result Accept()
        {
            if (Status == OrderStatus.Approved)
                return Result.Failure(DomainErrors.Order.AlreadyAccepted);

            if (Status == OrderStatus.Rejected)
                return Result.Failure(DomainErrors.Order.AlreadyRejected);

            Status = OrderStatus.Approved;
            LastUpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new OrderAcceptedDomainEvent(Id));

            return Result.Success();
        }

        public Result Reject()
        {
            if (Status == OrderStatus.Approved)
                return Result.Failure(DomainErrors.Order.AlreadyAccepted);

            if (Status == OrderStatus.Rejected)
                return Result.Failure(DomainErrors.Order.AlreadyRejected);

            Status = OrderStatus.Rejected;
            LastUpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new OrderRejectedDomainEvent(Id));

            return Result.Success();
        }

        #endregion

        #region Factory Methods

        public static Order Create(string email, IEnumerable<(int ProductId, decimal Price, int Quantity)> items)
        {
            var order = new Order(Email.Create(email));
            items.ForEach(item => order.AddItem(item.ProductId, item.Price, item.Quantity));

            order.AddDomainEvent(new OrderCreatedDomainEvent(order));

            return order;
        }

        #endregion
    }
}
