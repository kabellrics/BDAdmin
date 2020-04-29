using Common;
using DBConnector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class BusinessPage : BusinessBase, IBusinessPage
    {
        public BusinessPage() : base()
        {
        }

        public async Task<Page> GetById(int Id)
        {
            return MyMapper.Mapper.Map<Page>(await ConnectorPage.Get(Id));
        }
        public async Task<IEnumerable<Page>> GetAllByIdParent(int idFichier)
        {
            return MyMapper.Mapper.Map<IEnumerable<Page>>(await ConnectorPage.GetAllByFichier(idFichier));
        }
        public async Task Create(Page newvalue)
        {
            await ConnectorPage.Create(MyMapper.Mapper.Map<DBPage>(newvalue));
        }
        public async Task Update(Page newvalue)
        {
            await ConnectorPage.Update(MyMapper.Mapper.Map<DBPage>(newvalue));
        }
        public async Task<bool> Delete(Page newvalue)
        {
            return await ConnectorPage.Delete(MyMapper.Mapper.Map<DBPage>(newvalue));
        }
    }
}
