using System.Collections.Generic;
using ApplicationLayer.ReadModels;
using EventFlow.Queries;

namespace ApplicationLayer.Queries
{
    /// <summary>
    /// Query for retrieving all suit alterations.
    /// </summary>
    public sealed class GetAllSuitAlterationsQuery : IQuery<IReadOnlyCollection<SuitAlterationReadModel>>
    {
    }
}
