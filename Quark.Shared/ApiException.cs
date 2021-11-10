namespace Quark.Shared;

public class ApiException : Exception
{
    public ApiException() : base()
    {

    }

    public ApiException(string message) : base(message)
    {

    }

    public ApiException(string message, params object[] args)
        : base(string.Format(message, args))
    {

    }
}