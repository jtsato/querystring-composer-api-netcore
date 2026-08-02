using System.Threading.Tasks;

namespace Infra.MongoDB.Commons.Repository;

public interface IIndexInitializer
{
    Task EnsureIndexesAsync();
}
