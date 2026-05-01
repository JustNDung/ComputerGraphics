using Reward;

namespace Quest.SpecificQuests.WrenchExperiment
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Quest/Wrench/Objectives/Increase Force")]
    public class WrenchIncreaseForceObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "wrench"
                   && e.actionId == "increase_force"
                   && e.success;
        }
    }
}