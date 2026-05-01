using Reward;

namespace Quest.SpecificQuests.WrenchExperiment
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Quest/Wrench/Objectives/Explore Both Methods")]
    public class WrenchExploreBothObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "wrench"
                   && e.actionId == "explore_both";
        }
    }
}