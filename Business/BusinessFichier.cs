using Common;
using DBConnector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class BusinessFichier : BusinessBase, IBusinessFichier
    {
        public BusinessFichier() : base()
        {
        }
        //public async Task<IEnumerable<DisplayedFichier>> GetAllWithoutParent()
        //{
        //    return MyMapper.Mapper.Map<IEnumerable<DisplayedFichier>>(await ConnectorSerie.GetAllWithoutParent());
        //}
        public async Task AddFileToSerie(Nullable<int> Idserie, IEnumerable<Fichier> fichiers)
        {
            foreach (Fichier fichier in fichiers)
            {
                fichier.ParentID = Idserie;
                await Update(fichier);
            }
        }
        public async Task<Fichier> GetFichierAsync(Fichier fichier)
        {
            return MyMapper.Mapper.Map<Fichier>(await ConnectorFichier.Get(MyMapper.Mapper.Map<DBFichier>(fichier)));
        }
        public async Task<Fichier> GetById(int Id)
        {
            return MyMapper.Mapper.Map<Fichier>(await ConnectorFichier.Get(Id));
        }
        public async Task<IEnumerable<Fichier>> GetAll()
        {
            return MyMapper.Mapper.Map<IEnumerable<Fichier>>(await ConnectorFichier.GetAll());
        }
        public async Task<IEnumerable<Fichier>> GetAllByIdParent(Nullable<int> idparent)
        {
            return MyMapper.Mapper.Map<IEnumerable<Fichier>>(await ConnectorFichier.GetAllByParent(idparent));
        }
        public async Task Create(Fichier newvalue)
        {
             await ConnectorFichier.Create(MyMapper.Mapper.Map<DBFichier>(newvalue));
        }
        public async Task Update(Fichier newvalue)
        {
             await ConnectorFichier.Update(MyMapper.Mapper.Map<DBFichier>(newvalue));
        }
        public async Task<bool> Delete(Fichier newvalue)
        {
            return await ConnectorFichier.Delete(MyMapper.Mapper.Map<DBFichier>(newvalue));
        }
    }
}
