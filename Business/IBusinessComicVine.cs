using Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business
{
    public interface IBusinessComicVine
    {
        Task<List<ComicVineResult>> GetProposalForFichier(String Name);
    }
}