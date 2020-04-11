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
    public class DisplayingSerieViewModel : ViewModelBase
    {
        private readonly IBusinessFichier _businessFichier;
        private readonly IBusinessSerie _businessSerie;

        public DisplayingSerieViewModel(IBusinessFichier businessFichier, IBusinessSerie businessSerie)
        {
            _businessFichier = businessFichier;
            _businessSerie = businessSerie;
            
        }
        public async Task Initialize(DisplayedSerie serie)
        {
            this.Serie = serie;
            if(serie.ParentID.HasValue)
            this.Parent = await _businessSerie.GetById(serie.ParentID.Value);
            var childSerie = await _businessSerie.GetAllByIdParent(serie.ID);
            var childFichier = await _businessFichier.GetAllByIdParent(serie.ID);
            this.NumberOfChild = childSerie.Count() + childFichier.Count();
        }
        private int _numberOfChild;
        private DisplayedSerie _serie;
        private DisplayedSerie _parent;
        private bool _isSelected;
        public String Name { get => _serie.Name; set => _serie.Name = value; }
        public byte[] Image { get => _serie.Image; }
        public byte[] ParentImage { get => _parent?.Image; }
        public DisplayedSerie Parent { get => _parent; set => _parent = value; }
        public DisplayedSerie Serie { get => _serie; set => _serie = value; }
        public int NumberOfChild { get => _numberOfChild; set => _numberOfChild = value; }
        public bool IsSelected { get => _isSelected; set => _isSelected = value; }
    }
}
