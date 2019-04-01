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
    /// Executes <see cref="GetSuitAlterationsByStatusQuery"/>.
    /// </summary>
    public sealed class GetSuitAlterationsByStatusQueryHandler : IQueryHandler<GetSuitAlterationsByStatusQuery, IReadOnlyCollection<SuitAlterationReadModel>>
    {
        private readonly IInMemoryReadStore<SuitAlterationReadModel> _readStore;

        public GetSuitAlterationsByStatusQueryHandler(IInMemoryReadStore<SuitAlterationReadModel> readStore)
        {
            _readStore = readStore;
        }

        public async Task<IReadOnlyCollection<SuitAlterationReadModel>> ExecuteQueryAsync(GetSuitAlterationsByStatusQuery query, CancellationToken cancellationToken)
        {
            return await _readStore.FindAsync(model => model.Status == query.SuitAlterationStatus, cancellationToken);
        }
    }
}
