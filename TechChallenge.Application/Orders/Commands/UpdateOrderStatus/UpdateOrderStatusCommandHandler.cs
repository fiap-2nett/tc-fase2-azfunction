using System;
using System.Threading;
using System.Threading.Tasks;
using TechChallenge.Domain.Repositories;
using TechChallenge.Application.Core.Messaging;

namespace TechChallenge.Application.Orders.Commands.UpdateOrderStatus
{
    internal sealed class UpdateOrderStatusCommandHandler : ICommandHandler<UpdateOrderStatusCommand, UpdateOrderStatusCommand.Response>
    {
        #region Read-Only Fields

        private readonly IOrderRepository _orderRepository;

        #endregion

        #region Constructors

        public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        #endregion

        public async Task<UpdateOrderStatusCommand.Response> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
