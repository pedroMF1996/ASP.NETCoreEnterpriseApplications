using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Pedido.API.Application.DTO;
using NSE.Pedido.API.Application.Queries;
using NSE.WebAPI.Core.Controllers;
using System.Net;

namespace NSE.Pedido.API.Controllers
{
    [Authorize]
    public class VoucherController : MainController
    {
        private readonly IVoucherQuery _voucherQuery;

        public VoucherController(IVoucherQuery voucherQuery)
        {
            _voucherQuery = voucherQuery;
        }

        [HttpGet("voucher/{codigo}")]
        [ProducesResponseType(typeof(VoucherDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ObterPorCodigo(string codigo)
        {
            if(string.IsNullOrEmpty(codigo))
                return NotFound();

            var voucher = await _voucherQuery.ObterVoucherPorCodigo(codigo);

            return voucher == null? NotFound() : CustomResponse(voucher);
        }
    }
}
