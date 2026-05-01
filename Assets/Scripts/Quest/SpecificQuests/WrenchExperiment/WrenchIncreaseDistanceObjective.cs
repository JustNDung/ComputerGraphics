using Reward;

namespace Quest.SpecificQuests.WrenchExperiment
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Quest/Wrench/Objectives/Increase Distance")]
    public class WrenchIncreaseDistanceObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "wrench"
                   && e.actionId == "increase_distance"
                   && e.success;
        }
    }
}