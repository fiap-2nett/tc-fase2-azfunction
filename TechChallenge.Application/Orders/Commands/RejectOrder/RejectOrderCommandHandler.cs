using System;
using System.Threading;
using System.Threading.Tasks;
using TechChallenge.Domain.Errors;
using TechChallenge.Domain.Repositories;
using TechChallenge.Application.Core.Messaging;
using TechChallenge.Domain.Core.Primitives.Result;
using TechChallenge.Application.Core.Abstractions.Data;

namespace TechChallenge.Application.Orders.Commands.RejectOrder
{
    internal sealed class RejectOrderCommandHandler : ICommandHandler<RejectOrderCommand, Result>
    {
        #region Read-Only Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _orderRepository;

        #endregion

        #region Constructors

        public RejectOrderCommandHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        #endregion

        public async Task<Result> Handle(RejectOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order is null)
                return Result.Failure(DomainErrors.Order.NotFound);

            var rejectedResult = order.Reject();
            if (rejectedResult.IsFailure)
                return Result.Failure(rejectedResult.Error);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
