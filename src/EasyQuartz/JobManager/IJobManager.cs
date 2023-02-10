using System.Threading.Tasks;

namespace EasyQuartz
{
    public interface IJobManager
    {
        Task RunJobAsync(string jobKey);
    }
}