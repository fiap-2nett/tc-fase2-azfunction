using System;
using System.Threading;
using System.Threading.Tasks;

using TechChallenge.Domain.Events;
using TechChallenge.Domain.Extensions;
using TechChallenge.Domain.Core.Events;
using TechChallenge.Domain.Repositories;
using TechChallenge.Application.Core.Abstractions.Data;

namespace TechChallenge.Application.Orders.Events.OrderCreated
{
    internal sealed class OrderCreatedDomainEventHandler
        : IDomainEventHandler<OrderCreatedDomainEvent>
    {
        #region Read-Only Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        #endregion

        #region Constructors

        public OrderCreatedDomainEventHandler(IUnitOfWork unitOfWork,
            IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        #endregion

        public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(notification.Order.Id);

            foreach (var item in order.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                product.RemoveQuantity(item.Quantity);
            }

            order.Processing();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
