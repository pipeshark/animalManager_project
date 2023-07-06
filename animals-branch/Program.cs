using System;
using System.Collections;
using System.Text.RegularExpressions;
using Animals;
using MySql.Data.MySqlClient;

namespace Animals
{
    public class Program
    {
        public int var = 0;
        static void Main(string[] args)
        {
            Program obj = new Program();
            ArrayList animales = obj.AnimalBd();


            string choice = null!;

            do
            {
                obj.afficherMenu();
                Console.WriteLine("Faites votre choix");
                choice = Console.ReadLine();
                obj.selectChoice(choice, animales);
            } while (choice != "9");

            Conexion conexion = new Conexion();
            MySqlConnection con = conexion.connectBd();
            obj.ActualiserBD(animales);
            con.Close();
        }

        ArrayList AnimalBd()
        {
            ArrayList animales = new ArrayList();

            Conexion conexion = new Conexion();
            MySqlConnection con = conexion.connectBd();

            string query = "SELECT * FROM ANIMAL";

            MySqlCommand command = new MySqlCommand(query, con);

            try
            {
                MySqlDataReader read = command.ExecuteReader();

                while (read.Read())
                {
                    int id = read.GetInt32("ID");
                    string tipyAnimal = read.GetString("TipyAnimal");
                    string nom = read.GetString("nom");
                    int age = read.GetInt32("Age");
                    decimal poids = read.GetDecimal("Poids");
                    string couleur = read.GetString("couleur");
                    string propietaire = read.GetString("Propietaire");

                    pets animal = new pets(id, tipyAnimal, nom, age, poids, couleur, propietaire);
                    animales.Add(animal);
                }

                read.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur de chargement des animaux dans la base de données  " + ex.Message);
            }

            con.Close();

            return animales;
        }
        private void ActualiserBD(ArrayList animales)
        {

            Conexion conexion = new Conexion();
            MySqlConnection con = conexion.connectBd();

            string queryDelete = "DELETE FROM ANIMAL";
            MySqlCommand fileDelete = new MySqlCommand(queryDelete, con);

            try
            {
                fileDelete.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur lors de la suppression d'animaux existants dans la base de données : " + ex.Message);
                return;
            }

            Console.WriteLine("Base de données correctement mise à jour.");
        }

        private void afficherMenu()
        {
            Console.WriteLine("Menu");
            Console.WriteLine("1. Ajouter un animal");
            Console.WriteLine("2. Voir la liste des animaux en pension");
            Console.WriteLine("3. Voir la liste des propriétaires");
            Console.WriteLine("4. Voir le nombre total d'animaux en pension");
            Console.WriteLine("5. Voir le poids total des animaux en pension");
            Console.WriteLine("6. Voir la liste des animaux d'une couleur");
            Console.WriteLine("7. Retirer un animal de la liste");
            Console.WriteLine("8. Modifier un animal de la liste");
            Console.WriteLine("9. Quitter");
        }

        private void selectChoice(string choice, ArrayList animales)
        {
            Program obj = new Program();

            switch (choice)
            {
                case "1":
                    obj.ajouterUnAnimal(animales);
                    break;
                case "2":
                    obj.voirListeAnimauxPension(animales);
                    break;
                case "3":
                    obj.voirListePropietaire(animales);
                    break;
                case "4":
                    obj.voirNombreTotalAnimaux(animales);
                    break;
                case "5":
                    obj.voirPoidsTotalAnimaux(animales);
                    break;
                case "6":
                    obj.extraireAnimauxSelonCouleur(animales);
                    break;
                case "7":
                    obj.retirerUnAnimalDeListe(animales);

                    break;
                case "8":
                    obj.modifierUnAnimalDeListe(animales);
                    break;
                case "9":
                    break;
                default:
                    Console.WriteLine("Option invalide");
                    break;
            }
        }
        private void ajouterUnAnimal(ArrayList animales)
        {
            string animalType;
            do
            {
                Console.WriteLine("Type d'animal :");
                animalType = Console.ReadLine();
            } while (!Regex.IsMatch(animalType, "^[a-zA-Z]+$"));

            string animalName;
            do
            {
                Console.WriteLine("Nom de l'animal :");
                animalName = Console.ReadLine();
            } while (!Regex.IsMatch(animalName, "^[a-zA-Z]+$"));

            int age;
            do
            {
                Console.WriteLine("Âge :");
            } while (!int.TryParse(Console.ReadLine(), out age) || age <= 0 || age >= 100);

            decimal weight;
            do
            {
                Console.WriteLine("Poids :");
            } while (!decimal.TryParse(Console.ReadLine(), out weight));

            string animalCouleur;
            bool couleurValide = false;

            do
            {
                Console.WriteLine("Couleur (rouge, bleu o violet):");
                animalCouleur = Console.ReadLine();

              
            } while (!(animalCouleur == "rouge" || animalCouleur == "bleu" || animalCouleur == "violet"));

            string ownerName;
            do
            {
                Console.WriteLine("Nom du propriétaire :");
                ownerName = Console.ReadLine();
            } while (!Regex.IsMatch(ownerName, "^[a-zA-Z]+$"));

            pets nuveauAnimal = new pets(0, animalType, animalName, age, weight, animalCouleur, ownerName);

            animales.Add(nuveauAnimal);

            Console.WriteLine("Animal ajouté correctement.");

            Conexion conexion = new Conexion();
            MySqlConnection con = conexion.connectBd();
            ActualiserBD(animales);
            con.Close();

        }

        private void voirListeAnimauxPension(ArrayList animales)
        {

            Console.WriteLine(new string('-', 95));
            Console.WriteLine("{0,-5}{1,-20}{2,-20}{3,-5}{4,-10}{5,-15}{6,-20}",
                              "ID", "Type d'animal", "Nom", "Âge", "Poids", "Couleur", "Propriétaire");

            foreach (pets animal in animales)
            {
                Console.WriteLine("{0,-5}{1,-20}{2,-20}{3,-5}{4,-10}{5,-15}{6,-20}",
                    animal.ID, animal.TipyAnimal, animal.Nom, animal.Age,
                              animal.Poids, animal.Couleur, animal.Propietaire);

                Console.WriteLine(new string('-', 95));
            }

        }
        private void voirListePropietaire(ArrayList animales)
        {
            ArrayList propietaires = new ArrayList();

            foreach (pets animal in animales)
            {
                if (!propietaires.Contains(animal.Propietaire))
                {
                    propietaires.Add(animal.Propietaire);
                }
            }

            Console.WriteLine("Liste de propietaires:");
            foreach (string propietaire in propietaires)
            {
                Console.WriteLine(propietaire);
            }
        }
        private void voirNombreTotalAnimaux(ArrayList animales)
        {
            int totalAnimales = animales.Count;
            Console.WriteLine($" {totalAnimales}");
        }
        private void voirPoidsTotalAnimaux(ArrayList animales)
        {

            decimal poidsTotal = 0;

            foreach (pets animal in animales)
            {
                poidsTotal += animal.Poids;
            }

            Console.WriteLine($"Poids total des animeaux en pensión: {poidsTotal}");
        }

        private void extraireAnimauxSelonCouleur(ArrayList animales)
        {

            Console.WriteLine("Inserer couleur Violet, Bleu ou Rouge");
            string couleur = Console.ReadLine();

            ArrayList animalesSelonCouleur = new ArrayList();

            foreach (pets animal in animales)
            {
                if (animal.Couleur.ToLower() == couleur.ToLower())
                {
                    animalesSelonCouleur.Add(animal);
                }
            }

            Console.WriteLine($"Liste d'animaux de couleur {couleur}:");
            foreach (pets animal in animalesSelonCouleur)
            {
                Console.WriteLine($"ID: {animal.ID}, Tipyanimal: {animal.TipyAnimal}, Nom: {animal.Nom}, " +
                    $"Age: {animal.Age}, Poids: {animal.Poids}, Couleur: {animal.Couleur}, Propietaire: {animal.Propietaire}");
            }
        }
        private void retirerUnAnimalDeListe(ArrayList animales)
        {

            Console.WriteLine("Inserer ID a eliminer");
            int id = Convert.ToInt32(Console.ReadLine());

            pets animalTrouve = null;

            foreach (pets animal in animales)
            {
                if (animal.ID == id)
                {
                    animalTrouve = animal;
                    break;
                }
            }

            if (animalTrouve != null)
            {
                animales.Remove(animalTrouve);
                Console.WriteLine("Animal tué avec succès.");
            }
            else
            {
                Console.WriteLine("Il ny a pas animal ID ");
            }

            ActualiserBD(animales);
        }
        private void modifierUnAnimalDeListe(ArrayList animales)
        {

            Console.WriteLine("Ingrese el ID del animal a modificar:");
            int id = Convert.ToInt32(Console.ReadLine());

            pets animalTrouve = null;

            foreach (pets animal in animales)
            {
                if (animal.ID == id)
                {
                    animalTrouve = animal;
                    break;
                }
            }

            if (animalTrouve != null)
            {
                Console.WriteLine("Ingrese el nuevo nombre del animal:");
                string nuevoNombre = Console.ReadLine();

                Console.WriteLine("Ingrese la nueva edad del animal:");
                int nuevaEdad = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Ingrese el nuevo peso del animal:");
                decimal nuevoPeso = Convert.ToDecimal(Console.ReadLine());

                Console.WriteLine("Ingrese el nuevo color del animal:");
                string nuevoColor = Console.ReadLine();

                Console.WriteLine("Ingrese el nuevo propietario del animal:");
                string nuevoPropietario = Console.ReadLine();

                animalTrouve.Nom = nuevoNombre;
                animalTrouve.Age = nuevaEdad;
                animalTrouve.Poids = nuevoPeso;
                animalTrouve.Couleur = nuevoColor;
                animalTrouve.Propietaire = nuevoPropietario;

                Console.WriteLine("Animal modificado correctamente.");
            }
            else
            {
                Console.WriteLine("No se encontró ningún animal con ese ID.");
            }

            ActualiserBD(animales);
        }
        private void ActualizarBD(ArrayList animales)
        {
            string consultaEliminar = "DELETE FROM ANIMAL";

            Conexion conexion = new Conexion();
            MySqlConnection con = conexion.connectBd();


            MySqlCommand comandoEliminar = new MySqlCommand(consultaEliminar, con);

            try
            {
                comandoEliminar.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur lors de la suppression d'animaux existants dans la base de données : " + ex.Message);
                return;
            }


            foreach (pets animal in animales)
            {
                string consultaInsertar = $"INSERT INTO ANIMAL (TipyAnimal, nom, Age, Poids, couleur, Propietaire) " +
                    $"VALUES ('{animal.TipyAnimal}', '{animal.Nom}', {animal.Age}, {animal.Poids}, '{animal.Couleur}', '{animal.Propietaire}')";

                MySqlCommand comandoInsertar = new MySqlCommand(consultaInsertar, con);

                try
                {
                    comandoInsertar.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Erreur lors de l'insertion d'animaux dans la base de données: " + ex.Message);
                }
            }

            Console.WriteLine("");
        }

    }
}

        
    
   
