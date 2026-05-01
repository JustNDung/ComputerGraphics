using Reward;

namespace Quest.SpecificQuests.WrenchExperiment
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Quest/Wrench/Objectives/Complete Experiment")]
    public class WrenchCompleteObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "wrench"
                   && e.actionId == "complete"
                   && e.success;
        }
    }
}