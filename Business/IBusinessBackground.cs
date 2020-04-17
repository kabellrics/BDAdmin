using System.Threading.Tasks;

namespace Business
{
    public interface IBusinessBackground
    {
        Task<byte[]> GetRandomBackground();
    }
}