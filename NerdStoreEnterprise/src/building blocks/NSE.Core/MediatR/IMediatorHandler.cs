using FluentValidation.Results;
using NSE.Core.Messages;

namespace NSE.Core.MediatR
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<ValidationResult> EnviarComando<T>(T comando) where T : Command;
    }
}
