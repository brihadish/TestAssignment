using System.Collections.Generic;
using ApplicationLayer.ReadModels;
using EventFlow.Queries;

namespace ApplicationLayer.Queries
{
    /// <summary>
    /// Query for retrieving all suit alterations.
    /// </summary>
    public sealed class GetSuitAlterationByIdQuery : IQuery<SuitAlterationReadModel>
    {
        public GetSuitAlterationByIdQuery(string suitAlterationId)
        {
            SuitAlterationId = suitAlterationId;
        }

        /// <summary>
        /// Gets or sets the unique identity of the suit alteration.
        /// </summary>
        public string SuitAlterationId { get; }
    }
}
