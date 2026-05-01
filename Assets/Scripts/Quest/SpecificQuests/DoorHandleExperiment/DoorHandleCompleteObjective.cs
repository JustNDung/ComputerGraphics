
using Reward;
using UnityEngine;

namespace Quest.SpecificQuests.DoorHandleExperiment
{
    [CreateAssetMenu(menuName = "Quest/Handle/Objectives/Complete Experiment")]
    public class DoorHandleCompleteObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "handle"
                   && e.actionId == "complete"
                   && e.success;
        }
    }
}