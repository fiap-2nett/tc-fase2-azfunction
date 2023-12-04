using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TechChallenge.Application.Core.Abstractions.Data;
using TechChallenge.Application.Core.Messaging;
using TechChallenge.Domain.Entities;
using TechChallenge.Domain.Repositories;

namespace TechChallenge.Application.Orders.Commands.CreateOrder
{
    internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, (CreateOrderCommand.Response Response, int OrderId)>
    {
        #region Read-Only Fields

        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;

        #endregion

        #region Constructors

        public CreateOrderCommandHandler(ILogger<CreateOrderCommandHandler> logger, IUnitOfWork unitOfWork, IOrderRepository orderRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        #endregion

        public async Task<(CreateOrderCommand.Response Response, int OrderId)> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = Order.Create(request.CustomerEmail, request.Items.Select(item => (item.ProductId, 1M, item.Quantity)));

                _orderRepository.Insert(order);
                await _unitOfWork.SaveChangesAsync();

                return (CreateOrderCommand.Response.Successful, order.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return (CreateOrderCommand.Response.Error, default);
            }

        }
    }
}
