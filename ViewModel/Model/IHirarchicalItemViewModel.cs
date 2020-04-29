using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Common;

namespace BDAdmin.ViewModel.Model
{
    public interface IHirarchicalItemViewModel
    {
        ICommand ChangeImageForSerie { get; }
        ICommand ChangeParentCommand { get; }
        ICommand ShowImgCommand { get; }
        bool Haschanged { get; set; }
        byte[] Image { get; set; }
        bool IsExpanded { get; set; }
        string Name { get; set; }
    }
}