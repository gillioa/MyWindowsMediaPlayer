using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WpfMvvmSample.ViewModel.Interface
{
    public interface IVoirContactViewModel
    {
        int? ID { get; set; }
        string Nom { get; set; }
        string Prenom { get; set; }
        int? Age { get; set; }
        bool? Homme { get; set; }
    }
}
