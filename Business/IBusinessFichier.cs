using System.Collections.Generic;
using System.Threading.Tasks;
using Common;

namespace Business
{
    public interface IBusinessFichier
    {
        Task AddFileToSerie(int? Idserie, IEnumerable<Fichier> fichiers);
        Task Create(Fichier newvalue);
        Task<bool> Delete(Fichier newvalue);
        Task<IEnumerable<Fichier>> GetAll();
        Task<IEnumerable<Fichier>> GetAllByIdParent(int? idparent);
        Task<Fichier> GetById(int Id);
        Task Update(Fichier newvalue);
    }
}