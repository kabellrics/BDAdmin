using System.Threading.Tasks;

namespace Business
{
    public interface IBusinessFile
    {
        Task AnalyseFile(string path);
        Task AnalyseFolder(string path);
    }
}