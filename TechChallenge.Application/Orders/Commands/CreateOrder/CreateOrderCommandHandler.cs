using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using TechChallenge.Domain.Entities;
using TechChallenge.Domain.Repositories;
using TechChallenge.Application.Core.Messaging;
using TechChallenge.Domain.Core.Primitives.Result;
using TechChallenge.Application.Core.Abstractions.Data;

namespace TechChallenge.Application.Orders.Commands.CreateOrder
{
    internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Result<int>>
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

        public async Task<Result<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = Order.Create(request.CustomerEmail, await GetOrderItemProductAsync(request.Items));

            _orderRepository.Insert(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(order.Id);
        }

        private async Task<IEnumerable<(int ProductId, decimal Price, int Quantity)>> GetOrderItemProductAsync(IEnumerable<Dtos.OrderItem> items)
        {
            var output = new List<(int ProductId, decimal Price, int Quantity)>();

            foreach (var item in items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                output.Add((item.ProductId, product.Price, item.Quantity));
            }

            return output;
        }
    }
}
