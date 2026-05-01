
using Reward;
using UnityEngine;

namespace Quest.SpecificQuests.DoorHandleExperiment
{
    [CreateAssetMenu(menuName = "Quest/Handle/Objectives/Try Far Position")]
    public class DoorHandleTryFarObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "handle"
                   && e.actionId == "far";
        }
    }
}