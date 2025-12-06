namespace BibliothequeApp.Models;

public class DocumentNonTrouveException : Exception
{
    public DocumentNonTrouveException(string message) : base(message) {}
}
