using Quark.Core.Domain.Entities;
using Quark.Core.Specifications.Base;

namespace Quark.Core.Specifications
{
    public class CheckoutFilterSpecification : BaseSpecification<Checkout>
    {
        public CheckoutFilterSpecification(string searchString)
        {
            if(!string.IsNullOrWhiteSpace(searchString))
            {
                Criteria = p => p.BookId.ToString().Contains(searchString) || 
                                p.PatronId.ToString().Contains(searchString) || 
                                p.CheckedOutSince.ToString("dd/MM/yyyy").Contains(searchString) || 
                                p.CheckedOutSince.ToString("dd/MM/yyyy").Contains(searchString) || 
                                p.Book.Name.Contains(searchString) ||
                                p.Book.Barcode.Contains(searchString) ||
                                p.Book.DeweyIndex.Contains(searchString) || 
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
}