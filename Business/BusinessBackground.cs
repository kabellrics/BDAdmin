using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class BusinessBackground : IBusinessBackground
    {
        private BusinessSerie businessSerie;
        public BusinessBackground()
        {
            businessSerie = new BusinessSerie();
        }

        public async Task<byte[]> GetRandomBackground()
        {
            var elements = (await businessSerie.GetAll()).ToList();
            var random = new Random();
            var randomIndice = random.Next(elements.Count());
            var elebck = elements[randomIndice];
            return elebck.Image;

        }
    }
}
