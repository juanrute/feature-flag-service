using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFlag.Api.Controllers;

[ApiController]
public class AuditController : ControllerBase
{
    [HttpGet(Endpoints.Audit.Get)]
    public async Task<IActionResult> Get(
        [FromRoute] string idOrName,
        CancellationToken token)
    {
        throw await Task.FromException<NotImplementedException>(new NotImplementedException());
    }
}
