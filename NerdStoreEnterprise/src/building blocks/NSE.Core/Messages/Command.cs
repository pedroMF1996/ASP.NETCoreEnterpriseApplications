using FluentValidation.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace NSE.Core.Messages
{
    public abstract class Command : Message, IRequest<ValidationResult>
    {
        [JsonIgnore]
        public DateTime Timestamp { get; private set; }

        [JsonIgnore]
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.UtcNow;
            ValidationResult = new ValidationResult();
        }

        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}
