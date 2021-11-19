using MediatR;

namespace Quark.Server.Controllers.Utility;

[ApiController]
public class BaseApiController : ControllerBase
{
    private IMediator _mediatorInstance;
    protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
}