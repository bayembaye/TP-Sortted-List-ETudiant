using System;
using System.Collections;
using System.Globalization;

namespace UsageCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            SortedList lstEtudiant = new SortedList();

            Console.Write("Combien d'étudiants à saisir ? ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Nombre invalide. Arrêt.");
                return;
            }

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"\nÉtudiant #{i + 1}");

                int no;
                while (true)
                {
                    Console.Write("NO (numéro d'ordre) : ");
                    string noStr = Console.ReadLine();
                    if (int.TryParse(noStr, out no)) break;
                    Console.WriteLine("NO invalide. Réessayer.");
                }

                Console.Write("Prénom : ");
                string prenom = Console.ReadLine() ?? string.Empty;

                Console.Write("Nom : ");
                string nom = Console.ReadLine() ?? string.Empty;

                double noteCC;
                while (true)
                {
                    Console.Write("Note CC : ");
                    string s = Console.ReadLine();
                    if (double.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out noteCC)) break;
                    if (double.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out noteCC)) break;
                    Console.WriteLine("Note invalide. Réessayer (ex: 12.5 ou 12,5).");
                }

                double noteDevoir;
                while (true)
                {
                    Console.Write("Note Devoir : ");
                    string s = Console.ReadLine();
                    if (double.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out noteDevoir)) break;
                    if (double.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out noteDevoir)) break;
                    Console.WriteLine("Note invalide. Réessayer (ex: 14.0 ou 14,0).");
                }

                var etu = new Etudiant
                {
                    NO = no,
                    Prenom = prenom,
                    Nom = nom,
                    NoteCC = noteCC,
                    NoteDevoir = noteDevoir
                };

                if (lstEtudiant.ContainsKey(no))
                {
                    Console.WriteLine($"La clé {no} existe déjà. La valeur sera remplacée.");
                    lstEtudiant[no] = etu;
                }
                else
                {
                    lstEtudiant.Add(no, etu);
                }
            }

            Console.WriteLine("\nAppuyer sur Entrée pour configurer l'affichage des étudiants ");
            Console.ReadLine();

            // generer une liste d'étudiants à partir du SortedList
            var etuList = new System.Collections.Generic.List<Etudiant>();
            foreach (DictionaryEntry entry in lstEtudiant)
            {
                etuList.Add((Etudiant)entry.Value);
            }

            int countEtudiants = etuList.Count;
            if (countEtudiants == 0)
            {
                Console.WriteLine("Aucun étudiant à afficher.");
                return;
            }

            // Calcul des moyennes
            double[] moyennes = new double[countEtudiants];
            double totalMoyennes = 0.0;
            for (int i = 0; i < countEtudiants; i++)
            {
                double m = etuList[i].NoteCC * 0.33 + etuList[i].NoteDevoir * 0.67;
                moyennes[i] = Math.Round(m, 2);
                totalMoyennes += moyennes[i];
            }

            //Nombre de ligne entre 5 et 15
            int pageSize = 5;
            while (true)
            {
                Console.Write($"Nombre de lignes par page (5-15) [default {pageSize}]: ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) break;
                if (int.TryParse(input, out int p) && p >= 5 && p <= 15)
                {
                    pageSize = p;
                    break;
                }
                Console.WriteLine("Valeur invalide. Entrez un entier entre 5 et 15.");
            }

            int totalPages = (countEtudiants + pageSize - 1) / pageSize;

            int currentPage = 0;
            bool quit = false;

            while (!quit)
            {
                Console.Clear();
                Console.WriteLine($"--- Affichage Paginé des etudiants {currentPage + 1} / {totalPages} ---");
                int start = currentPage * pageSize;
                int end = Math.Min(start + pageSize, countEtudiants);
                for (int i = start; i < end; i++)
                {
                    var e = etuList[i];
                    Console.WriteLine($"NO: {e.NO}, Nom: {e.Nom}, Prénom: {e.Prenom}, NoteCC: {e.NoteCC}, NoteDevoir: {e.NoteDevoir}, Moyenne: {moyennes[i]:F2}");
                }

                // Recuperer le choix de l'utilisateur pour aller a suivant ou precedent
                Console.WriteLine();
                string options = "[S]uivant";
                if (currentPage > 0) options += "/[P]recedent";
                options += "/[Q]uit";
                Console.Write($"Options {options} : ");
                string choice = Console.ReadLine() ?? string.Empty;
                choice = choice.Trim().ToUpperInvariant();

                if (choice == "Q" || choice == "QUIT")
                {
                    quit = true;
                    break;
                }

                if (choice == "P" || choice == "PRECEDENT")
                {
                    if (currentPage > 0) currentPage--;
                    else Console.WriteLine("Déjà à la première page.");
                }
                else // aller a la page suivante par defaut
                {
                    if (currentPage < totalPages - 1) currentPage++;
                    else
                    {
                        // last page and user requested next -> finish
                        break;
                    }
                }
            }

            double moyenneClasse = Math.Round(totalMoyennes / countEtudiants, 2);
            Console.WriteLine($"\nMoyenne de la classe: {moyenneClasse:F2}");

            Console.WriteLine("Appuyer sur Entrée pour quitter.");
            Console.ReadLine();
        }
    }
}
