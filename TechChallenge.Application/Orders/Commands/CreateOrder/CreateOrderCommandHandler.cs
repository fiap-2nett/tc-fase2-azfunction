using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using TechChallenge.Domain.ValueObjects;
using TechChallenge.Domain.Repositories;
using TechChallenge.Application.Core.Messaging;
using TechChallenge.Domain.Core.Primitives.Result;
using TechChallenge.Application.Core.Abstractions.Data;

namespace TechChallenge.Application.Orders.Commands.CreateOrder
{
    internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Result<int?>>
    {
        #region Read-Only Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        #endregion

        #region Constructors

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        #endregion

        public async Task<Result<int?>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var emailResult = Email.Create(request.CustomerEmail);
            if (emailResult.IsFailure)
                return Result.Failure<int?>(emailResult.Error);

            var orderItemsResult = await GetOrderItemProductAsync(request.Items);
            if (orderItemsResult.IsFailure)
                return Result.Failure<int?>(orderItemsResult.Error);

            var order = Domain.Entities.Order.Create(emailResult.Value, orderItemsResult.Value);

            _orderRepository.Insert(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success<int?>(order.Id);
        }

        private async Task<Result<ICollection<Domain.Entities.OrderItem>>> GetOrderItemProductAsync(IEnumerable<Contracts.OrderItem> items)
        {
            List<Domain.Entities.OrderItem> output = new();

            foreach (var item in items)
            {
                var orderItemResult = await Domain.Entities.OrderItem.CreateAsync(_productRepository, item.ProductId, item.Quantity);
                if (orderItemResult.IsFailure)
                    return Result.Failure<ICollection<Domain.Entities.OrderItem>>(orderItemResult.Error);

                output.Add(orderItemResult.Value);
            }

            return output;
        }
    }
}
