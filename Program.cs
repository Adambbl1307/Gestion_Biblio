using BibliothequeApp.Models;
using BibliothequeApp.Services;

Bibliotheque biblio = new();

bool continuer = true;

while (continuer)
{
    Console.WriteLine("\n=== MENU BIBLIOTHÈQUE ===");
    Console.WriteLine("1. Ajouter un document");
    Console.WriteLine("2. Afficher tous les documents");
    Console.WriteLine("3. Rechercher par mot-clé");
    Console.WriteLine("4. Supprimer un document");
    Console.WriteLine("5. Sauvegarder dans un fichier");
    Console.WriteLine("6. Charger depuis un fichier");
    Console.WriteLine("7. Quitter");
    Console.Write("Choix : ");

    string? choix = Console.ReadLine();

    try
    {
        switch (choix)
        {
            case "1":
                Ajouter();
                break;

            case "2":
                biblio.AfficherTous();
                break;

            case "3":
                Console.Write("Mot-clé : ");
                string mot = Console.ReadLine() ?? string.Empty;
                var res = biblio.Rechercher(mot);
                res.ForEach(d => d.AfficherDetails());
                break;

            case "4":
                Console.Write("ID du document à supprimer : ");
                string? idStr = Console.ReadLine();
                if (!Guid.TryParse(idStr, out Guid id))
                {
                    Console.WriteLine("ID invalide.");
                    break;
                }
                biblio.SupprimerDocument(id);
                Console.WriteLine("Document supprimé.");
                break;

            case "5":
                biblio.Sauvegarder("Data/bibliotheque_sauvegarde.txt");
                break;

            case "6":
                biblio.Charger("Data/bibliotheque_sauvegarde.txt");
                break;

            case "7":
                continuer = false;
                break;

            default:
                Console.WriteLine("Choix invalide!");
                break;
        }
    }
    catch (DocumentNonTrouveException ex)
    {
        Console.WriteLine("⚠ " + ex.Message);
    }
    catch (FormatException ex)
    {
        Console.WriteLine("Erreur de format : " + ex.Message);
    }
    catch (IOException ex)
    {
        Console.WriteLine("Erreur d'accès au fichier : " + ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erreur inattendue : " + ex.Message);
    }
}

void Ajouter()
{
    Console.WriteLine("1. Livre");
    Console.WriteLine("2. Magazine");
    Console.WriteLine("3. PDF");

    Console.Write("Type : ");
    string? type = Console.ReadLine();

    Console.Write("Titre : ");
    string titre = Console.ReadLine() ?? string.Empty;
    Console.Write("Auteur : ");
    string auteur = Console.ReadLine() ?? string.Empty;

    Console.Write("Année : ");
    if (!int.TryParse(Console.ReadLine(), out int annee))
    {
        Console.WriteLine("Année invalide.");
        return;
    }

    Document doc = type switch
    {
        "1" => new Livre(titre, auteur, annee, LireInt("Nombre de pages : ")),
        "2" => new Magazine(titre, auteur, annee, LireInt("Numéro : ")),
        "3" => new DocumentPDF(titre, auteur, annee, LireDouble("Taille en Mo : ")),
        _ => throw new Exception("Type incorrect.")
    };

    biblio.AjouterDocument(doc);
    Console.WriteLine("Document ajouté !");
}

int LireInt(string msg)
{
    Console.Write(msg);
    if (!int.TryParse(Console.ReadLine(), out int value))
    {
        throw new FormatException("Valeur numérique invalide.");
    }
    return value;
}

double LireDouble(string msg)
{
    Console.Write(msg);
    if (!double.TryParse(Console.ReadLine(), out double value))
    {
        throw new FormatException("Valeur numérique invalide.");
    }
    return value;
}
