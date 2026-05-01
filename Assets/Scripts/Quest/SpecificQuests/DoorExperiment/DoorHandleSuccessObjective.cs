using Reward;

namespace Quest.SpecificQuests.DoorExperiment
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Quest/Door/Objectives/Open At Handle")]
    public class DoorHandleSuccessObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "door"
                   && e.actionId == "handle"
                   && e.success;
        }
    }
}