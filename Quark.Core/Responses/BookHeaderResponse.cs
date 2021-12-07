namespace Quark.Core.Responses;

public class BookHeaderResponse
{
    public Guid Id { get; set; }
    public string Barcode { get; set; }
    public string Condition { get; set; }
    public Guid BookId { get; set; }
}