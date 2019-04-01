using System.Collections.Generic;

namespace DomainModel.SuitAlteration
{
    /// <summary>
    /// Flyweight for getting suit alteration strategies.
    /// </summary>
    internal static class SuitAlterationStrategies
    {
        private static Dictionary<SuitSleeveAlterationChoice, ISuitAlterationStrategy> _sleeveAlterationStrategies;
        private static Dictionary<SuitTrouserAlterationChoice, ISuitAlterationStrategy> _trouserAlterationStrategies;

        static SuitAlterationStrategies()
        {
            _sleeveAlterationStrategies = new Dictionary<SuitSleeveAlterationChoice, ISuitAlterationStrategy>();
            _trouserAlterationStrategies = new Dictionary<SuitTrouserAlterationChoice, ISuitAlterationStrategy>();

            _sleeveAlterationStrategies.Add(SuitSleeveAlterationChoice.Left, new SuitSleeveAlterationStrategy(SuitSleeveAlterationChoice.Left));
            _sleeveAlterationStrategies.Add(SuitSleeveAlterationChoice.Right, new SuitSleeveAlterationStrategy(SuitSleeveAlterationChoice.Right));
            _sleeveAlterationStrategies.Add(SuitSleeveAlterationChoice.Both, new SuitSleeveAlterationStrategy(SuitSleeveAlterationChoice.Both));

            _trouserAlterationStrategies.Add(SuitTrouserAlterationChoice.Left, new SuitTrouserAlterationStrategy(SuitTrouserAlterationChoice.Left));
            _trouserAlterationStrategies.Add(SuitTrouserAlterationChoice.Right, new SuitTrouserAlterationStrategy(SuitTrouserAlterationChoice.Right));
            _trouserAlterationStrategies.Add(SuitTrouserAlterationChoice.Both, new SuitTrouserAlterationStrategy(SuitTrouserAlterationChoice.Both));
        }

        public static ISuitAlterationStrategy GetStrategy(SuitTrouserAlterationChoice suitTrouserAlterationChoice)
        {
            return _trouserAlterationStrategies[suitTrouserAlterationChoice];
        }

        public static ISuitAlterationStrategy GetStrategy(SuitSleeveAlterationChoice suitSleeveAlterationChoice)
        {
            return _sleeveAlterationStrategies[suitSleeveAlterationChoice];
        }
    }
}
