using Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animals
{
       public class pets
    {
       
            public int ID { get; set; }
            public string TipyAnimal { get; set; }
            public string Nom { get; set; }
            public int Age { get; set; }
            public decimal Poids { get; set; }
            public string Couleur { get; set; }
            public string Propietaire { get; set; }

            
            public pets (int id, string tipyAnimal, string nom, int age, decimal poids, string couleur, string propietaire)
            {
                ID = id;
                TipyAnimal = tipyAnimal;
                Nom = nom;
                Age = age;
                Poids = poids;
                Couleur = couleur;
                Propietaire = propietaire;
                
            }
        }
}
