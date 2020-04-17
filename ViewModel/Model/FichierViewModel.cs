using Business;
using Common;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAdmin.ViewModel.Model
{
    public class FichierViewModel : ViewModelBase
    {
        private readonly IBusinessFichier _businessFichier;
        private readonly IBusinessSerie _businessSerie;

        public FichierViewModel(IBusinessFichier businessFichier, IBusinessSerie businessSerie)
        {
            _businessFichier = businessFichier;
            _businessSerie = businessSerie;
        }
        public async Task Initialize(Fichier fichier)
        {
            this.Fichier = fichier;
        }
        private Fichier _fichier;
        private Serie _parent;
        private bool _isSelected;
        public Fichier Fichier { get => _fichier; set => _fichier = value; }
        public bool IsSelected { get => _isSelected; set => _isSelected = value; }

    }
}
