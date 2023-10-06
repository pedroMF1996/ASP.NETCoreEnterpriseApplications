using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSE.Core.Communication;

namespace NSE.WebAPI.Core.Controllers
{
    
    [ApiController]
    public abstract class MainController : Controller
    {

        protected ICollection<string> Errors = new List<string>();

        protected ActionResult CustomResponse(object? result = null)
        {
            if(OperacaoValida()) {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>()
            {
                {"Mensagens", Errors.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary model)
        {
            var errors = model.Values.SelectMany(x => x.Errors);

            foreach (var error in errors)
            {
                AdicionarErroProcessamento(error.ErrorMessage);
            }

            return CustomResponse();
        }

        protected ActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AdicionarErroProcessamento(error.ErrorMessage);
            }

            return CustomResponse();
        }

        protected bool OperacaoValida()
        { 
            return !Errors.Any(); 
        }

        protected void AdicionarErroProcessamento(string erro)
        {
            Errors.Add(erro);
        }

        protected void AdicionarErrosProcessamento(List<ValidationFailure> erros)
        {
            erros.ForEach(erro => AdicionarErroProcessamento(erro.ErrorMessage));
        }
        
        protected bool ResponsePossuiErros(ResponseResult result)
        {
            if (result != null || !result.Errors.Mensagens.Any())
                return false;
            
            foreach (var mensagem in result.Errors.Mensagens)
            {
                AdicionarErroProcessamento(mensagem);
            }

            return true;
        }

        protected ActionResult CustomResponse(ResponseResult resposta)
        {
            ResponsePossuiErros(resposta);

            return CustomResponse();
        }

        protected void LimparErroProcessamento()
        {
            Errors.Clear();
        }
    }
}
