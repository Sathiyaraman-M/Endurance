namespace Quark.Core.Responses;

public class CheckoutResponse
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string BookBarcode { get; set; }
    public string DeweyIndex { get; set; }
    public string BookName { get; set; }
    public Guid PatronId { get; set; }
    public string PatronRegisterId { get; set; }
    public string PatronName { get; set; }
    public DateTime CheckedOutSince { get; set; }
    public DateTime ExpectedCheckInDate { get; set; }
    public DateTime? CheckedOutUntil { get; set; }
}