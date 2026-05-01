
using Reward;

namespace Quest.SpecificQuests.DoorHandleExperiment
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Quest/Handle/Objectives/Try Near Position")]
    public class DoorHandleTryNearObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "handle"
                   && e.actionId == "near";
        }
    }
}