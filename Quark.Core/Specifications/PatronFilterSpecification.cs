namespace Quark.Core.Specifications;

public class PatronFilterSpecification : BaseSpecification<Patron>
{
    public PatronFilterSpecification(string searchString)
    {
        if (!string.IsNullOrEmpty(searchString))
        {
            Criteria = p => p.FirstName.Contains(searchString) || p.LastName.Contains(searchString) || p.Email.Contains(searchString) || p.RegisterId.Contains(searchString) || p.Mobile.Contains(searchString) || p.Address.Contains(searchString);
        }
        else
        {
            Criteria = p => true;
        }
    }
}