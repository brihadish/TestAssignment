using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.Queries;
using ApplicationLayer.ReadModels;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;

namespace ApplicationLayer.QueryHandlers
{
    /// <summary>
    /// Executes <see cref="GetSuitAlterationsByStatusQuery"/>.
    /// // TODO before deployment :- Use a repository which internally uses a durable read model store instead of 'IInMemoryReadStore'.
    /// </summary>
    public sealed class GetSuitAlterationByIdQueryHandler : IQueryHandler<GetSuitAlterationByIdQuery, SuitAlterationReadModel>
    {
        private readonly IInMemoryReadStore<SuitAlterationReadModel> _readStore;

        public GetSuitAlterationByIdQueryHandler(IInMemoryReadStore<SuitAlterationReadModel> readStore)
        {
            _readStore = readStore;
        }

        public async Task<SuitAlterationReadModel> ExecuteQueryAsync(GetSuitAlterationByIdQuery query, CancellationToken cancellationToken)
        {
            var result = await _readStore.FindAsync(model => model.SuitAlterationId == query.SuitAlterationId, cancellationToken);
            return result.SingleOrDefault();
        }
    }
}
