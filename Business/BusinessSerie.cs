using Common;
using DBConnector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class BusinessSerie : BusinessBase, IBusinessSerie
    {
        public BusinessSerie() : base()
        {
        }
        //public async Task<IEnumerable<DisplayedSerie>> GetAllWithoutParent()
        //{
        //    return MyMapper.Mapper.Map<IEnumerable<DisplayedSerie>>(await ConnectorSerie.GetAllWithoutParent());
        //}
        public async Task AddSerieToSerie(Nullable<int> Idserie, IEnumerable<Serie> series)
        {
            foreach (Serie serie in series)
            {
                serie.ParentID = Idserie;
                await Update(serie);
            }
        }
        public async Task<Serie> GetById(int Id)
        {
            return MyMapper.Mapper.Map<Serie>(await ConnectorSerie.Get(Id));
        }
        public async Task<Serie> FirstAncestor(Serie node)
        {
            return MyMapper.Mapper.Map<Serie>(await ConnectorSerie.FirstAncestor(MyMapper.Mapper.Map<DBSerie>(node)));
        }
        public async Task<IEnumerable<int>> GetAllAncestor(Serie serie)
        {
            return await ConnectorSerie.GetAllAncestor(MyMapper.Mapper.Map<DBSerie>(serie));
        }
        public async Task<IEnumerable<int>> GetAllDescendant(Serie serie)
        {
            return await ConnectorSerie.GetAllDescendant(MyMapper.Mapper.Map<DBSerie>(serie));
        }
        public async Task<IEnumerable<Serie>> GetAll()
        {
            return MyMapper.Mapper.Map<IEnumerable<Serie>>(await ConnectorSerie.GetAll());
        }
        public async Task<IEnumerable<Serie>> GetAllByIdParent(Nullable<int> idparent)
        {
            return MyMapper.Mapper.Map<IEnumerable<Serie>>(await ConnectorSerie.GetAllByParent(idparent));
        }
        public async Task Create(Serie newvalue)
        {
            await ConnectorSerie.Create(MyMapper.Mapper.Map<DBSerie>(newvalue));
        }
        public async Task Update(Serie newvalue)
        {
            await ConnectorSerie.Update(MyMapper.Mapper.Map<DBSerie>(newvalue));
        }
        public async Task<bool> Delete(Serie newvalue)
        {
            return await ConnectorSerie.Delete(MyMapper.Mapper.Map<DBSerie>(newvalue));
        }
    }
}
