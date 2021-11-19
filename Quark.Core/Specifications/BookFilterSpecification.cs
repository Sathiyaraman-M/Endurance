namespace Quark.Core.Specifications;

public class BookFilterSpecification : BaseSpecification<Book>
{
    public BookFilterSpecification(string searchString)
    {
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            Criteria = p => p.Author.Contains(searchString) || p.Barcode.Contains(searchString) || p.Description.Contains(searchString) || p.DeweyIndex.Contains(searchString) || p.Edition.Contains(searchString) || p.ISBN.Contains(searchString) || p.Name.Contains(searchString) || p.PublicationYear.ToString().Contains(searchString) || p.Publisher.Contains(searchString);
        }
        else
        {
            Criteria = p => true;
        }
    }
}