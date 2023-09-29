using FluentValidation.Results;

namespace NSE.Core.Messages.Integration
{
    public class ResponseMessage : Message 
    {
        public ValidationResult ValidationResult { get; private set; }

        public ResponseMessage(ValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }
    }
}
