using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.Queries;
using ApplicationLayer.ReadModels;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;

namespace ApplicationLayer.QueryHandlers
{
    /// <summary>
    /// Executes <see cref="GetAllSuitAlterationsQuery"/>.
    /// // TODO before deployment :- Use a repository which internally uses a durable read model store instead of 'IInMemoryReadStore'.
    /// </summary>
    public sealed class GetAllSuitAlterationsQueryHandler : IQueryHandler<GetAllSuitAlterationsQuery, IReadOnlyCollection<SuitAlterationReadModel>>
    {
        private readonly IInMemoryReadStore<SuitAlterationReadModel> _readStore;

        public GetAllSuitAlterationsQueryHandler(IInMemoryReadStore<SuitAlterationReadModel> readStore)
        {
            _readStore = readStore;
        }

        public async Task<IReadOnlyCollection<SuitAlterationReadModel>> ExecuteQueryAsync(GetAllSuitAlterationsQuery query, CancellationToken cancellationToken)
        {
            return await _readStore.FindAsync(model => true, cancellationToken);
        }
    }
}
