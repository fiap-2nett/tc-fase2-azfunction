using MediatR;

namespace TechChallenge.Application.Core.Messaging
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    { }
}
