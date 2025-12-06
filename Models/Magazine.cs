namespace BibliothequeApp.Models;

public class Magazine : Document
{
    public int Numero { get; set; }

    public Magazine(string titre, string auteur, int annee, int numero)
        : base(titre, auteur, annee)
    {
        Numero = numero;
    }

    public override void AfficherDetails()
    {
        Console.WriteLine($"[MAGAZINE] {Titre} - {Auteur}, {Annee}, Numéro {Numero} (Id: {Id})");
    }

    public override string ToString()
    {
        return base.ToString() + $";{Numero}";
    }
}
