namespace Quark.Core.Responses;

public class AuthorResponse
{
    public string Author { get; set; }
    public Dictionary<Guid, string> Books { get; set; }
}