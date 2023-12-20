using System.Linq;
using FluentValidation;

using TechChallenge.Domain.Extensions;
using TechChallenge.Application.Core.Errors;
using TechChallenge.Application.Core.Extensions;
using TechChallenge.Application.Orders.Contracts;

namespace TechChallenge.Application.Orders.Commands.CreateOrder
{
    public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(order => order.CustomerEmail)
                .NotEmpty().WithError(ValidationErrors.CreateOrder.CustomerEmailIsRequired);

            RuleFor(order => order.Items)
                .NotEmpty().WithError(ValidationErrors.CreateOrder.ItemsIsRequired);

            RuleFor(order => order.Items)
                .Must(items => items.GroupBy(item => item.ProductId).All(g => g.Count() == 1))
                .WithError(ValidationErrors.CreateOrder.DuplicatedItems).When(order => !order.Items.IsNullOrEmpty());

            RuleForEach(order => order.Items)
                .SetValidator(new OrderItemValidatior()).When(order => !order.Items.IsNullOrEmpty());
        }
    }

    public sealed class OrderItemValidatior : AbstractValidator<OrderItem>
    {
        public OrderItemValidatior()
        {
            RuleFor(item => item.ProductId)
                .NotEmpty().WithError(ValidationErrors.CreateOrder.CustomerEmailIsRequired);

            RuleFor(item => item.Quantity)
                .GreaterThan(0).WithError(ValidationErrors.CreateOrder.QuantityGreaterThanZero);
        }
    }
}
