using System;
using System.Collections;

namespace UsageCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            SortedList lstEmployé = new SortedList();
            lstEmployé.Add("Raci" +
                "ne", new Employé { Nom = "Racine", 
                Age= 32 });
            Employé layssa = new Employé()
            {
                Nom = "layssa",
                Age = 25
            };
            lstEmployé.Add(layssa.Nom, layssa);
            Employé unEmployé = (Employé)lstEmployé["Racine"];

            if (unEmployé != null)
            {
                Console.WriteLine($"Nom: {unEmployé.Nom}, Age:" +
                    $" {unEmployé.Age.ToString()}");
            }

            Console.ReadLine();

            foreach (DictionaryEntry employé in lstEmployé)
            {
                Employé autreEmployé = (Employé)employé.Value;
                Console.WriteLine($"Nom: {autreEmployé.Nom}, Age: {autreEmployé.Age.ToString()}");
            }
            Console.ReadLine();
        }
    }
}
