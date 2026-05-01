using Reward;

namespace Quest.SpecificQuests.WrenchExperiment
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Quest/Wrench/Objectives/Compare Moment Correctly")]
    public class WrenchCompareMomentObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "wrench"
                   && e.actionId == "compare_correct";
        }
    }
}