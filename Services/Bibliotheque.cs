using BibliothequeApp.Models;

namespace BibliothequeApp.Services;

public class Bibliotheque
{
    private readonly List<Document> documents = new();

    public void AjouterDocument(Document d)
    {
        documents.Add(d);
    }

    public void SupprimerDocument(Guid id)
    {
        var doc = documents.FirstOrDefault(x => x.Id == id);

        if (doc == null)
            throw new DocumentNonTrouveException("Document introuvable.");

        documents.Remove(doc);
    }

    public List<Document> Rechercher(string motCle)
    {
        var result = documents
            .Where(d => d.Titre.Contains(motCle, StringComparison.OrdinalIgnoreCase)
                     || d.Auteur.Contains(motCle, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (result.Count == 0)
            throw new DocumentNonTrouveException("Aucun document trouvé.");

        return result;
    }

    public void AfficherTous()
    {
        if (documents.Count == 0)
        {
            Console.WriteLine("Aucun document dans la bibliothèque.");
            return;
        }

        foreach (var d in documents)
            d.AfficherDetails();
    }

    public void Sauvegarder(string chemin)
    {
        try
        {
            string? directory = Path.GetDirectoryName(chemin);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using FileStream fs = new FileStream(chemin, FileMode.Create);
            using StreamWriter writer = new StreamWriter(fs);

            foreach (var doc in documents)
                writer.WriteLine(doc.ToString());

            Console.WriteLine("Sauvegarde réussie !");
        }
        catch (IOException ex)
        {
            Console.WriteLine("Erreur d'accès au fichier : " + ex.Message);
            throw;
        }
    }

    public void Charger(string chemin)
    {
        try
        {
            if (!File.Exists(chemin))
                throw new FileNotFoundException("Fichier introuvable.");

            documents.Clear();

            using FileStream fs = new FileStream(chemin, FileMode.Open);
            using StreamReader reader = new StreamReader(fs);

            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(';');
                if (parts.Length < 6)
                    throw new FormatException("Format incorrect dans le fichier.");

                string type = parts[0];
                string titre = parts[2];
                string auteur = parts[3];

                if (!int.TryParse(parts[4], out int annee))
                    throw new FormatException("Année non valide dans le fichier.");

                Document doc = type switch
                {
                    "Livre" => new Livre(titre, auteur, annee, int.Parse(parts[5])),
                    "Magazine" => new Magazine(titre, auteur, annee, int.Parse(parts[5])),
                    "DocumentPDF" => new DocumentPDF(titre, auteur, annee, double.Parse(parts[5])),
                    _ => throw new FormatException("Type de document inconnu.")
                };

                documents.Add(doc);
            }

            Console.WriteLine("Chargement terminé !");
        }
        catch
        {
            // on ré-affiche dans Program la vraie erreur
            throw;
        }
    }
}
