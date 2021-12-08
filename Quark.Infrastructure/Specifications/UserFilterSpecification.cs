using Quark.Core.Specifications.Base;

namespace Quark.Infrastructure.Specifications;

public class UserFilterSpecification : BaseSpecification<ApplicationUser>
{
    public UserFilterSpecification(string searchString)
    {
        if (!string.IsNullOrEmpty(searchString))
        {
            Criteria = p => p.FullName.Contains(searchString) || p.Email.Contains(searchString) || p.PhoneNumber.Contains(searchString) || p.UserName.Contains(searchString);
        }
        else
        {
            Criteria = p => true;
        }
    }
}