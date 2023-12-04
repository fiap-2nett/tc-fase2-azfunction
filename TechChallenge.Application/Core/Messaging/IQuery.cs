using MediatR;

namespace TechChallenge.Application.Core.Messaging
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    { }
}
