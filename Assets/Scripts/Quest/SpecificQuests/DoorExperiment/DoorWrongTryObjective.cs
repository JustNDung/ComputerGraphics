using Reward;

namespace Quest.SpecificQuests.DoorExperiment
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Quest/Door/Objectives/Try Wrong Position")]
    public class DoorWrongTryObjective : ObjectiveSO
    {
        public override bool Match(RewardEvent e)
        {
            return e.experimentId == "door"
                   && !e.success;
        }
    }
}