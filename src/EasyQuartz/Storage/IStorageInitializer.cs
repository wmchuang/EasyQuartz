using System.Threading;
using System.Threading.Tasks;

namespace EasyQuartz.Storage;

/// <summary>
/// 持久化库初始化
/// </summary>
public interface IStorageInitializer
{
    /// <summary>
    /// 初始化库
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task InitializeAsync(CancellationToken cancellationToken);

    string GetJobTableName();

    string GetLogTableName();
}