using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfMvvmSample.Model.Classes;

namespace WpfMvvmSample.Model.Services
{
    /// <summary>
    /// Voici le modèle !
    /// Le modèle est ici un service qui va gérer nos contacts
    /// </summary>
    public class ServiceContact
    {
        /// <summary>
        /// On charge aléatoirement une fiche
        /// </summary>
        /// <returns></returns>
        public Contact Charger()
        {
            bool randomSex = GetRandomBooleanValue();

            if (randomSex)
            {
                // on créé en dur un contact homme
                return new Contact { MediaPath = new Uri("C:\\Users\\Windaub\\Downloads\\badload.mp3"), ID = 1, Nom = "DUPUIS", Prenom = "Paul", Age = 30, Homme = true };
            }
            else
            {
                // on créé en dur un contact femme
                return new Contact { MediaPath = new Uri("C:\\Users\\Windaub\\Downloads\\badload.mp3"), ID = 2, Nom = "DURAND", Prenom = "Sophie", Age = 27, Homme = false };
            }
        }

        /// <summary>
        /// Obtient de façon aléatoire un booléen
        /// </summary>
        /// <returns></returns>
        private bool GetRandomBooleanValue()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);

            int randomValue = rnd.Next(0, 100);

            if (randomValue > 50)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

}
