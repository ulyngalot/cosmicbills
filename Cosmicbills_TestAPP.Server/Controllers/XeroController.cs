using Cosmicbills_TestAPP.Server.Accounting.Xero;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Cosmicbills_TestAPP.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class XeroController : ControllerBase
    {
        private readonly ILogger<XeroController> _logger;

        public XeroController(ILogger<XeroController> logger)
        {
            _logger = logger;
        }

        [HttpGet("XeroAuthConnect")]
        public IActionResult XeroAuthConnect()
        {
            return Redirect(XeroProxy.Authenticate());
        }

        [HttpGet("XeroAuthCallBack")]
        public async Task<IActionResult> XeroAuthCallBack([FromQuery] string code, [FromQuery] string state)
        {
            var xeroToken = await XeroProxy.GetSetXeroToken(code, state);
            var redirectUrl = $"https://localhost:4200/?xeroToken={JsonConvert.SerializeObject(xeroToken)}";

            return Redirect(redirectUrl);
        }


        [HttpGet("GetCustomers")]
        public async Task<IActionResult> GetCustomersAsync([FromQuery] string accessToken = "", [FromQuery] string tenantId = "")
        {
            return Ok(await XeroProxy.GetAllCustomersAsync(accessToken, tenantId, 0));
        }

        [HttpGet("GetSuppliers")]
        public async Task<IActionResult> GetSuppliers([FromQuery] string accessToken = "", [FromQuery] string tenantId = "")
        {
            return Ok(await XeroProxy.GetAllSuppliersAsync(accessToken, tenantId, 0));
        }

        [HttpGet("GetCOA")]
        public async Task<IActionResult> GetCOA([FromQuery] string accessToken = "", [FromQuery] string tenantId = "")
        {
            return Ok(await XeroProxy.GetAllChartOfAccountsAsync(accessToken, tenantId, 0));
        }
    }
}
