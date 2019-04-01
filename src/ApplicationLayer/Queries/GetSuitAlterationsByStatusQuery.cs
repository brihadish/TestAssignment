using System.Collections.Generic;
using ApplicationLayer.ReadModels;
using EventFlow.Queries;

namespace ApplicationLayer.Queries
{
    /// <summary>
    /// Query for retrieving suit alterations based on status.
    /// </summary>
    public sealed class GetSuitAlterationsByStatusQuery : IQuery<IReadOnlyCollection<SuitAlterationReadModel>>
    {
        public GetSuitAlterationsByStatusQuery(string suitAlterationStatus)
        {
            SuitAlterationStatus = suitAlterationStatus?.ToLowerInvariant();
        }

        /// <summary>
        /// Gets the suit alteration status on which to filter the records.
        /// </summary>
        public string SuitAlterationStatus { get; }
    }
}
