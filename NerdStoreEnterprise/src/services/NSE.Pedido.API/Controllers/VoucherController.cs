using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.WebAPI.Core.Controllers;

namespace NSE.Pedido.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class VoucherController : MainController
    {

    }
}
