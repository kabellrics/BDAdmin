using System.Collections.Generic;
using System.Threading.Tasks;
using Common;

namespace Business
{
    public interface IBusinessSerie
    {
        Task AddSerieToSerie(int? Idserie, IEnumerable<Serie> series);
        Task Create(Serie newvalue);
        Task<bool> Delete(Serie newvalue);
        Task<IEnumerable<Serie>> GetAll();
        Task<IEnumerable<Serie>> GetAllByIdParent(int? idparent);
        Task<Serie> GetById(int Id);
        Task Update(Serie newvalue);
        Task<IEnumerable<int>> GetAllAncestor(Serie serie);
        Task<IEnumerable<int>> GetAllDescendant(Serie serie);
        Task<Serie> FirstAncestor(Serie node);
    }
}