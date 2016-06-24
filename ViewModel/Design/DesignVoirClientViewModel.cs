using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using WpfMvvmSample.ViewModel.Interface;

namespace WpfMvvmSample.ViewModel.Design
{
    /// <summary>
    /// Ici on créé un contact de test qui va servir uniquement au mode design dans Visual Studio
    /// </summary>
    public class DesignVoirContactViewModel : IVoirContactViewModel
    {
        public int? ID
        {
            get { return 1; }
            set { }
        }

        public string Nom
        {
            get { return "DURAND"; }
            set { }
        }
        
        public string Prenom
        {
            get { return "Nicolas"; }
            set { }
        }

        public int? Age
        {
            get { return 30; }
            set { }
        }

        public bool? Homme
        {
            get { return true; }
            set { }
        }
    }

}
