namespace BibliothequeApp.Models;

public class DocumentPDF : Document
{
    public double TailleEnMo { get; set; }

    public DocumentPDF(string titre, string auteur, int annee, double taille)
        : base(titre, auteur, annee)
    {
        TailleEnMo = taille;
    }

    public override void AfficherDetails()
    {
        Console.WriteLine($"[PDF] {Titre} - {Auteur}, {Annee}, {TailleEnMo} Mo (Id: {Id})");
    }

    public override string ToString()
    {
        return base.ToString() + $";{TailleEnMo}";
    }
}
