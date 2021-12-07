namespace Quark.Core.Specifications;

public class CheckoutFilterSpecification : BaseSpecification<Checkout>
{
    public CheckoutFilterSpecification(string searchString)
    {
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            Criteria = p => p.BookHeaderId.ToString().Contains(searchString) ||
                            p.PatronId.ToString().Contains(searchString) ||
                            p.CheckedOutSince.ToString("dd/MM/yyyy").Contains(searchString) ||
                            p.CheckedOutSince.ToString("dd/MM/yyyy").Contains(searchString) ||
                            p.BookHeader.Book.Name.Contains(searchString) ||
                            p.BookHeader.Barcode.Contains(searchString) ||
                            p.BookHeader.Book.DeweyIndex.Contains(searchString) ||
                            p.Patron.RegisterId.Contains(searchString) ||
                            p.Patron.FirstName.Contains(searchString) ||
                            p.Patron.LastName.Contains(searchString);
        }
        else
        {
            Criteria = p => true;
        }
    }
}