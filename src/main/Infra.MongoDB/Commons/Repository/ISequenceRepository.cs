using System.Threading.Tasks;
using MongoDB.Driver;

namespace Infra.MongoDB.Commons.Repository;

public interface ISequenceRepository<T> where T : ISequence
{
    Task<ISequence> GetSequenceAndUpdate(FilterDefinition<T> filterDefinition);
}