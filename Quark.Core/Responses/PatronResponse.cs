namespace Quark.Core.Responses;

public class PatronResponse
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Name => FirstName + " " + LastName;

    public string Address { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Email { get; set; }

    public string Mobile { get; set; }

    public string RegisterId { get; set; }

    public DateTime Issued { get; set; }

    public decimal CurrentFees { get; set; }

    public int CheckoutsCount { get; set; }

    public int CheckoutsPending { get; set; }

    public int MultipleCheckoutLimit { get; set; }
}