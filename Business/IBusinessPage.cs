using System.Collections.Generic;
using System.Threading.Tasks;
using Common;

namespace Business
{
    public interface IBusinessPage
    {
        Task Create(Page newvalue);
        Task<bool> Delete(Page newvalue);
        Task<IEnumerable<Page>> GetAllByIdParent(int idFichier);
        Task<Page> GetById(int Id);
        Task Update(Page newvalue);
    }
}