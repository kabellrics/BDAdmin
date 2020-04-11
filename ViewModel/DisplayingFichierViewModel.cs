using Business;
using Common;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAdmin.ViewModel
{
    public class DisplayingFichierViewModel : ViewModelBase
    {
        private readonly IBusinessFichier _businessFichier;
        private readonly IBusinessSerie _businessSerie;

        public DisplayingFichierViewModel(IBusinessFichier businessFichier, IBusinessSerie businessSerie)
        {
            _businessFichier = businessFichier;
            _businessSerie = businessSerie;
        }
        public async Task Initialize(DisplayedFichier fichier)
        {
            this.Fichier = fichier;
            if (fichier.ParentID.HasValue)
                this.Parent = await _businessSerie.GetById(fichier.ParentID.Value);
        }
        private DisplayedFichier _fichier;
        private DisplayedSerie _parent;
        private bool _isSelected;
        public DisplayedFichier Fichier { get => _fichier; set => _fichier = value; }
        public DisplayedSerie Parent { get => _parent; set => _parent = value; }
        public bool IsSelected { get => _isSelected; set => _isSelected = value; }

    }
}
