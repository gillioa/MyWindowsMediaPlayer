using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfMvvmSample.Model.Classes
{
    /// <summary>
    /// Voici la classe contact !
    /// C'est une classe simple avec des propriétés en get/set
    /// On ne met pas les INotifyPropertychanged ici, ils sont mis dans le view model
    /// </summary>
    public class Contact
    {
        public Uri MediaPath { get; set; }
        public int? ID { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public int? Age { get; set; }
        public bool? Homme { get; set; }
    }

}
