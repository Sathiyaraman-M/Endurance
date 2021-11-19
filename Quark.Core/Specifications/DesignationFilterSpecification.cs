namespace Quark.Core.Specifications;

public class DesignationFilterSpecification : BaseSpecification<Designation>
{
    public DesignationFilterSpecification(string searchString)
    {
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            Criteria = p => p.Name.Contains(searchString);
        }
        else
        {
            Criteria = p => true;
        }
    }
}