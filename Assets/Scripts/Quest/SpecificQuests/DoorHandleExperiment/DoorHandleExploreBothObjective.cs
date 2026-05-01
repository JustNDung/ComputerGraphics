
using Reward;
using UnityEngine;

namespace Quest.SpecificQuests.DoorHandleExperiment
{
    [CreateAssetMenu(menuName = "Quest/Handle/Objectives/Explore Both Positions")]
    public class DoorHandleExploreBothObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "handle"
                   && e.actionId == "explore_both";
        }
    }
}