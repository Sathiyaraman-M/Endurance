namespace Quark.Core.Responses;

public class DashboardResponse
{
    public int BooksCount { get; set; }
    public int BookItemsCount { get; set; }
    public int PatronsCount { get; set; }
    public int CheckoutsCount { get; set; }
    public int CheckInPending { get; set; }
    public int CheckoutTodayCount { get; set; }
    public int CheckInTodayCount { get; set; }
}