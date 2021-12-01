using Quark.Core.Features.Books.Commands;
using Quark.Core.Features.Books.Queries;

namespace Quark.Server.Controllers;

[Route(Routes.BookEndpoints.BaseRoute)]
public class BookController : BaseApiController
{
    [Authorize(Policy = Permissions.Books.View)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        return Ok(await _mediator.Send(new GetBookByIdQuery(id)));
    }

    [Authorize(Policy = Permissions.Books.View)]
    [HttpGet("header/{id}")]
    public async Task<IActionResult> GetHeaderByIdAsync(Guid id)
    {
        return Ok(await _mediator.Send(new GetBookHeaderByIdQuery(id)));
    }

    [Authorize(Policy = Permissions.Books.View)]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(int pageNumber, int pageSize, string searchString, string orderBy = null)
    {
        return Ok(await _mediator.Send(new GetAllBooksQuery(pageNumber, pageSize, searchString, orderBy)));
    }

    [Authorize(Policy = Permissions.Books.Create)]
    [HttpPost]
    public async Task<IActionResult> PostAsync(AddEditBookCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(Policy = Permissions.Books.Create)]
    [HttpPost("header")]
    public async Task<IActionResult> PostHeadersAsync(AddEditBookHeaderCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(Policy = Permissions.Books.Edit)]
    [HttpPost("{barcode}")]
    public async Task<IActionResult> UpdateCondition(ChangeBookConditionCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(Policy = Permissions.Books.Delete)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(await _mediator.Send(new DeleteBookCommand(id)));
    }

    [Authorize(Policy = Permissions.Books.Export)]
    [HttpGet("export")]
    public async Task<IActionResult> ExportToExcelAsync(string searchString = "")
    {
        return Ok(await _mediator.Send(new ExportBooksQuery(searchString)));
    }
}