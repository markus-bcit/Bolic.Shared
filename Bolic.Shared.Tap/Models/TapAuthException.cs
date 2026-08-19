namespace Bolic.Shared.Tap.Models;

public class TapAuthException : Exception
{
    public TapError Error { get; }

    public TapAuthException(TapError error) : base(error.Detail)
    {
        Error = error;
    }

    public TapAuthException(TapError error, Exception inner) : base(error.Detail, inner)
    {
        Error = error;
    }
}
